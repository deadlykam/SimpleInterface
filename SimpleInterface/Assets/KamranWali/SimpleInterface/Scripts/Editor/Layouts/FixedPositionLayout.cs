using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace KamranWali.SimpleInterface.Editor.Layouts
{
    public class FixedPositionLayout : BaseLayout
    {
        private float _fixedPosX;
        private float _fixedPosY;
        private float _fixedPosZ;
        private AnimBool _fixedPosGroup;
        private AnimBool _fixedPosGroupX;
        private AnimBool _fixedPosGroupY;
        private AnimBool _fixedPosGroupZ;

        public FixedPositionLayout(UnityAction repaint) : base(repaint){}

        public override bool IsShown() => _fixedPosGroup.target;

        public override void SetupOnGUI()
        {
            _fixedPosGroup.target = ToggleLeft("Fixed Position (I)", "Toggle to place prefab in given fixed Vector3 axis. Hotkey = 'I'", _fixedPosGroup);

            if (BeginFadeGroup(_fixedPosGroup.faded))
            {
                BeginHorizontalLayout(ref _fixedPosGroupX, ref _fixedPosX, "X", "Toggle to keep the X axis fixed", 20f, 1f);
                BeginHorizontalLayout(ref _fixedPosGroupY, ref _fixedPosY, "Y", "Toggle to keep the Y axis fixed", 20f, 1f);
                BeginHorizontalLayout(ref _fixedPosGroupZ, ref _fixedPosZ, "Z", "Toggle to keep the Z axis fixed", 20f, 1f);
            }
            EndFadeGroup();
            HideOtherLayouts(); // Hidding other layouts
        }

        public override void Update(Event currentEvent) 
        {
            if (currentEvent.keyCode == KeyCode.I && currentEvent.type == EventType.KeyDown)
            {
                _fixedPosGroup.target = !_fixedPosGroup.target;
                HideOtherLayouts(); // Hidding other layouts
            }
        }

        public override void Hide() { if (IsShown()) _fixedPosGroup.target = false; }

        public override Vector3 GetPosition(Vector3 position)
        {
            position.Set(IsToggleGroupShown(_fixedPosGroupX.faded) ? _fixedPosX : position.x,
                         IsToggleGroupShown(_fixedPosGroupY.faded) ? _fixedPosY : position.y,
                         IsToggleGroupShown(_fixedPosGroupZ.faded) ? _fixedPosZ : position.z);
            return position;
        }

        protected override void SetupOnEnable()
        {
            _fixedPosGroup = new AnimBool(false);
            _fixedPosGroup.valueChanged.AddListener(repaint);
            _fixedPosGroupX = new AnimBool(false);
            _fixedPosGroupX.valueChanged.AddListener(repaint);
            _fixedPosGroupY = new AnimBool(false);
            _fixedPosGroupY.valueChanged.AddListener(repaint);
            _fixedPosGroupZ = new AnimBool(false);
            _fixedPosGroupZ.valueChanged.AddListener(repaint);
        }
    }
}