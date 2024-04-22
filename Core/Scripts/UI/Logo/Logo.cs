using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

namespace Roguelike.Core
{
    [Serializable]
    public class LogoData
    {
        public enum DisplayType
        {
            Image,
            Movie,
        }

        public enum TargetPlatform
        {
            All,
            Android_Only,
            iOS_Only,
            Unknown,
        }

        public DisplayType displayType = DisplayType.Image;
        public TargetPlatform targetPlatform = TargetPlatform.All;

        public bool ignoreGlobal = false;
        public bool m_isDisplay = true;

        [Header("TYPE IMAGE")]
        public Image image = null;
        public float time = 0.0f;
    }
    public class Logo : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup = null;
        [SerializeField] private LogoData[] _logos = null;
        [Space(10)]
        [SerializeField] private float _fadeTime = 0.5f;
        [SerializeField] private bool _fadeInverse = false;
        [Space(10)]
        [SerializeField] private string _nextSceneName = null;

        private void Awake()
        {
            _canvasGroup.alpha = (_fadeInverse) ? 1.0f : 0.0f;
            StartCoroutine(Logo_Internal());
        }

        private IEnumerator Logo_Internal()
        {
            yield return new WaitForEndOfFrame();

            for (int nInd = 0; nInd < _logos.Length; ++nInd)
            {
                if (_logos[nInd].m_isDisplay == false)
                    continue;

                bool bShow = _logos[nInd].targetPlatform != LogoData.TargetPlatform.Unknown;
                if (bShow)
                {
#if UNITY_ANDROID
                    bShow = _logos[nInd].targetPlatform == LogoData.TargetPlatform.All ||
                            _logos[nInd].targetPlatform == LogoData.TargetPlatform.Android_Only;
#elif UNITY_IOS
				bShow = _logos[nInd].targetPlatform == LogoData.eTargetPlatform.All ||
						_logos[nInd].targetPlatform == LogoData.eTargetPlatform.iOS_Only;
#endif
                }

                if (bShow == false)
                    continue;

                for (int nLogo = 0; nLogo < _logos.Length; ++nLogo)
                {
                    if (_logos[nLogo].displayType == LogoData.DisplayType.Image)
                    {
                        Utils.SafeActive(_logos[nLogo].image, (nLogo == nInd) ? true : false);
                    }

                    if (_logos[nLogo].displayType == LogoData.DisplayType.Movie)
                    {
                        //	Utils.SafeActive(_logos[nLogo].mediaPlayerCtrl, (nLogo == nInd) ? true : false);
                    }
                }


                switch (_logos[nInd].displayType)
                {
                    case LogoData.DisplayType.Image:
                        {
                            yield return StartCoroutine(Fade_Internal((_fadeInverse) ? false : true));
                            yield return new WaitForSeconds(_logos[nInd].time);
                            yield return StartCoroutine(Fade_Internal((_fadeInverse) ? true : false));
                        }
                        break;

                    case LogoData.DisplayType.Movie:
                        break;
                }
            }

            UnityEngine.SceneManagement.SceneManager.LoadScene(_nextSceneName);
        }

        private IEnumerator Fade_Internal(bool bShow)
        {
            if (bShow)
            {
                while (_canvasGroup.alpha < 1.0f)
                {
                    _canvasGroup.alpha += Time.deltaTime / _fadeTime;
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                while (_canvasGroup.alpha > 0.0f)
                {
                    _canvasGroup.alpha -= Time.deltaTime / _fadeTime;
                    yield return new WaitForEndOfFrame();
                }
            }
        }

    }
}
