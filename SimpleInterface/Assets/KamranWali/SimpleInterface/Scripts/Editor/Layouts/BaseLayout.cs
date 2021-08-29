using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace KamranWali.SimpleInterface.Editor.Layouts
{
    public abstract class BaseLayout
    {
        /// <summary>
        /// This method initializes the layout's internal objects and MUST be called in OnEnable method.
        /// </summary>
        /// <param name="repaint"></param>
        public abstract void SetupOnEnable(UnityAction repaint);

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

        #region Creation
        protected bool ToggleLeft(string name, string toolTip, AnimBool toggle) => EditorGUILayout.ToggleLeft(new GUIContent(name, toolTip), toggle.target);
        protected Transform TransformField(string name, string toolTip, Transform obj, bool isHierarchy) => EditorGUILayout.ObjectField(new GUIContent(name, toolTip), obj, typeof(Transform), isHierarchy) as Transform;
        protected int LayerField(string name, string toolTip, LayerMask layerMask) => EditorGUILayout.LayerField(new GUIContent(name, toolTip), layerMask);
        protected float FloatField(float obj) => EditorGUILayout.FloatField(obj);
        #endregion

        #region Layout
        protected bool BeginFadeGroup(float value) => EditorGUILayout.BeginFadeGroup(value);
        protected void EndFadeGroup() => EditorGUILayout.EndFadeGroup();
        protected void EndHorizontal() => GUILayout.EndHorizontal();
        #endregion

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
        /// This method checks if the toggle group is shown.
        /// </summary>
        /// <param name="value">The value to check for if toggle group shown, value > 0.5 means shown, otherwise hidden, of type float</param>
        /// <returns>True means shown and value > 0.5, false means hidden and value <= 0.5, of type bool</returns>
        protected bool IsToggleGroupShown(float value) => value > 0.5f;
    }
}