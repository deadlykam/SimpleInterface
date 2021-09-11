using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace KamranWali.SimpleInterface.Editor.Layouts {
    public class RandomRotationLayout : BaseLayout
    {
        private float _rangeRotXMin;
        private float _rangeRotXMax;
        private float _rangeRotYMin;
        private float _rangeRotYMax;
        private float _rangeRotZMin;
        private float _rangeRotZMax;
        private AnimBool _randomRotGroup;
        private AnimBool _rangeRotGroupX;
        private AnimBool _rangeRotGroupY;
        private AnimBool _rangeRotGroupZ;

        public RandomRotationLayout(UnityAction repaint) : base(repaint)
        {
        }

        public override bool IsShown() => _randomRotGroup.target;

        public override void SetupOnGUI()
        {
            _randomRotGroup.target = ToggleLeft("Random Rotation (J)", "Toggle to place prefab in random Rotation axis. Hotkey = 'J'", _randomRotGroup);

            if (BeginFadeGroup(_randomRotGroup.faded))
            {
                BeginHorizontalLayout(ref _rangeRotGroupX, ref _rangeRotXMin, ref _rangeRotXMax, "X", "Toggle to randomize the X rotation. Min, Max.", "Min", "Max", "Minimum Value", "Maximum Value", 20f, 25f);
                BeginHorizontalLayout(ref _rangeRotGroupY, ref _rangeRotYMin, ref _rangeRotYMax, "Y", "Toggle to randomize the Y rotation. Min, Max.", "Min", "Max", "Minimum Value", "Maximum Value", 20f, 25f);
                BeginHorizontalLayout(ref _rangeRotGroupZ, ref _rangeRotZMin, ref _rangeRotZMax, "Z", "Toggle to randomize the Z rotation. Min, Max.", "Min", "Max", "Minimum Value", "Maximum Value", 20f, 25f);
            }
            EndFadeGroup();
            HideOtherLayouts(); // Hidding other layouts
        }

        public override void Update(Event currentEvent)
        {
            if (currentEvent.keyCode == KeyCode.J && currentEvent.type == EventType.KeyDown)
            {
                _randomRotGroup.target = !_randomRotGroup.target;
                HideOtherLayouts(); // Hidding other layouts
            }
        }

        public override void Hide() { if (IsShown()) _randomRotGroup.target = false; }

        public override Quaternion GetRotation(Quaternion rotation) => rotation = Quaternion.Euler(IsToggleGroupShown(_rangeRotGroupX.faded) ? Random.Range(_rangeRotXMin, _rangeRotXMax) : rotation.eulerAngles.x,
                                                                                                   IsToggleGroupShown(_rangeRotGroupY.faded) ? Random.Range(_rangeRotYMin, _rangeRotYMax) : rotation.eulerAngles.y,
                                                                                                   IsToggleGroupShown(_rangeRotGroupZ.faded) ? Random.Range(_rangeRotZMin, _rangeRotZMax) : rotation.eulerAngles.z);

        protected override void SetupOnEnable()
        {
            _randomRotGroup = new AnimBool(false);
            _randomRotGroup.valueChanged.AddListener(repaint);
            _rangeRotGroupX = new AnimBool(false);
            _rangeRotGroupX.valueChanged.AddListener(repaint);
            _rangeRotGroupY = new AnimBool(false);
            _rangeRotGroupY.valueChanged.AddListener(repaint);
            _rangeRotGroupZ = new AnimBool(false);
            _rangeRotGroupZ.valueChanged.AddListener(repaint);
        }
    }
}