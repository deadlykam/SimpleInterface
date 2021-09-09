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
        private string _path;
        private Transform _root;
        private LayerMask _layerMask;
        private AnimBool _placeGroup;
        private AnimBool _placeLimitGroup;
        private int _maxPlace;
        private Vector2 _prefabsScroll;
        private int _selPrefabGrid;
        private RaycastHit _hit; // Storing ray hit
        private Transform _prefabValidate;
        private Transform _prefabTemp; // For storing created prefabs
        private Func<Vector3, Vector3> _getActualPosition;
        private Func<Quaternion, Quaternion> _getActualRotation;
        private Func<Vector3, Vector3> _getActualScale;
        private int _curPlace;
        private readonly int _defaultMinPlace; // The minimum default value for placement limit
        private string[] _objectNames;
        private List<Transform> _prefabs;
        private List<string> _prefabsNames;
        private int _prefabCounter;

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
            _defaultMinPlace = 1; // Setting the default min placement limit value

            _prefabs = new List<Transform>();
            _prefabsNames = new List<string>();
        }

        public override bool IsShown() => _placeGroup.target;

        public override void SetupOnGUI()
        {
            _placeGroup.target = ToggleLeft("Place Prefab (U)", "Toggle to place prefab. Hotkey = 'U'", _placeGroup);
            if (BeginFadeGroup(_placeGroup.faded))
            {
                BeginHorizontal();
                Space(20f);
                _path = TextField("Path", "The path is the folder containing the prefabs", _path);
                EndHorizontal();

                BeginHorizontal();
                Space(20f);
                if (Button("Load Prefabs", "This button will load all the prefabs in the give path.")) LoadPrefabs();
                EndHorizontal();

                Space(10f);
                _prefabsScroll = BeginScrollView(_prefabsScroll, 100f);
                _selPrefabGrid = SelectionGrid(_selPrefabGrid, _prefabsNames.ToArray(), 2);
                EndScrollView();

                BeginHorizontal();
                Space(20f);
                _root = TransformField("Root", "The root into which the prefab will be placed. Keeping null means default root will be used.", _root, true);
                EndHorizontal();

                BeginHorizontal();
                Space(20f);
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
                    _prefabTemp = _root == null ? PrefabUtility.InstantiatePrefab(_prefabs[_selPrefabGrid]) as Transform : PrefabUtility.InstantiatePrefab(_prefabs[_selPrefabGrid], _root) as Transform; // Creating the prefab
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
        private void LoadPrefabs()
        {
            _prefabs.Clear(); // Clearing all prefabs
            _prefabsNames.Clear(); // Clearing all prefab names

            if (Directory.Exists(_path)) // Checking if the path exists
            {
                _objectNames = Directory.GetFiles(_path); // Getting all object names

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
    }
}