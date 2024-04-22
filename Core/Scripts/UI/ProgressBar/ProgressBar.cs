using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Roguelike.Core
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _progress;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private GameEvent<float> _progressChangedEvent;

        public float Value
        {
            get { return _progress.fillAmount; }
            set
            {
                float clamp = Mathf.Clamp(value, 0f, 1f);
                _progress.fillAmount = clamp;
                if (_text != null)
                {
                    _text.text = ((int)(clamp * 100f)).ToString() + "%";

                }
            }
        }

        private void OnEnable()
        {
            _progressChangedEvent?.AddListener(OnProgressChanged);
        }

        private void OnDisable()
        {
            _progressChangedEvent?.RemoveListener(OnProgressChanged);
        }

        private void OnProgressChanged(float progress)
        {
            Value = progress;
        }
    }
}
