using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace KamranWali.SimpleInterface.Editor.Layouts
{
    public class OffsetPositionLayout : BaseLayout
    {
        private AnimBool _mainGroup;
        private AnimBool _posGroupX;
        private AnimBool _posGroupY;
        private AnimBool _posGroupZ;
        private float _posX;
        private float _posY;
        private float _posZ;
        private Vector3 _prePos;
        private bool _isStart; // Flag to check if the layout has been just enabled

        public OffsetPositionLayout(UnityAction repaint) : base(repaint)
        {
        }

        public override void Hide() { if (IsShown()) _mainGroup.target = false; }
        public override bool IsShown() => _mainGroup.target;

        public override void SetupOnGUI()
        {
            _mainGroup.target = ToggleLeft("Offset Position (V)", "Toggle to place prefab in given offset position. Hotkey = 'V'", _mainGroup);

            if (BeginFadeGroup(_mainGroup.faded))
            {
                BeginHorizontalLayout(ref _posGroupX, ref _posX, "X", "Toggle to offset X axis.", 20f, 1f);
                BeginHorizontalLayout(ref _posGroupY, ref _posY, "Y", "Toggle to offset Y axis.", 20f, 1f);
                BeginHorizontalLayout(ref _posGroupZ, ref _posZ, "Z", "Toggle to offset Z axis.", 20f, 1f);
            }
            EndFadeGroup();
            HideOtherLayouts(); // Hidding other layouts
        }

        public override void Update(Event currentEvent)
        {
            if (!IsShown() && !_isStart) _isStart = true; // Condition to reset flag

            if (currentEvent.keyCode == KeyCode.V && currentEvent.type == EventType.KeyDown)
            {
                _mainGroup.target = !_mainGroup.target;
                HideOtherLayouts(); // Hidding other layouts
            }
        }

        public override Vector3 GetPosition(Vector3 position)
        {
            if (!_isStart) // Condition to check offset
            {
                if((_posGroupX.target ? Mathf.Abs(position.x - _prePos.x) >= _posX : true) &&
                   (_posGroupY.target ? Mathf.Abs(position.y - _prePos.y) >= _posY : true) &&
                   (_posGroupZ.target ? Mathf.Abs(position.z - _prePos.z) >= _posZ : true)) // Validating the current position
                    _prePos = position; // Sending the offset position
            }
            else // Condition for sending the start up position
            {
                _isStart = false;
                _prePos = position; // Sending start up position
            }
            return _prePos;
        }

        protected override void SetupOnEnable()
        {
            _mainGroup = new AnimBool(false);
            _mainGroup.valueChanged.AddListener(repaint);
            _posGroupX = new AnimBool(false);
            _posGroupX.valueChanged.AddListener(repaint);
            _posGroupY = new AnimBool(false);
            _posGroupY.valueChanged.AddListener(repaint);
            _posGroupZ = new AnimBool(false);
            _posGroupZ.valueChanged.AddListener(repaint);
        }
    }
}