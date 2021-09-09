using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace KamranWali.SimpleInterface.Editor.Layouts
{
    public class RandomScaleLayout : BaseLayout
    {
        private float _randomScaleMin;
        private float _randomScaleMax;
        private AnimBool _randomScaleGroup;
        private Vector3 _vectorOne;

        public RandomScaleLayout(UnityAction repaint) : base(repaint) => _vectorOne = new Vector3(1f, 1f, 1f);

        public override void Hide() { if (IsShown()) _randomScaleGroup.target = false; }
        public override bool IsShown() => _randomScaleGroup.target;

        public override void SetupOnGUI()
        {
            _randomScaleGroup.target = ToggleLeft("Random Scale (L)", "Toggle to place prefab in random Scale. Hotkey = 'L'", _randomScaleGroup);
            if (BeginFadeGroup(_randomScaleGroup.faded)) BeginHorizontalLayout(ref _randomScaleMin, ref _randomScaleMax, "Min", "Max", "Minimum Scale", "Maximum Scale", 25f, 40f);
            EndFadeGroup();
            HideOtherLayouts();
        }

        public override void Update(Event currentEvent)
        {
            if (currentEvent.keyCode == KeyCode.L && currentEvent.type == EventType.KeyDown)
            {
                _randomScaleGroup.target = !_randomScaleGroup.target;
                HideOtherLayouts(); // Hidding other layouts
            }
        }

        public override Vector3 GetScale(Vector3 scale) => _vectorOne * Random.Range(_randomScaleMin, _randomScaleMax);

        protected override void SetupOnEnable()
        {
            _randomScaleGroup = new AnimBool(false);
            _randomScaleGroup.valueChanged.AddListener(repaint);
        }
    }
}