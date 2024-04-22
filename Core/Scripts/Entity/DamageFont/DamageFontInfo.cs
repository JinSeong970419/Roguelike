using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "DamageFontInfo", menuName = "TheSalt/Data/DamageFontInfo")]
    public class DamageFontInfo : ScriptableObject
    {
        [SerializeField] private string _codeName;
        [SerializeField] private string _name;
        [SerializeField] private Sprite[] _fonts = new Sprite[10];
        [SerializeField] private float _duration = 1f;

        public string CodeName { get { return _codeName; } }
        public string Name { get { return _name; } }
        public Sprite[] Fonts { get { return _fonts; } }
        public float Duration { get { return _duration; } }

        private void OnValidate()
        {
            if (_fonts == null)
            {
                _fonts = new Sprite[10];
            }
            if (_fonts.Length > 10)
            {
                Sprite[] newFonts = new Sprite[10];
                for (int i = 0; i < 10; i++)
                {
                    newFonts[i] = _fonts[i];
                }
                _fonts = newFonts;
            }
            else if (_fonts.Length < 10)
            {
                int len = _fonts.Length;
                Sprite[] newFonts = new Sprite[10];
                for (int i = 0; i < len; i++)
                {
                    newFonts[i] = _fonts[i];
                }
                _fonts = newFonts;
            }
        }


    }
}
