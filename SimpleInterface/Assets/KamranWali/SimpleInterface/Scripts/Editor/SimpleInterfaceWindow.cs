using KamranWali.SimpleInterface.Editor.Layouts;
using UnityEditor;
using UnityEngine;

namespace KamranWali.SimpleInterface.Editor
{
    public class SimpleInterfaceWindow : EditorWindow
    {
        private LayoutManager _manager;
        private BaseLayout _placementLayout;
        private BaseLayout _fixedPositionLayout;
        private Event _event; // Storing current events

        [MenuItem("KamranWali/SimpleInterfaceWindow")]
        private static void Init()
        {
            SimpleInterfaceWindow window = (SimpleInterfaceWindow) EditorWindow.GetWindow(typeof(SimpleInterfaceWindow));
            window.Show();
        }

        private void OnEnable()
        {
            SceneView.duringSceneGui += UpdateLocal; // Adding update method
            Setup();
        }

        private void OnDisable() => SceneView.duringSceneGui -= UpdateLocal; // Removing update method

        private void OnGUI()
        {
            GUILayout.Label("SimpleInterfaceWindow", EditorStyles.boldLabel);
            _manager.OnGUI();
        }

        /// <summary>
        /// This method updates the SimpleInterfaceWindow
        /// </summary>
        /// <param name="sceneView">The current scene view, of type SceneView </param>
        private void UpdateLocal(SceneView sceneView)
        {
            _event = Event.current;
            _manager.Update(_event);
            //TODO: For dragging Tools.current = Tool.View and use MouseDrag
        }

        /// <summary>
        /// This method sets up all the layouts and the manager.
        /// </summary>
        private void Setup() 
        {
            _manager = new LayoutManager();

            _placementLayout = new PlacementLayout(Repaint, GetActualPosition);
            _fixedPositionLayout = new FixedPositionLayout(Repaint);

            _manager.AddLayout(_placementLayout);
            _manager.AddLayout(_fixedPositionLayout);
        }

        /// <summary>
        /// This method gets the actual position for placing the prefab.
        /// </summary>
        /// <param name="position">The position to be modified, of type Vector3</param>
        /// <returns>The actual placement position, of type Vector3</returns>
        private Vector3 GetActualPosition(Vector3 position)
        {
            if (_fixedPositionLayout.IsShown()) return _fixedPositionLayout.GetPosition(position); // Returning Fixed Position
            return position; // Returning the actual position
        }
    }
}