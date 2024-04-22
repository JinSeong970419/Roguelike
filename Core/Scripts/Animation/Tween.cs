using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Roguelike.Core
{
    [RequireComponent(typeof(Image))]
    public class Tween : MonoBehaviour
    {
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float duration;
        [SerializeField] private bool loop;

        private float tick = 0f;
        private bool tweenScale2DFlag = false;


        public void StartTweenScale2D()
        {
            //StartCoroutine(CoTweenScale2D());
            tweenScale2DFlag = true;
        }

        public void EndTweenScale2D()
        {
            tick = 0f;
            tweenScale2DFlag = false;
            transform.localScale = Vector3.one;
        }

        private void Update()
        {
            if(tweenScale2DFlag)
            {
                tick += Time.deltaTime;
                if (tick <= duration)
                {
                    float ratio = tick / duration;
                    float value = curve.Evaluate(ratio);
                    transform.localScale = new Vector3(value, value, 1f);
                }
                else
                {
                    if(loop)
                    {
                        tick = 0f;
                    }
                }
            }
            
        }

        IEnumerator CoTweenScale2D()
        {
            
            while (true)
            {
                tick += Time.deltaTime;
                if (tick <= duration)
                {
                    float ratio = tick / duration;
                    float value = curve.Evaluate(ratio);
                    transform.localScale = new Vector3(value, value, 1f);
                }
                else
                {
                    tick = 0f;
                    break;
                }


                yield return null;
            }
            
        }
    }
}
