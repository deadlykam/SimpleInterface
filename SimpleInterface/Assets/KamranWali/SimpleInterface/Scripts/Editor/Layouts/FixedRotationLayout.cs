using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace KamranWali.SimpleInterface.Editor.Layouts
{
    public class FixedRotationLayout : BaseLayout
    {
        private float _fixedRotX;
        private float _fixedRotY;
        private float _fixedRotZ;
        private AnimBool _fixedRotGroup;
        private AnimBool _fixedRotGroupX;
        private AnimBool _fixedRotGroupY;
        private AnimBool _fixedRotGroupZ;

        public FixedRotationLayout(UnityAction repaint) : base(repaint){}

        public override bool IsShown() => IsToggleGroupShown(_fixedRotGroup.faded);

        public override void SetupOnGUI()
        {
            _fixedRotGroup.target = ToggleLeft("Fixed Rotation", "Toggle to place prefab in given fixed Rotation axis. Hotkey = 'O'", _fixedRotGroup);

            if (BeginFadeGroup(_fixedRotGroup.faded))
            {
                BeginHorizontalLayout(ref _fixedRotGroupX, ref _fixedRotX, "X", "Toggle to keep the X rotation fixed", 50f, 1f);
                BeginHorizontalLayout(ref _fixedRotGroupY, ref _fixedRotY, "Y", "Toggle to keep the Y rotation fixed", 50f, 1f);
                BeginHorizontalLayout(ref _fixedRotGroupZ, ref _fixedRotZ, "Z", "Toggle to keep the Z rotation fixed", 50f, 1f);
            }
            EndFadeGroup();
        }

        public override void Update(Event currentEvent){ if (currentEvent.keyCode == KeyCode.O && currentEvent.type == EventType.KeyDown) _fixedRotGroup.target = !_fixedRotGroup.target; }

        public override Quaternion GetRotation(Quaternion rotation)
        {
            rotation = Quaternion.Euler(IsToggleGroupShown(_fixedRotGroupX.faded) ? _fixedRotX : rotation.eulerAngles.x,
                                        IsToggleGroupShown(_fixedRotGroupY.faded) ? _fixedRotY : rotation.eulerAngles.y,
                                        IsToggleGroupShown(_fixedRotGroupZ.faded) ? _fixedRotZ : rotation.eulerAngles.z);
            return rotation;
        }

        protected override void SetupOnEnable(UnityAction repaint)
        {
            _fixedRotGroup = new AnimBool(false);
            _fixedRotGroup.valueChanged.AddListener(repaint);
            _fixedRotGroupX = new AnimBool(false);
            _fixedRotGroupX.valueChanged.AddListener(repaint);
            _fixedRotGroupY = new AnimBool(false);
            _fixedRotGroupY.valueChanged.AddListener(repaint);
            _fixedRotGroupZ = new AnimBool(false);
            _fixedRotGroupZ.valueChanged.AddListener(repaint);
        }
    }
}