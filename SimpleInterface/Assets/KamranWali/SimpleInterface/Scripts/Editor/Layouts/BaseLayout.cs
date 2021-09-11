using System;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace KamranWali.SimpleInterface.Editor.Layouts
{
    public abstract class BaseLayout
    {
        protected UnityAction repaint;
        private Action _hideLayouts; // Layouts that should be hidden when the this layout is shown

        /// <summary>
        /// This method creates the BaseLayout object
        /// </summary>
        /// <param name="repaint">For repainting the GUI, of type UnityAction</param>
        public BaseLayout(UnityAction repaint)
        {
            this.repaint = repaint;
            SetupOnEnable();
        }

        /// <summary>
        /// This method adds the hide method of the layout that should be hidden when the current layout is shown.
        /// </summary>
        /// <param name="hideLayout">The hide method of the layout to be hidden, of type Action</param>
        public void AddHideLayout(Action hideLayout) => _hideLayouts += hideLayout;

        /// <summary>
        /// This method makes the give layout hide when this layout is shown and vice versa.
        /// </summary>
        /// <param name="layout">The layout to hide when this layout is shown and vice versa, of type BaseLayout</param>
        public void AddHideLayout(BaseLayout layout)
        {
            _hideLayouts += layout.Hide;
            layout.AddHideLayout(Hide);
        }

        /// <summary>
        /// This method gets the actual position.
        /// </summary>
        /// <param name="position">The position used to get the actual position, of type Vector3</param>
        /// <returns>The actual position, of type Vector3</returns>
        public virtual Vector3 GetPosition(Vector3 position) => position;

        /// <summary>
        /// This method gets the actual rotation.
        /// </summary>
        /// <param name="rotation">The rotation used to get the actual rotation, of type Quaternion</param>
        /// <returns>The actual rotation, of type Quaternion</returns>
        public virtual Quaternion GetRotation(Quaternion rotation) => rotation;

        /// <summary>
        /// This method gets the actual scale.
        /// </summary>
        /// <param name="scale">The scale used to get the actual scale, of type Vector3</param>
        /// <returns>The actual scale, of type Vector3</returns>
        public virtual Vector3 GetScale(Vector3 scale) => scale;

        /// <summary>
        /// This method sets up the layout and MUST be called in OnGUI method.
        /// </summary>
        public abstract void SetupOnGUI();

        /// <summary>
        /// This method updates the layout and MUST be called in the custom update method of EditorWindow.
        /// </summary>
        /// <param name="currentEvent">The current event that took place, of type Event</param>
        public abstract void Update(Event currentEvent);

        /// <summary>
        /// This method checks if the layout is being shown.
        /// </summary>
        /// <returns>True means shown, false otherwise, of type bool</returns>
        public abstract bool IsShown();

        /// <summary>
        /// This method hides the layout.
        /// </summary>
        public abstract void Hide();

        /// <summary>
        /// This method hides the other layouts.
        /// </summary>
        protected void HideOtherLayouts() { if (IsShown()) _hideLayouts?.Invoke(); }

        /// <summary>
        /// This method initializes the layout's internal objects and MUST be called in OnEnable method.
        /// </summary>
        protected abstract void SetupOnEnable();

        #region Creation
        protected bool Toggle(string name, string tooltip, bool obj) => EditorGUILayout.Toggle(new GUIContent(name, tooltip), obj);
        protected bool ToggleLeft(string name, string toolTip, AnimBool toggle) => EditorGUILayout.ToggleLeft(new GUIContent(name, toolTip), toggle.target);
        protected Transform TransformField(string name, string toolTip, Transform obj, bool isHierarchy) => EditorGUILayout.ObjectField(new GUIContent(name, toolTip), obj, typeof(Transform), isHierarchy) as Transform;
        protected int LayerField(string name, string toolTip, LayerMask layerMask) => EditorGUILayout.LayerField(new GUIContent(name, toolTip), layerMask);
        protected float FloatField(string name, string toolTip, float obj) => EditorGUILayout.FloatField(new GUIContent(name, toolTip), obj);
        protected float FloatField(float obj) => EditorGUILayout.FloatField(obj);
        protected int IntField(string name, string toolTip, int obj) => EditorGUILayout.IntField(new GUIContent(name, toolTip), obj);
        protected int IntField(int obj) => EditorGUILayout.IntField(obj);
        protected void LabelField(string name) => EditorGUILayout.LabelField(name);
        protected void LabelField(string name, GUIStyle style) => EditorGUILayout.LabelField(name, style);
        protected void LabelField(string name, GUIStyle style, GUILayoutOption[] options) => EditorGUILayout.LabelField(name, style, options);
        protected void LabelField(string name, string tooltip) => EditorGUILayout.LabelField(new GUIContent(name, tooltip));
        protected void LabelField(string name, string tooltip, GUIStyle style) => EditorGUILayout.LabelField(new GUIContent(name, tooltip), style);
        protected string TextField(string name, string tooltip, string obj) => EditorGUILayout.TextField(new GUIContent(name, tooltip), obj);
        protected bool Button(string name, string tooltip) => GUILayout.Button(new GUIContent(name, tooltip));
        protected int SelectionGrid(int obj, string[] objs, int columns) => GUILayout.SelectionGrid(obj, objs, columns);
        protected Vector2 BeginScrollView(Vector2 obj, float width, float height) => EditorGUILayout.BeginScrollView(obj, GUILayout.Width(width), GUILayout.Height(height));
        protected Vector2 BeginScrollView(Vector2 obj, float height) => EditorGUILayout.BeginScrollView(obj, GUILayout.ExpandWidth(true), GUILayout.Height(height));
        protected int Popup(int obj, string name, string tooltip, string[] displayData) => EditorGUILayout.Popup(new GUIContent(name, tooltip), obj, displayData);
        protected void Box(Texture texture) => GUILayout.Box(texture);
        protected void Box(Texture texture, bool isExpand) => GUILayout.Box(texture, new GUILayoutOption[] { GUILayout.ExpandWidth(isExpand), GUILayout.ExpandHeight(isExpand) });
        protected void Box(Texture texture, float width, float height, bool isExpandWidth, bool isExpandHeight) => GUILayout.Box(texture, new GUILayoutOption[] { GUILayout.Width(width), GUILayout.Height(height), GUILayout.ExpandWidth(isExpandWidth), GUILayout.ExpandHeight(isExpandHeight) });
        #endregion

        #region Layout
        protected bool BeginFadeGroup(float value) => EditorGUILayout.BeginFadeGroup(value);
        protected void EndFadeGroup() => EditorGUILayout.EndFadeGroup();
        protected void BeginHorizontal() => GUILayout.BeginHorizontal();
        protected void EndHorizontal() => GUILayout.EndHorizontal();
        protected void BeginVertical() => GUILayout.BeginVertical();
        protected void EndVertical() => GUILayout.EndVertical();
        protected void EndScrollView() => EditorGUILayout.EndScrollView();
        protected void Space(float space) => GUILayout.Space(space);
        protected void LabelWidth(float width) => EditorGUIUtility.labelWidth = width;
        #endregion

        /// <summary>
        /// This method creates a horizontal layout.
        /// </summary>
        /// <param name="obj">The reference to the float, of type float</param>
        /// <param name="space">The space from the left, of type float</param>
        /// <param name="width">The gap between fields, of type float</param>
        protected void BeginHorizontalLayout(ref float obj, string name, string toolTip, float space, float width)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(space);
            EditorGUIUtility.labelWidth = width;
            obj = FloatField(name, toolTip, obj);
            EndHorizontal();
        }

        /// <summary>
        /// This method creates a horizontal layout.
        /// </summary>
        /// <param name="toggle">The reference to the toggle, of type Animbool</param>
        /// <param name="obj">The reference to the float, of type float</param>
        /// <param name="space">The space from the left, of type float</param>
        /// <param name="width">The gap between fields, of type float</param>
        protected void BeginHorizontalLayout(ref AnimBool toggle, ref float obj, string toggleName, string toggleToolTip, float space, float width)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(space);
            EditorGUIUtility.labelWidth = width;
            toggle.target = ToggleLeft(toggleName, toggleToolTip, toggle);
            if (BeginFadeGroup(toggle.faded)) obj = FloatField(obj);
            EndFadeGroup();
            EndHorizontal();
        }

        /// <summary>
        /// This method creates a horizontal layout.
        /// </summary>
        /// <param name="obj1">The reference to the float, of type float</param>
        /// <param name="obj2">The reference to the float, of type float</param>
        /// <param name="name1">The name of obj1, of type string</param>
        /// <param name="name2">The name of obj2, of type string</param>
        /// <param name="toolTip1">The tooltip for obj1, of type string</param>
        /// <param name="toolTip2">The tooltip for obj2, of type string</param>
        /// <param name="space">The space from the left, of type float</param>
        /// <param name="width">The gap between fields, of type float</param>
        protected void BeginHorizontalLayout(ref float obj1, ref float obj2, string name1, string name2, string toolTip1, string toolTip2, float space, float width)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(space);
            EditorGUIUtility.labelWidth = width;
            obj1 = FloatField(name1, toolTip1, obj1);
            obj2 = FloatField(name2, toolTip2, obj2);
            EndHorizontal();
        }

        /// <summary>
        /// This method creates a horizontal layout.
        /// </summary>
        /// <param name="toggle">The reference to the toggle, of type Animbool</param>
        /// <param name="obj1">The reference to the float, of type float</param>
        /// <param name="obj2">The reference to the float, of type float</param>
        /// <param name="space">The space from the left, of type float</param>
        /// <param name="width">The gap between fields, of type float</param>
        protected void BeginHorizontalLayout(ref AnimBool toggle, ref float obj1, ref float obj2, string toggleName, string toggleToolTip, float space, float width)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(space);
            EditorGUIUtility.labelWidth = width;
            toggle.target = ToggleLeft(toggleName, toggleToolTip, toggle);

            if (BeginFadeGroup(toggle.faded)) // Condition for showing the two floats value
            {
                obj1 = FloatField(obj1);
                obj2 = FloatField(obj2);
            }

            EndFadeGroup();
            EndHorizontal();
        }

        /// <summary>
        /// This method creates a horizontal layout.
        /// </summary>
        /// <param name="toggle">The reference to the toggle, of type Animbool</param>
        /// <param name="obj1">The reference to the float, of type float</param>
        /// <param name="obj2">The reference to the float, of type float</param>
        /// <param name="space">The space from the left, of type float</param>
        /// <param name="width">The gap between fields, of type float</param>
        protected void BeginHorizontalLayout(ref AnimBool toggle, ref float obj1, ref float obj2, string toggleName, string toggleToolTip, string name1, string name2, string toolTip1, string toolTip2, float space, float width)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(space);
            EditorGUIUtility.labelWidth = width;
            toggle.target = ToggleLeft(toggleName, toggleToolTip, toggle);

            if (BeginFadeGroup(toggle.faded)) // Condition for showing the two floats value
            {
                GUILayout.BeginHorizontal();
                obj1 = FloatField(name1, toolTip1, obj1);
                obj2 = FloatField(name2, toolTip2, obj2);
                EndHorizontal();
            }

            EndFadeGroup();
            EndHorizontal();
        }

        /// <summary>
        /// This method checks if the toggle group is shown.
        /// </summary>
        /// <param name="value">The value to check for if toggle group shown, value > 0.5 means shown, otherwise hidden, of type float</param>
        /// <returns>True means shown and value > 0.5, false means hidden and value <= 0.5, of type bool</returns>
        protected bool IsToggleGroupShown(float value) => value > 0.5f;
    }
}