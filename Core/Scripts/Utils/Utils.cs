using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace Roguelike.Core
{
    public class Utils
    {
        private interface IInvokator
        {
            void Invoke();
        }

        private class InvokeElement_Param0 : IInvokator
        {
            Action _callback = null;

            public InvokeElement_Param0(Action pAction)
            {
                _callback = pAction;
            }

            public void Invoke()
            {
                SafeInvoke(_callback);
            }
        }

        private class InvokeElement_Param1<T1> : IInvokator
        {
            Action<T1> _callback = null;
            T1 _param1 = default(T1);

            public InvokeElement_Param1(Action<T1> pAction, T1 pParam1)
            {
                _callback = pAction;
                _param1 = pParam1;
            }

            public void Invoke()
            {
                SafeInvoke(_callback, _param1);
            }
        }

        private class InvokeElement_Param2<T1, T2> : IInvokator
        {
            Action<T1, T2> _callback = null;
            T1 _param1 = default(T1);
            T2 _param2 = default(T2);

            public InvokeElement_Param2(Action<T1, T2> pAction, T1 pParam1, T2 pParam2)
            {
                _callback = pAction;
                _param1 = pParam1;
                _param2 = pParam2;
            }

            public void Invoke()
            {
                SafeInvoke(_callback, _param1, _param2);
            }
        }

        private class InvokeElement_Param3<T1, T2, T3> : IInvokator
        {
            Action<T1, T2, T3> _callback = null;
            T1 _param1 = default(T1);
            T2 _param2 = default(T2);
            T3 _param3 = default(T3);

            public InvokeElement_Param3(Action<T1, T2, T3> pAction, T1 pParam1, T2 pParam2, T3 pParam3)
            {
                _callback = pAction;
                _param1 = pParam1;
                _param2 = pParam2;
                _param3 = pParam3;
            }

            public void Invoke()
            {
                SafeInvoke(_callback, _param1, _param2, _param3);
            }
        }

        private class InvokeElement_Param4<T1, T2, T3, T4> : IInvokator
        {
            Action<T1, T2, T3, T4> _callback = null;
            T1 _param1 = default(T1);
            T2 _param2 = default(T2);
            T3 _param3 = default(T3);
            T4 _param4 = default(T4);

            public InvokeElement_Param4(Action<T1, T2, T3, T4> pAction, T1 pParam1, T2 pParam2, T3 pParam3, T4 pParam4)
            {
                _callback = pAction;
                _param1 = pParam1;
                _param2 = pParam2;
                _param3 = pParam3;
                _param4 = pParam4;
            }

            public void Invoke()
            {
                SafeInvoke(_callback, _param1, _param2, _param3, _param4);
            }
        }

        private class InvokeElement_Param5<T1, T2, T3, T4, T5> : IInvokator
        {
            Action<T1, T2, T3, T4, T5> _callback = null;
            T1 _param1 = default(T1);
            T2 _param2 = default(T2);
            T3 _param3 = default(T3);
            T4 _param4 = default(T4);
            T5 _param5 = default(T5);

            public InvokeElement_Param5(Action<T1, T2, T3, T4, T5> pAction, T1 pParam1, T2 pParam2, T3 pParam3, T4 pParam4, T5 pParam5)
            {
                _callback = pAction;
                _param1 = pParam1;
                _param2 = pParam2;
                _param3 = pParam3;
                _param4 = pParam4;
                _param5 = pParam5;
            }

            public void Invoke()
            {
                SafeInvoke(_callback, _param1, _param2, _param3, _param4, _param5);
            }
        }

        private class InvokeElement_Param6<T1, T2, T3, T4, T5, T6> : IInvokator
        {
            Action<T1, T2, T3, T4, T5, T6> _callback = null;
            T1 _param1 = default(T1);
            T2 _param2 = default(T2);
            T3 _param3 = default(T3);
            T4 _param4 = default(T4);
            T5 _param5 = default(T5);
            T6 _param6 = default(T6);

            public InvokeElement_Param6(Action<T1, T2, T3, T4, T5, T6> pAction, T1 pParam1, T2 pParam2, T3 pParam3, T4 pParam4, T5 pParam5, T6 pParam6)
            {
                _callback = pAction;
                _param1 = pParam1;
                _param2 = pParam2;
                _param3 = pParam3;
                _param4 = pParam4;
                _param5 = pParam5;
                _param6 = pParam6;
            }

            public void Invoke()
            {
                SafeInvoke(_callback, _param1, _param2, _param3, _param4, _param5, _param6);
            }
        }


        private static ConcurrentBag<IInvokator> _invokeElements = new ConcurrentBag<IInvokator>();
        static public void InvokeAll()
        {
            foreach (var element in _invokeElements)
            {
                element.Invoke();
            }
            _invokeElements.Clear();
        }

        /// <summary>
        /// Callback�� �����ϰ� Invoke�մϴ�.
        /// Invoke ȣ�� ��ġ�� Main Thread�� �ƴ� ��� ����ȣ��˴ϴ�.
        /// </summary>
        static public bool SafeInvoke(Action pCallback)
        {
            if (pCallback == null) return false;

            if (Framework.IsMainThread == false)
            {
                _invokeElements.Add(new InvokeElement_Param0(pCallback));
                return true;
            }

            pCallback();

            return true;
        }

        /// <summary>
        /// Callback�� �����ϰ� Invoke�մϴ�.
        /// Invoke ȣ�� ��ġ�� Main Thread�� �ƴ� ��� ����ȣ��˴ϴ�.
        /// </summary>
        static public bool SafeInvoke<T>(Action<T> pCallback, T tParam)
        {
            if (pCallback == null) return false;

            if (Framework.IsMainThread == false)
            {
                _invokeElements.Add(new InvokeElement_Param1<T>(pCallback, tParam));
                return true;
            }

            pCallback(tParam);

            return true;
        }

        /// <summary>
        /// Callback�� �����ϰ� Invoke�մϴ�.
        /// Invoke ȣ�� ��ġ�� Main Thread�� �ƴ� ��� ����ȣ��˴ϴ�.
        /// </summary>
        static public bool SafeInvoke<T1, T2>(Action<T1, T2> pCallback, T1 tParam1, T2 tParam2)
        {
            if (pCallback == null) return false;

            if (Framework.IsMainThread == false)
            {
                _invokeElements.Add(new InvokeElement_Param2<T1, T2>(pCallback, tParam1, tParam2));
                return true;
            }

            pCallback(tParam1, tParam2);

            return true;
        }

        /// <summary>
        /// Callback�� �����ϰ� Invoke�մϴ�.
        /// Invoke ȣ�� ��ġ�� Main Thread�� �ƴ� ��� ����ȣ��˴ϴ�.
        /// </summary>
        static public bool SafeInvoke<T1, T2, T3>(Action<T1, T2, T3> pCallback, T1 tParam1, T2 tParam2, T3 tParam3)
        {
            if (pCallback == null) return false;

            if (Framework.IsMainThread == false)
            {
                _invokeElements.Add(new InvokeElement_Param3<T1, T2, T3>(pCallback, tParam1, tParam2, tParam3));
                return true;
            }

            pCallback(tParam1, tParam2, tParam3);

            return true;
        }

        /// <summary>
        /// Callback�� �����ϰ� Invoke�մϴ�.
        /// Invoke ȣ�� ��ġ�� Main Thread�� �ƴ� ��� ����ȣ��˴ϴ�.
        /// </summary>
        static public bool SafeInvoke<T1, T2, T3, T4>(Action<T1, T2, T3, T4> pCallback, T1 tParam1, T2 tParam2, T3 tParam3, T4 tParam4)
        {
            if (pCallback == null) return false;

            if (Framework.IsMainThread == false)
            {
                _invokeElements.Add(new InvokeElement_Param4<T1, T2, T3, T4>(pCallback, tParam1, tParam2, tParam3, tParam4));
                return true;
            }

            pCallback(tParam1, tParam2, tParam3, tParam4);

            return true;
        }

        /// <summary>
        /// Callback�� �����ϰ� Invoke�մϴ�.
        /// Invoke ȣ�� ��ġ�� Main Thread�� �ƴ� ��� ����ȣ��˴ϴ�.
        /// </summary>
        static public bool SafeInvoke<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> pCallback, T1 tParam1, T2 tParam2, T3 tParam3, T4 tParam4, T5 tParam5)
        {
            if (pCallback == null) return false;

            if (Framework.IsMainThread == false)
            {
                _invokeElements.Add(new InvokeElement_Param5<T1, T2, T3, T4, T5>(pCallback, tParam1, tParam2, tParam3, tParam4, tParam5));
                return true;
            }

            pCallback(tParam1, tParam2, tParam3, tParam4, tParam5);

            return true;
        }

        /// <summary>
        /// Callback�� �����ϰ� Invoke�մϴ�.
        /// Invoke ȣ�� ��ġ�� Main Thread�� �ƴ� ��� ����ȣ��˴ϴ�.
        /// </summary>
        static public bool SafeInvoke<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> pCallback, T1 tParam1, T2 tParam2, T3 tParam3, T4 tParam4, T5 tParam5, T6 tParam6)
        {
            if (pCallback == null) return false;

            if (Framework.IsMainThread == false)
            {
                _invokeElements.Add(new InvokeElement_Param6<T1, T2, T3, T4, T5, T6>(pCallback, tParam1, tParam2, tParam3, tParam4, tParam5, tParam6));
                return true;
            }

            pCallback(tParam1, tParam2, tParam3, tParam4, tParam5, tParam6);

            return true;
        }

        /// <summary>
        /// CallBack�� AddFunc�� ��� �� �� AddFunc�� ȣ���մϴ�.
        /// CallBack�� ��ϰ� ȣ���� ���ÿ� �Ǿ��ϴ� Initialize ������ �����մϴ�.
        /// </summary>
        /// <param name="pCallback">AddFunc�� �߰� �� ��� Callback</param>
        /// <param name="pAddFunc">�߰� �� �� ȣ�� �� AddFunc</param>
        static public void SafeInvokeAndAdd(ref Action pCallback, Action pAddFunc)
        {
            if (pAddFunc == null)
                return;

            pCallback += pAddFunc;
            pAddFunc();
        }

        /// <summary>
        /// CallBack�� AddFunc�� ��� �� �� AddFunc�� ȣ���մϴ�.
        /// CallBack�� ��ϰ� ȣ���� ���ÿ� �Ǿ��ϴ� Initialize ������ �����մϴ�.
        /// </summary>
        /// <param name="pCallback">AddFunc�� �߰� �� ��� Callback</param>
        /// <param name="pAddFunc">�߰� �� �� ȣ�� �� AddFunc</param>
        static public void SafeInvokeAndAdd<T>(ref Action<T> pCallback, Action<T> pAddFunc, T tParam)
        {
            if (pAddFunc == null)
                return;

            pCallback += pAddFunc;
            pAddFunc(tParam);
        }

        /// <summary>
        /// CallBack�� AddFunc�� ��� �� �� AddFunc�� ȣ���մϴ�.
        /// CallBack�� ��ϰ� ȣ���� ���ÿ� �Ǿ��ϴ� Initialize ������ �����մϴ�.
        /// </summary>
        /// <param name="pCallback">AddFunc�� �߰� �� ��� Callback</param>
        /// <param name="pAddFunc">�߰� �� �� ȣ�� �� AddFunc</param>
        static public void SafeInvokeAndAdd<T1, T2>(ref Action<T1, T2> pCallback, Action<T1, T2> pAddFunc, T1 tParam1, T2 tParam2)
        {
            if (pAddFunc == null)
                return;

            pCallback += pAddFunc;
            pAddFunc(tParam1, tParam2);
        }

        /// <summary>
        /// CallBack�� AddFunc�� ��� �� �� AddFunc�� ȣ���մϴ�.
        /// CallBack�� ��ϰ� ȣ���� ���ÿ� �Ǿ��ϴ� Initialize ������ �����մϴ�.
        /// </summary>
        /// <param name="pCallback">AddFunc�� �߰� �� ��� Callback</param>
        /// <param name="pAddFunc">�߰� �� �� ȣ�� �� AddFunc</param>
        static public void SafeInvokeAndAdd<T1, T2, T3>(ref Action<T1, T2, T3> pCallback, Action<T1, T2, T3> pAddFunc, T1 tParam1, T2 tParam2, T3 tParam3)
        {
            if (pAddFunc == null)
                return;

            pCallback += pAddFunc;
            pAddFunc(tParam1, tParam2, tParam3);
        }

        /// <summary>
		/// GameObject�� Active State�� �����ϰ� �����մϴ�.
		/// </summary>
		static public void SafeActive(GameObject pObject, bool bActive)
        {
            if (pObject != null && pObject.activeSelf != bActive)
                pObject.SetActive(bActive);
        }

        /// <summary>
        /// GameObject �迭�� Active State�� �����ϰ� �����մϴ�.
        /// </summary>
        static public void SafeActive(GameObject[] vObject, bool bActive)
        {
            for (int nInd = 0; nInd < vObject.Length; ++nInd)
                SafeActive(vObject[nInd], bActive);
        }

        /// <summary>
        /// GameObject �迭�� Active State�� �����ϰ� �����մϴ�.
        /// </summary>
        static public void SafeActive(List<GameObject> vObject, bool bActive)
        {
            for (int nInd = 0; nInd < vObject.Count; ++nInd)
                SafeActive(vObject[nInd], bActive);
        }

        /// <summary>
        /// MonoBehaviour�� ��ϵǾ��ִ� GameObject�� Active State�� �����ϰ� �����մϴ�.
        /// </summary>
        static public void SafeActive(Component pObject, bool bActive)
        {
            if (pObject != null && pObject.gameObject.activeSelf != bActive)
                pObject.gameObject.SetActive(bActive);
        }

        /// <summary>
        /// MonoBehaviour�� ��ϵǾ��ִ� GameObject �迭�� Active State�� �����ϰ� �����մϴ�.
        /// </summary>
        static public void SafeActive(Component[] vObject, bool bActive)
        {
            if (vObject != null)
            {
                for (int nInd = 0; nInd < vObject.Length; ++nInd)
                    SafeActive(vObject[nInd], bActive);
            }
        }

        /// <summary>
        /// MonoBehaviour�� ��ϵǾ��ִ� GameObject �迭�� Active State�� �����ϰ� �����մϴ�.
        /// </summary>
        static public void SafeActive<T>(List<T> vObject, bool bActive) where T : Component
        {
            if (vObject != null)
            {
                for (int nInd = 0; nInd < vObject.Count; ++nInd)
                    SafeActive(vObject[nInd], bActive);
            }
        }

        /// <summary>
        /// MonoBehaviour�� Enable State�� �����ϰ� �����մϴ�.
        /// </summary>
        static public void SafeEnable(Behaviour pObject, bool bActive)
        {
            if (pObject != null && pObject.enabled != bActive)
                pObject.enabled = bActive;
        }

        /// <summary>
        /// MonoBehaviour�� Enable State�� �����ϰ� �����մϴ�.
        /// </summary>
        static public void SafeEnable(Behaviour[] vObject, bool bActive)
        {
            for (int nInd = 0; nInd < vObject.Length; ++nInd)
                SafeEnable(vObject[nInd], bActive);
        }

        /// <summary>
        /// GameObject�� �����ϰ� �ı��մϴ�.
        /// Object Pool�� �ͼ� �� GameObject�� Object Pool�� ȯ���˴ϴ�.
        /// </summary>
        static public bool SafeDestroy(GameObject pObject)
        {
            if (pObject == null)
                return false;
            // TODO : 2021 ���� ObjectPool ���� ������ �°� ����
            //if (ObjectPool.ReturnToPool(pObject))
            //    return true;
            GameObject.Destroy(pObject);
            return true;
        }

        /// <summary>
        /// GameObject�� �����ϰ� �ı��մϴ�.
        /// Object Pool�� �ͼ� �� GameObject�� Object Pool�� ȯ���˴ϴ�.
        /// </summary>
        static public bool SafeDestroy(Component pObject)
        {
            if (pObject == null)
                return false;

            return SafeDestroy(pObject.gameObject);
        }

        /// <summary>
        /// GameObject�� Ư�� �ð� ���� �Ŀ� �����ϰ� �ı��մϴ�.
        /// Object Pool�� �ͼ� �� GameObject�� Object Pool�� ȯ���˴ϴ�.
        /// </summary>
        /// <param name="fDelay">�ı� ���� �ð�</param>
        static public bool SafeDestroy(GameObject pObject, float fDelay)
        {
            return SafeDestroy(pObject, fDelay, false);
        }

        /// <summary>
        /// GameObject�� Ư�� �ð� ���� �Ŀ� �����ϰ� �ı��մϴ�.
        /// Object Pool�� �ͼ� �� GameObject�� Object Pool�� ȯ���˴ϴ�.
        /// </summary>
        /// <param name="fDelay">�ı� ���� �ð�</param>
        static public bool SafeDestroy(Component pObject, float fDelay)
        {
            if (pObject == null)
                return false;

            return SafeDestroy(pObject.gameObject, fDelay, false);
        }

        /// <summary>
        /// GameObject�� Ư�� �ð� ���� �Ŀ� �����ϰ� �ı��մϴ�.
        /// Object Pool�� �ͼ� �� GameObject�� Object Pool�� ȯ���˴ϴ�.
        /// </summary>
        /// <param name="fDelay">�ı� ���� �ð�</param>
        /// <param name="bRealTime">���� �ð��� Time Scale ���� �����Դϴ�</param>
        static public bool SafeDestroy(GameObject pObject, float fDelay, bool bRealTime)
        {
            if (pObject == null)
                return false;

            // TODO : 2021 ���� ObjectPool ���� ������ �°� ����
            //if (ObjectPool.IsPoolObject(pObject))
            //{
            //    StopWatch.CreateNewStopWatch(fDelay, bRealTime, () => { ObjectPool.ReturnToPool(pObject); });
            //    return true;
            //}

            GameObject.Destroy(pObject, fDelay);
            return true;
        }

        /// <summary>
        /// GameObject�� Ư�� �ð� ���� �Ŀ� �����ϰ� �ı��մϴ�.
        /// Object Pool�� �ͼ� �� GameObject�� Object Pool�� ȯ���˴ϴ�.
        /// </summary>
        /// <param name="fDelay">�ı� ���� �ð�</param>
        /// <param name="bRealTime">���� �ð��� Time Scale ���� �����Դϴ�</param>
        static public bool SafeDestroy(Component pObject, float fDelay, bool bRealTime)
        {
            if (pObject == null)
                return false;

            return SafeDestroy(pObject.gameObject, fDelay, bRealTime);
        }


        /////////////////////////////////////////
        /// <summary>
        /// Coroutine�� �����ϰ� ������ŵ�ϴ�.
        /// </summary>
        /// <param name="pParent">Coroutine�� �ͼ� �� MonoBehaviour</param>
        /// <param name="pCorutine">��� Coroutine</param>
        static public void SafeStopCorutine(MonoBehaviour pParent, ref Coroutine pCorutine)
        {
            do
            {
                if (pParent == null || pCorutine == null)
                    break;

                pParent.StopCoroutine(pCorutine);
            }
            while (false);

            pCorutine = null;
        }

        public static string ArrayToString<T>(T[] array, string separator)
        {
            string str = "";
            foreach (T s in array)
            {
                str += (s == null ? "null" : s.ToString()) + separator;
            }

            if (str.Length > 0)
            {
                str = str.Substring(0, str.Length - separator.Length);
            }

            return str;
        }



        /////////////////////////////////////////
        /// <summary>
        /// �ش� GameObject�� Transform�� ���� ��� Transform�� ����ϴ�.
        /// </summary>
        /// <param name="pTransform">���� ��� Transform</param>
        /// <param name="bInheritPos">��� Transform�� ��ġ�� �����մϴ�</param>
        /// <param name="bInheritRot">��� Transform�� ȸ���� �����մϴ�</param>
        /// <param name="bInheritScale">��� Transform�� ũ�⸦ �����մϴ�</param>
        /// <param name="bInheritParent">��� Transform�� Hierarchy ������ ��ġ��ŵ�ϴ�</param>
        static public void InheritTransform(GameObject pObject, Transform pTransform, bool bInheritPos = false, bool bInheritRot = false, bool bInheritScale = false, bool bInheritParent = false)
        {
            if (pObject == null || pTransform == null)
                return;

            if (bInheritPos)
                pObject.transform.position = pTransform.position;

            if (bInheritRot)
                pObject.transform.rotation = pTransform.rotation;

            if (bInheritParent)
                pObject.transform.SetParent(pTransform);

            if (bInheritScale)
                pObject.transform.localScale = pTransform.localScale;
        }

    }
}
