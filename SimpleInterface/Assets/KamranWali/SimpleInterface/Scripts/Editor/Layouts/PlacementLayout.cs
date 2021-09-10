using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace KamranWali.SimpleInterface.Editor.Layouts
{
    public class PlacementLayout : BaseLayout
    {
        // Input Feilds
        private string _path;
        private Transform _root;
        private LayerMask _layerMask;
        private AnimBool _placeGroup;
        private AnimBool _placeLimitGroup;
        private int _maxPlace;
        private Vector2 _prefabsScroll;
        private int _selPrefabGrid;
        private int _selPath;
        private UnityEditor.Editor _preview;
        private GUIStyle _previewColour;
        private GameObject _previewPrefab; // For storing the selected prefab

        // Internal Fields
        private RaycastHit _hit; // Storing ray hit
        private Transform _prefabValidate;
        private Transform _prefabTemp; // For storing created prefabs
        private Func<Vector3, Vector3> _getActualPosition;
        private Func<Quaternion, Quaternion> _getActualRotation;
        private Func<Vector3, Vector3> _getActualScale;
        private int _curPlace;
        private string[] _paths;
        private string[] _objectNames;
        private int _curPath;
        private List<Transform> _prefabs;
        private List<string> _prefabsNames;
        private int _prefabCounter;
        private PrefabPathSearch _prefabSearch;
        private readonly int _defaultMinPlace; // The minimum default value for placement limit
        private readonly string _defaultPath; // The default path if no path given

        /// <summary>
        /// This constructor creates the PlacementLayout object.
        /// </summary>
        /// <param name="repaint">For repainting the GUI, of type UnityAction</param>
        /// <param name="getActualPosition">The delegate that returns the actual position, of type Func<Vector3, Vector3></param>
        /// <param name="getActualRotation">The delegate that returns the actual rotation, of type Func<Quaternion, Quaternion></param>
        /// <param name="getActualScale">The delegate that returns the actual scale, of type Func<Vector3, Vector3></param>
        public PlacementLayout(UnityAction repaint, Func<Vector3, Vector3> getActualPosition, Func<Quaternion, Quaternion> getActualRotation, Func<Vector3, Vector3> getActualScale) : base(repaint)
        {
            _getActualPosition = getActualPosition;
            _getActualRotation = getActualRotation;
            _getActualScale = getActualScale;
            _prefabSearch = new PrefabPathSearch();
            _prefabs = new List<Transform>();
            _prefabsNames = new List<string>();
            _defaultMinPlace = 1; // Setting the default min placement limit value
            _defaultPath = "Assets";
        }

        public override bool IsShown() => _placeGroup.target;

        public override void SetupOnGUI()
        {
            _placeGroup.target = ToggleLeft("Place Prefab (U)", "Toggle to place prefab. Hotkey = 'U'", _placeGroup);
            if (BeginFadeGroup(_placeGroup.faded))
            {
                BeginHorizontal();
                Space(20f);
                _path = TextField("Path", "The path is the search location point from which all the prefabs will be found. Keeping it empty will start from 'Assets' folder which is the main folder. Providing a location point will reduce the time taken for searching.", _path);
                EndHorizontal();

                BeginHorizontal();
                Space(20f);
                if (Button("Load Prefabs", "This button will search and load all the prefabs."))
                {
                    _prefabSearch.SearchAllPrefabPaths(string.IsNullOrEmpty(_path) ? _defaultPath : _path);
                    _paths = _prefabSearch.GetPrefabPaths();
                    _selPath = 0; // Resetting the drop down selection
                    _curPath = -1; // Setting the prefabs to be updated in the first change call
                    if (_paths.Length == 0 && _prefabs.Count != 0) ClearPrefabs(); // Clearing the prefab and prefab name list
                }
                EndHorizontal();

                BeginHorizontal();
                Space(20f);
                LabelWidth(90f);
                _selPath = Popup(_selPath, "Prefab Paths", "Select a prefab path to load prefabs from.", IsPathsFound() ? _prefabSearch.GetPathNames() : new string[] { "None" });
                
                if (IsPathsFound() && _curPath != _selPath) // Condition for loading the prefabs
                {
                    LoadPrefabs(_paths[_selPath]);
                    _curPath = _selPath; // Updating the current path index
                }

                EndHorizontal();

                Space(10f);
                _prefabsScroll = BeginScrollView(_prefabsScroll, 100f);
                _selPrefabGrid = SelectionGrid(_selPrefabGrid, _prefabsNames.ToArray(), 2);
                EndScrollView();

                #region Prefab Preview
                if (IsPathsFound()) // Condition for showing preview
                {
                    if (_previewPrefab != GetSelectedPrefab().gameObject) // Checking if selection have changed.
                    {
                        _previewPrefab = GetSelectedPrefab().gameObject; // Updating preview prefab

                        if (_previewPrefab != null) // Checking if the preview prefab is NOT null
                        {
                            if (_preview != null) UnityEditor.Editor.DestroyImmediate(_preview); // Destroying previous editor to avoid allocate cull masking issue
                            if (_preview == null) _preview = UnityEditor.Editor.CreateEditor(_previewPrefab); // Creating a new editor with the updated prefab view
                        }
                    }

                    Space(10f);
                    if (_preview != null) _preview.OnPreviewGUI(GUILayoutUtility.GetRect(256f, 256f), _previewColour); // Showing the prefab preview
                    Space(10f);
                }
                #endregion

                BeginHorizontal();
                Space(20f);
                LabelWidth(100f);
                _root = TransformField("Root", "The root into which the prefab will be placed. Keeping null means default root will be used.", _root, true);
                EndHorizontal();

                BeginHorizontal();
                Space(20f);
                LabelWidth(100f);
                _layerMask = LayerField("Collidable Layer", "The layer on which the prefab will be placed.", _layerMask);
                EndHorizontal();

                // Limit group layout
                BeginHorizontal();
                Space(20f);
                _placeLimitGroup.target = ToggleLeft("Limit Placement (B)", "Toggle to place a limited number of prefabs. Hotkey = 'B'", _placeLimitGroup);

                if (BeginFadeGroup(_placeLimitGroup.faded)) 
                {
                    // Max Place Int Field
                    BeginVertical();
                    Space(20f);
                    BeginHorizontal();
                    Space(-180f);
                    LabelWidth(30f);
                    _maxPlace = (_maxPlace = IntField("Max", "The total number of placement allowed", _maxPlace)) < _defaultMinPlace ? _defaultMinPlace : _maxPlace;
                    EndHorizontal();
                    EndVertical();

                    // Remain Label
                    BeginVertical();
                    BeginHorizontal();
                    Space(-180f);
                    LabelWidth(30f);
                    LabelField($"Placed: {_curPlace.ToString()}, Left: {(_maxPlace - _curPlace).ToString()}", "This shows the number of objects placed and left to place.", EditorStyles.boldLabel);
                    EndHorizontal();
                    EndVertical();

                    // Reset Button
                    BeginVertical();
                    BeginHorizontal();
                    Space(-180f);
                    LabelWidth(30f);
                    if (Button("Reset(N)", "Resets placed counter. Hotkey = 'N'")) ResetPlacementCounter();
                    EndHorizontal();
                    EndVertical();
                }
                EndFadeGroup();
                EndHorizontal();
            }
            EndFadeGroup();
            HideOtherLayouts(); // Hidding other layouts
        }

        public override void Update(Event currentEvent)
        {
            if (IsToggleGroupShown(_placeGroup.faded) && currentEvent.type == EventType.MouseDown && currentEvent.button == 0 && IsPlaceable())
            {
                if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition), out _hit, Mathf.Infinity, 1 << _layerMask)) // Hitting the correct layer
                {
                    _prefabTemp = _root == null ? PrefabUtility.InstantiatePrefab(GetSelectedPrefab()) as Transform : PrefabUtility.InstantiatePrefab(GetSelectedPrefab(), _root) as Transform; // Creating the prefab
                    _prefabTemp.position = _getActualPosition(_hit.point); // Placing in hit position
                    _prefabTemp.rotation = _getActualRotation(_prefabTemp.rotation); // Rotating to the actual rotation
                    _prefabTemp.localScale = _getActualScale(_prefabTemp.localScale); // Setting the actual scale

                    if (_placeLimitGroup.target) // Condition to check if limit placement mode activated
                    {
                        _curPlace = (_curPlace + 1) > _maxPlace ? _maxPlace : _curPlace + 1;
                        repaint(); // Repainting to update the UI
                    }

                    Undo.RegisterCreatedObjectUndo(_prefabTemp.gameObject, "Prefab Placement");
                }
            }

            if (currentEvent.keyCode == KeyCode.U && currentEvent.type == EventType.KeyDown)
            {
                _placeGroup.target = !_placeGroup.target; // Toggling placement
                HideOtherLayouts(); // Hidding other layouts
            }
            else if (currentEvent.keyCode == KeyCode.B && currentEvent.type == EventType.KeyDown) _placeLimitGroup.target = !_placeLimitGroup.target;
            else if (currentEvent.keyCode == KeyCode.N && currentEvent.type == EventType.KeyDown && _placeLimitGroup.target)
            {
                ResetPlacementCounter();
                repaint();
            }
        }

        public override void Hide() { if (IsShown()) _placeGroup.target = false; }

        protected override void SetupOnEnable()
        {
            _placeGroup = new AnimBool(true);
            _placeGroup.valueChanged.AddListener(repaint);
            _placeLimitGroup = new AnimBool(false);
            _placeLimitGroup.valueChanged.AddListener(repaint);
            _previewColour = new GUIStyle();
            _previewColour.normal.background = EditorGUIUtility.whiteTexture;
        }

        /// <summary>
        /// This method checks if a prefab is placeable.
        /// </summary>
        /// <returns>True means placeable, false otherwise, of type bool</returns>
        private bool IsPlaceable() => (_placeLimitGroup.target ? _curPlace < _maxPlace : true) && _prefabs.Count != 0;

        /// <summary>
        /// This method resets the placement counter.
        /// </summary>
        private void ResetPlacementCounter() => _curPlace = 0;

        /// <summary>
        /// This method loads a new set of prefabs.
        /// </summary>
        /// <param name="path">The path from which to load the prefabs, of type string</param>
        private void LoadPrefabs(string path)
        {
            ClearPrefabs(); // Clearing the prefab and prefab name list

            if (Directory.Exists(path)) // Checking if the path exists
            {
                _objectNames = Directory.GetFiles(path, "*.prefab"); // Getting all object names

                if (_objectNames != null && _objectNames.Length > 0) // Checking if any object found
                {
                    for (_prefabCounter = 0; _prefabCounter < _objectNames.Length; _prefabCounter++) // Loop for validating path and adding prefabs
                    {
                        _objectNames[_prefabCounter] = _objectNames[_prefabCounter].Replace("\\", "/");
                        _prefabValidate = AssetDatabase.LoadAssetAtPath(_objectNames[_prefabCounter], typeof(Transform)) as Transform;

                        if (_prefabValidate != null) // Checking if the current object is NOT null and a prefab.
                        {
                            _prefabs.Add(_prefabValidate);
                            _prefabsNames.Add(_prefabValidate.name);
                        }
                    }
                    _selPrefabGrid = 0; // Setting selection to the first selection
                }
            }
        }

        /// <summary>
        /// This method clears the prefab and prefab name lists.
        /// </summary>
        private void ClearPrefabs()
        {
            _prefabs.Clear();
            _prefabsNames.Clear();
        }

        /// <summary>
        /// This method checks if the path has been loaded.
        /// </summary>
        /// <returns>True means loaded, false otherwise, of type bool</returns>
        private bool IsPathsFound() => _paths != null && _paths.Length != 0;

        /// <summary>
        /// This method gets the selected prefab.
        /// </summary>
        /// <returns>The selected prefab, of type Transform</returns>
        private Transform GetSelectedPrefab() => _prefabs[_selPrefabGrid];
    }
}