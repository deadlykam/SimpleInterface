using UnityEditor;
using UnityEngine;

namespace KamranWali.SimpleInterface.Editor
{
    public class SimpleInterfaceWindow : EditorWindow
    {
        // Input Fields
        private Transform _prefab;
        private Transform _root;
        private LayerMask _layerMask;
        private bool _togglePlacement;

        // Local Fields
        private Event _event; // Storing current events
        private Transform _prefabTemp; // For storing created prefabs
        private RaycastHit _hit; // Storing ray hit

        [MenuItem("KamranWali/SimpleInterfaceWindow")]
        private static void Init()
        {
            SimpleInterfaceWindow window = (SimpleInterfaceWindow)EditorWindow.GetWindow(typeof(SimpleInterfaceWindow));
            window.Show();
        }

        private void OnEnable() => SceneView.duringSceneGui += UpdateLocal; // Adding update method
        private void OnDisable() => SceneView.duringSceneGui -= UpdateLocal; // Removing update method

        private void OnGUI()
        {
            GUILayout.Label("SimpleInterfaceWindow", EditorStyles.boldLabel);
            _prefab = EditorGUILayout.ObjectField("Prefab", _prefab, typeof(Transform), false) as Transform;
            _root = EditorGUILayout.ObjectField(new GUIContent("Root", "The root into which the prefab will be placed. Keeping null means default root will be used."), _root, typeof(Transform), true) as Transform;
            _layerMask = EditorGUILayout.LayerField("Collidable Layer", _layerMask);
            //_togglePlacement = EditorGUILayout.Toggle(new GUIContent("Place Prefab", "Enable to place prefab. Hotkey = 'U'"), _togglePlacement, "button");
            _togglePlacement = EditorGUILayout.Toggle(_togglePlacement, "button");
        }

        /// <summary>
        /// This method updates the SimpleInterfaceWindow
        /// </summary>
        /// <param name="sceneView">The current scene view, of type SceneView </param>
        private void UpdateLocal(SceneView sceneView)
        {
            _event = Event.current;

            if(_togglePlacement && _event.type == EventType.MouseDown && _event.button == 0) // Checking if left mouse button pressed
            {
                if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(_event.mousePosition), out _hit, Mathf.Infinity, 1 << _layerMask)) // Hitting the correct layer
                {
                    _prefabTemp = _root == null ? PrefabUtility.InstantiatePrefab(_prefab) as Transform : PrefabUtility.InstantiatePrefab(_prefab, _root) as Transform; // Creating the prefab
                    _prefabTemp.position = _hit.point; // Placing in hit position
                    Undo.RegisterCreatedObjectUndo(_prefabTemp.gameObject, "Prefab Placement");
                }
            }

            if (_event.keyCode == KeyCode.U && _event.type == EventType.KeyDown) // Toggling placement
            {
                _togglePlacement = !_togglePlacement; // Toggling placement
                Repaint(); // Updating the window's inspector
            }

            //TODO: For dragging Tools.current = Tool.View and use MouseDrag
        }
    }
}