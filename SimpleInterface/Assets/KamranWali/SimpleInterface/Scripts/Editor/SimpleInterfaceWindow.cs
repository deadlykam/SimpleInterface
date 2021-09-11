using KamranWali.SimpleInterface.Editor.Layouts;
using UnityEditor;
using UnityEngine;

namespace KamranWali.SimpleInterface.Editor
{
    public class SimpleInterfaceWindow : EditorWindow
    {
        private Vector2 _mainScroll; // The main scroll body that will contain all other elements
        private LayoutManager _manager;
        private BaseLayout _placementLayout;
        private BaseLayout _fixedPositionLayout;
        private BaseLayout _offsetPositionLayout;
        private BaseLayout _fixedRotationLayout;
        private BaseLayout _randomRotationLayout;
        private BaseLayout _fixedScaleLayout;
        private BaseLayout _randomScaleLayout;
        private BaseLayout _logoLayout; // Always at the bottom
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
            _mainScroll = EditorGUILayout.BeginScrollView(_mainScroll, GUILayout.ExpandWidth(true)); // Creating main scroll to contain all elements
            _manager.OnGUI();
            EditorGUILayout.EndScrollView(); // Ending main scroll
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

            /*===Initializing Layouts===*/
            _offsetPositionLayout = new OffsetPositionLayout(Repaint);
            _placementLayout = new PlacementLayout(Repaint, GetActualPosition, GetActualRotation, GetActualScale, _offsetPositionLayout.IsShown);
            _fixedPositionLayout = new FixedPositionLayout(Repaint);
            _fixedRotationLayout = new FixedRotationLayout(Repaint);
            _randomRotationLayout = new RandomRotationLayout(Repaint);
            _fixedScaleLayout = new FixedScaleLayout(Repaint);
            _randomScaleLayout = new RandomScaleLayout(Repaint);
            _logoLayout = new LogoLayout(Repaint);

            /*===Linking Opposite Layouts===*/
            _fixedPositionLayout.AddHideLayout(_offsetPositionLayout);
            _fixedRotationLayout.AddHideLayout(_randomRotationLayout);
            _fixedScaleLayout.AddHideLayout(_randomScaleLayout);

            /*===Adding Layouts===*/
            _manager.AddLayout(_placementLayout);
            _manager.AddLayout(_fixedPositionLayout);
            _manager.AddLayout(_offsetPositionLayout);
            _manager.AddLayout(_fixedRotationLayout);
            _manager.AddLayout(_randomRotationLayout);
            _manager.AddLayout(_fixedScaleLayout);
            _manager.AddLayout(_randomScaleLayout);
            _manager.AddLayout(_logoLayout); // Always at the bottom
        }

        /// <summary>
        /// This method gets the actual position for placing the prefab.
        /// </summary>
        /// <param name="position">The position to be modified, of type Vector3</param>
        /// <returns>The actual placement position, of type Vector3</returns>
        private Vector3 GetActualPosition(Vector3 position)
        {
            if (_fixedPositionLayout.IsShown()) return _fixedPositionLayout.GetPosition(position); // Returning Fixed Position
            else if (_offsetPositionLayout.IsShown()) return _offsetPositionLayout.GetPosition(position); // Returning Offset Position
            return position; // Returning the actual position
        }

        /// <summary>
        /// This method gets the actual rotation for the placement prefab.
        /// </summary>
        /// <param name="rotation">The rotation to be modified, of type Quaternion</param>
        /// <returns>The actual placement rotation, of type Quaternion</returns>
        private Quaternion GetActualRotation(Quaternion rotation)
        {
            if (_fixedRotationLayout.IsShown()) return _fixedRotationLayout.GetRotation(rotation); // Returning Fixed Rotation
            else if (_randomRotationLayout.IsShown()) return _randomRotationLayout.GetRotation(rotation); // Returning Random Rotation
            return rotation;
        }

        /// <summary>
        /// This method gets the actual scale for the placement prefab.
        /// </summary>
        /// <param name="scale">The scale to be modified, of type Vector3</param>
        /// <returns>The actual placement scale, of type Vector3</returns>
        private Vector3 GetActualScale(Vector3 scale)
        {
            if (_fixedScaleLayout.IsShown()) return _fixedScaleLayout.GetScale(scale);
            else if (_randomScaleLayout.IsShown()) return _randomScaleLayout.GetScale(scale);
            return scale;
        }
    }
}