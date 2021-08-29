using KamranWali.SimpleInterface.Editor.Layouts;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace KamranWali.SimpleInterface.Editor
{
    public class SimpleInterfaceWindow : EditorWindow
    {
        // Input Fields
        private BaseLayout _placementLayout;
        private BaseLayout _fixedPositionLayout;

        /*private Transform _prefab;
        private Transform _root;
        private LayerMask _layerMask;*/
        /*private float _fixedPosX;
        private float _fixedPosY;
        private float _fixedPosZ;*/

        // Group Fields
        //private AnimBool _placeGroup;
        /*private AnimBool _fixedPosGroup;
        private AnimBool _fixedPosGroupX;
        private AnimBool _fixedPosGroupY;
        private AnimBool _fixedPosGroupZ;*/

        // Local Fields
        private Event _event; // Storing current events
        /*private Transform _prefabTemp; // For storing created prefabs
        private RaycastHit _hit; // Storing ray hit*/
        private Vector3 _actualPos; // The actual position to place the object in

        [MenuItem("KamranWali/SimpleInterfaceWindow")]
        private static void Init()
        {
            SimpleInterfaceWindow window = (SimpleInterfaceWindow) EditorWindow.GetWindow(typeof(SimpleInterfaceWindow));
            window.Show();
        }

        private void OnEnable()
        {
            SceneView.duringSceneGui += UpdateLocal; // Adding update method
            SetupGroups();
        }

        private void OnDisable() => SceneView.duringSceneGui -= UpdateLocal; // Removing update method

        private void OnGUI()
        {
            GUILayout.Label("SimpleInterfaceWindow", EditorStyles.boldLabel);
            _placementLayout.SetupOnGUI();
            _fixedPositionLayout.SetupOnGUI();

            /*_placeGroup.target = EditorGUILayout.ToggleLeft(new GUIContent("Place Prefab", "Toggle to place prefab. Hotkey = 'U'"), _placeGroup.target);
            if (EditorGUILayout.BeginFadeGroup(_placeGroup.faded)) // Placement group
            {
                _prefab = EditorGUILayout.ObjectField("Prefab", _prefab, typeof(Transform), false) as Transform;
                _root = EditorGUILayout.ObjectField(new GUIContent("Root", "The root into which the prefab will be placed. Keeping null means default root will be used."), _root, typeof(Transform), true) as Transform;
                _layerMask = EditorGUILayout.LayerField("Collidable Layer", _layerMask);
            }
            EditorGUILayout.EndFadeGroup();*/

            /*_fixedPosGroup.target = EditorGUILayout.ToggleLeft(new GUIContent("Fixed Position", "Toggle to place prefab in given fixed Vector3 axis. Hotkey = 'I'"), _fixedPosGroup.target);
            if (EditorGUILayout.BeginFadeGroup(_fixedPosGroup.faded))
            {
                // X Layout
                GUILayout.BeginHorizontal();
                GUILayout.Space(50);
                EditorGUIUtility.labelWidth = 1f;
                _fixedPosGroupX.target = EditorGUILayout.ToggleLeft(new GUIContent("X", "Toggle to keep the X axis fixed"), _fixedPosGroupX.target);
                if (EditorGUILayout.BeginFadeGroup(_fixedPosGroupX.faded)) _fixedPosX = EditorGUILayout.FloatField(_fixedPosX);
                EditorGUILayout.EndFadeGroup();
                GUILayout.EndHorizontal();

                // Y Layout
                GUILayout.BeginHorizontal();
                GUILayout.Space(50);
                EditorGUIUtility.labelWidth = 1f;
                _fixedPosGroupY.target = EditorGUILayout.ToggleLeft(new GUIContent("Y", "Toggle to keep the Y axis fixed"), _fixedPosGroupY.target);
                if (EditorGUILayout.BeginFadeGroup(_fixedPosGroupY.faded)) _fixedPosY = EditorGUILayout.FloatField(_fixedPosY);
                EditorGUILayout.EndFadeGroup();
                GUILayout.EndHorizontal();

                // Z Layout
                GUILayout.BeginHorizontal();
                GUILayout.Space(50);
                EditorGUIUtility.labelWidth = 1f;
                _fixedPosGroupZ.target = EditorGUILayout.ToggleLeft(new GUIContent("Z", "Toggle to keep the Z axis fixed"), _fixedPosGroupZ.target);
                if (EditorGUILayout.BeginFadeGroup(_fixedPosGroupZ.faded)) _fixedPosZ = EditorGUILayout.FloatField(_fixedPosZ);
                EditorGUILayout.EndFadeGroup();
                GUILayout.EndHorizontal();
            }
            EditorGUILayout.EndFadeGroup();*/
        }

        /// <summary>
        /// This method updates the SimpleInterfaceWindow
        /// </summary>
        /// <param name="sceneView">The current scene view, of type SceneView </param>
        private void UpdateLocal(SceneView sceneView)
        {
            _event = Event.current;

            _placementLayout.Update(_event);
            _fixedPositionLayout.Update(_event);

            /*if(IsToggleGroupShown(_placeGroup.faded) && _event.type == EventType.MouseDown && _event.button == 0) // Checking if left mouse button pressed
            {
                if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(_event.mousePosition), out _hit, Mathf.Infinity, 1 << _layerMask)) // Hitting the correct layer
                {
                    _prefabTemp = _root == null ? PrefabUtility.InstantiatePrefab(_prefab) as Transform : PrefabUtility.InstantiatePrefab(_prefab, _root) as Transform; // Creating the prefab
                    _prefabTemp.position = GetActualPosition(_hit.point); // Placing in hit position
                    Undo.RegisterCreatedObjectUndo(_prefabTemp.gameObject, "Prefab Placement");
                }
            }

            if (_event.keyCode == KeyCode.U && _event.type == EventType.KeyDown) _placeGroup.target = !_placeGroup.target; // Toggling placement*/
            //if (_event.keyCode == KeyCode.I && _event.type == EventType.KeyDown) _fixedPosGroup.target = !_fixedPosGroup.target; // Toggling fixed position

            //TODO: For dragging Tools.current = Tool.View and use MouseDrag
        }

        /// <summary>
        /// This method sets up all the group related feilds.
        /// </summary>
        private void SetupGroups() 
        {
            _placementLayout = new PlacementLayout();
            _placementLayout.SetupOnEnable(Repaint);

            _fixedPositionLayout = new FixedPositionLayout();
            _fixedPositionLayout.SetupOnEnable(Repaint);

            /*_placeGroup = new AnimBool(true);
            _placeGroup.valueChanged.AddListener(Repaint);*/
            /*_fixedPosGroup = new AnimBool(false);
            _fixedPosGroup.valueChanged.AddListener(Repaint);
            _fixedPosGroupX = new AnimBool(false);
            _fixedPosGroupX.valueChanged.AddListener(Repaint);
            _fixedPosGroupY = new AnimBool(false);
            _fixedPosGroupY.valueChanged.AddListener(Repaint);
            _fixedPosGroupZ = new AnimBool(false);
            _fixedPosGroupZ.valueChanged.AddListener(Repaint);*/
        }

        /// <summary>
        /// This method checks if the toggle group is shown.
        /// </summary>
        /// <param name="value">The value to check for if toggle group shown, value > 0.5 means shown, otherwise hidden, of type float</param>
        /// <returns>True means shown and value > 0.5, false means hidden and value <= 0.5, of type bool</returns>
        private bool IsToggleGroupShown(float value) => value > 0.5f;

        /// <summary>
        /// This method gets the actual position for placing the prefab.
        /// </summary>
        /// <param name="hitPoint">The collided point to be modified, of type Vector3</param>
        /// <returns>The actual placement position, of type Vector3</returns>
        private Vector3 GetActualPosition(Vector3 hitPoint)
        {
            _actualPos = hitPoint;

            /*if (IsToggleGroupShown(_fixedPosGroup.faded))
            {
                if (IsToggleGroupShown(_fixedPosGroupX.faded)) _actualPos.x = _fixedPosX;
                if (IsToggleGroupShown(_fixedPosGroupY.faded)) _actualPos.y = _fixedPosY;
                if (IsToggleGroupShown(_fixedPosGroupZ.faded)) _actualPos.z = _fixedPosZ;
            }*/

            return _actualPos; // Returning the actual position
        }
    }
}