using KamranWali.SimpleInterface.Editor.Layouts;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace KamranWali.SimpleInterface.Editor
{
    public class SimpleInterfaceWindow : EditorWindow
    {
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
            SetupGroups();
        }

        private void OnDisable() => SceneView.duringSceneGui -= UpdateLocal; // Removing update method

        private void OnGUI()
        {
            GUILayout.Label("SimpleInterfaceWindow", EditorStyles.boldLabel);
            _placementLayout.SetupOnGUI();
            _fixedPositionLayout.SetupOnGUI();
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
            //TODO: For dragging Tools.current = Tool.View and use MouseDrag
        }

        /// <summary>
        /// This method sets up all the group related feilds.
        /// </summary>
        private void SetupGroups() 
        {
            _placementLayout = new PlacementLayout(GetActualPosition);
            _placementLayout.SetupOnEnable(Repaint);

            _fixedPositionLayout = new FixedPositionLayout();
            _fixedPositionLayout.SetupOnEnable(Repaint);
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