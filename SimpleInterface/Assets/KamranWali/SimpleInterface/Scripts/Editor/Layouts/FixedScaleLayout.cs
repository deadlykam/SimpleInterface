using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace KamranWali.SimpleInterface.Editor.Layouts
{
    public class FixedScaleLayout : BaseLayout
    {
        private float _fixedScale;
        private AnimBool _fixedScaleGroup;

        public FixedScaleLayout(UnityAction repaint) : base(repaint)
        {
        }

        public override void Hide() { if (IsShown()) _fixedScaleGroup.target = false; }
        public override bool IsShown() => _fixedScaleGroup.target;

        public override void SetupOnGUI()
        {
            _fixedScaleGroup.target = ToggleLeft("Fixed Scale", "Toggle to place prefab in given fixed Scale. Hotkey = 'K'", _fixedScaleGroup);

            if (BeginFadeGroup(_fixedScaleGroup.faded)) BeginHorizontalLayout(ref _fixedScale, "Scale", "The fixed scale for the prefab.", 25f, 40f);
            EndFadeGroup();
            HideOtherLayouts();
        }

        public override void Update(Event currentEvent)
        {
            if (currentEvent.keyCode == KeyCode.K && currentEvent.type == EventType.KeyDown)
            {
                _fixedScaleGroup.target = !_fixedScaleGroup.target;
                HideOtherLayouts(); // Hidding other layouts
            }
        }

        public override Vector3 GetScale(Vector3 scale) => scale * _fixedScale;

        protected override void SetupOnEnable(UnityAction repaint)
        {
            _fixedScaleGroup = new AnimBool(false);
            _fixedScaleGroup.valueChanged.AddListener(repaint);
        }
    }
}