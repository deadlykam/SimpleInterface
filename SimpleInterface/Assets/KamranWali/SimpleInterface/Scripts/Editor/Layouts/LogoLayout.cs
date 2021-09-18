using UnityEngine;
using UnityEngine.Events;

namespace KamranWali.SimpleInterface.Editor.Layouts
{
    public class LogoLayout : BaseLayout
    {
        private readonly Texture _texLogo;
        private readonly Texture _texLogoName;
        private readonly string _logoPath = "KamranWali/SimpleInterface/Images/Logo";
        private readonly string _logoNamePath = "KamranWali/SimpleInterface/Images/LogoName";
        private readonly GUIStyle _versionStyle;
        private readonly int _fontSize = 18;

        public LogoLayout(UnityAction repaint) : base(repaint) 
        { 
            _texLogo = Resources.Load<Texture>(_logoPath);
            _texLogoName = Resources.Load<Texture>(_logoNamePath);
            _versionStyle = new GUIStyle();
            _versionStyle.fontSize = _fontSize;
            _versionStyle.normal.textColor = Color.white;
        }

        public override void Hide() { }
        public override bool IsShown() => true;
        public override void SetupOnGUI() 
        {
            if (_texLogo != null)
            {
                Space(30f);
                Box(_texLogo, 100f, 100f, true, false);
                Box(_texLogoName, 300f, 34f, true, false);

                Space(10f);
                BeginHorizontal();
                Space(5f);
                LabelField("Version - v1.0.1", _versionStyle);
                EndHorizontal();

            }
        }
        public override void Update(Event currentEvent) { }

        protected override void SetupOnEnable() { }
    }
}