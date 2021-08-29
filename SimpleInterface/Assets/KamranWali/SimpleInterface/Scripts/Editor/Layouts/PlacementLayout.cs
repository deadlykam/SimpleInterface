using System;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace KamranWali.SimpleInterface.Editor.Layouts
{
    public class PlacementLayout : BaseLayout
    {
        private Transform _prefab;
        private Transform _root;
        private LayerMask _layerMask;
        private AnimBool _placeGroup;
        private RaycastHit _hit; // Storing ray hit
        private Transform _prefabTemp; // For storing created prefabs

        public override bool IsShown() => IsToggleGroupShown(_placeGroup.faded);

        public override void SetupOnEnable(UnityAction repaint)
        {
            _placeGroup = new AnimBool(true);
            _placeGroup.valueChanged.AddListener(repaint);
        }

        public override void SetupOnGUI()
        {
            _placeGroup.target = ToggleLeft("Place Prefab", "Toggle to place prefab. Hotkey = 'U'", _placeGroup);
            if (BeginFadeGroup(_placeGroup.faded))
            {
                _prefab = TransformField("Prefab", "The prefab to spawn", _prefab, false);
                _root = TransformField("Root", "The root into which the prefab will be placed. Keeping null means default root will be used.", _root, true);
                _layerMask = LayerField("Collidable Layer", "The layer on which the prefab will be placed.", _layerMask);
            }
            EndFadeGroup();
        }

        public override void Update(Event currentEvent)
        {
            if (IsToggleGroupShown(_placeGroup.faded) && currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
            {
                if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition), out _hit, Mathf.Infinity, 1 << _layerMask)) // Hitting the correct layer
                {
                    _prefabTemp = _root == null ? PrefabUtility.InstantiatePrefab(_prefab) as Transform : PrefabUtility.InstantiatePrefab(_prefab, _root) as Transform; // Creating the prefab
                    _prefabTemp.position = /*GetActualPosition(_hit.point)*/ _hit.point; // Placing in hit position
                    Undo.RegisterCreatedObjectUndo(_prefabTemp.gameObject, "Prefab Placement");
                }
            }

            if (currentEvent.keyCode == KeyCode.U && currentEvent.type == EventType.KeyDown) _placeGroup.target = !_placeGroup.target; // Toggling placement
        }
    }
}