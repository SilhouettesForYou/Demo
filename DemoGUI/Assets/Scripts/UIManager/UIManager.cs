using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;
        public static UIManager GetInstance()
        {
            if (instance == null)
            {
                instance = new UIManager();
            }
            return instance;
        }

        Dictionary<string, string> UIPaths = new Dictionary<string, string>();
        Dictionary<string, BaseUI> cacheUIs = new Dictionary<string, BaseUI>();
        Dictionary<string, BaseUI> showUIs = new Dictionary<string, BaseUI>();

        Transform transformRoot;
        Transform transformNormal;
        Transform transformFixed;
        Transform transformPopUp;
        Transform transformUIScripts;

        Stack<BaseUI> staUIs = new Stack<BaseUI>();

        void Awake()
        {
            instance = this;
            InitRootCavansLoading();
            transformRoot = GameObject.FindGameObjectWithTag(SystemDefine.canvasTag).transform;
            transformNormal = transformRoot.Find("Normal");
            transformFixed = transformRoot.Find("Fixed");
            transformPopUp = transformRoot.Find("PopUp");
            transformUIScripts = transformRoot.Find("ScriptsMgr");

            this.gameObject.transform.SetParent(transformUIScripts, false);
            DontDestroyOnLoad(transformRoot);
            UIPaths?.Add("BeginGameUI", "UIPrefabs/BeginGameUI");
            UIPaths?.Add("HomeUI", "UIPrefabs/HomeUI");
        }

        void OnDestroy()
        {
            ClearStack();
        }

        public void ShowUI(string UIName)
        {
            BaseUI baseUI;
            if (string.IsNullOrEmpty(UIName))
                return;

            baseUI = LoadUIToCacheUIs(UIName);
            if (baseUI == null)
                return;

            switch (baseUI.curUIType.mode)
            {
                case UIFormShowMode.Normal:
                    Debug.Log("Show mode - normal");
                    LoadUIToShowUIs(UIName);
                    break;
                case UIFormShowMode.ReverseChange:
                    PushUIToStack(UIName);
                    break;
                case UIFormShowMode.HideOther:
                    EnterUIAndHideOther(UIName);
                    break;
            }

        }

        public void CloseUI(string UIName)
        {
            BaseUI baseUI;
            //参数检查
            if (string.IsNullOrEmpty(UIName))
                return;
            //所有UI窗体 没有记录直接返回
            cacheUIs.TryGetValue(UIName, out baseUI);
            if (baseUI == null)
                return;

            switch (baseUI.curUIType.mode)
            {
                case UIFormShowMode.Normal:
                    ExitUI(UIName);
                    break;
                case UIFormShowMode.ReverseChange:
                    PopUIToStack();
                    break;
                case UIFormShowMode.HideOther:
                    ExitUIAndDisPlayOther(UIName);
                    break;
            }

        }

        private void InitRootCavansLoading()
        {
            ResourcesMgr.GetInstance().LoadAsset(SystemDefine.canvasPath, false);
        }

        private BaseUI LoadUIToCacheUIs(string UIName)
        {
            BaseUI baseUI;
            cacheUIs.TryGetValue(UIName, out baseUI);
            if (baseUI == null)
            {
                baseUI = LoadUI(UIName);
            }

            return baseUI;
        }

        private BaseUI LoadUI(string UIName)
        {
            string uiPath;
            GameObject UIPrefabs = null;
            BaseUI baseUI = null;

            UIPaths.TryGetValue(UIName, out uiPath);
            if (!string.IsNullOrEmpty(uiPath))
            {
                UIPrefabs = ResourcesMgr.GetInstance().LoadAsset(uiPath, false);
            }

            if (transformRoot != null && UIPrefabs != null)
            {
                baseUI = UIPrefabs.GetComponent<BaseUI>();
                if (baseUI == null)
                {
                    return null;
                }

                switch (baseUI.curUIType.type)
                {
                    case UIFormType.Normal:
                        UIPrefabs.transform.SetParent(transformNormal, false);
                        break;
                    case UIFormType.Fixed:
                        UIPrefabs.transform.SetParent(transformFixed, false);
                        break;
                    case UIFormType.PopUp:
                        UIPrefabs.transform.SetParent(transformPopUp, false);
                        break;
                }

                UIPrefabs.SetActive(true);
                cacheUIs.Add(UIName, baseUI);
                return baseUI;
            }
            return null;
        }

        void LoadUIToShowUIs(string UIName)
        {
            BaseUI baseUI;
            BaseUI baseUIFormCache;

            showUIs.TryGetValue(UIName, out baseUI);

            if (baseUI != null)
                return;

            cacheUIs.TryGetValue(UIName, out baseUIFormCache);

            if (baseUIFormCache != null)
            {
                Debug.Log("load UI from cache.");
                showUIs.Add(UIName, baseUIFormCache);
                baseUIFormCache.ActivateTrue();
            }
        }

        void PushUIToStack(string UIName)
        {
            BaseUI baseUI;
            //判断栈中是否有其他的窗体，有则冻结
            if (staUIs.Count > 0)
            {
                BaseUI topUI = staUIs.Peek();
                topUI.Freeze();
            }
            //判断UI所有窗体 是否有指定的UI窗体 有就处理
            cacheUIs.TryGetValue(UIName, out baseUI);
            if (baseUI != null)
            {
                baseUI.ActivateTrue();
                //把指定UI窗体，入栈
                staUIs.Push(baseUI);
            }
            else
                Debug.Log($"baseUI==null,请确认窗口是否有BaseUI脚本，参数:{UIName}");
        }

        void ExitUI(string UIName)
        {
            BaseUI baseUI;
            //"正在显示集合"如果没有记录 则直接返回
            showUIs.TryGetValue(UIName, out baseUI);
            if (baseUI == null)
                return;
            //指定窗体，标记为隐藏状态,从正在显示集合中移除
            baseUI.ActivateFalse();
            showUIs.Remove(UIName);
        }

        void PopUIToStack()
        {
            BaseUI baseUI = staUIs.Pop();
            baseUI.ActivateFalse();
            if (staUIs.Count >= 2)
            {
                //下一个窗体重新显示
                staUIs.Peek().ReActivateTrue();
            }
        }

        void EnterUIAndHideOther(string UIName)
        {
            BaseUI baseUI;
            BaseUI baseUIFromAll;
            if (string.IsNullOrEmpty(UIName))
                return;
            showUIs.TryGetValue(UIName, out baseUI);
            if (baseUI != null)
                return;
            //把正在显示集合 栈集合都隐藏
            foreach (var item in showUIs)
            {
                item.Value.ActivateFalse();
            }

            foreach (var item in staUIs)
            {
                item.ActivateFalse();
            }

            //把当前窗体加入到"正在显示窗体"集合中,且做显示处理
            cacheUIs.TryGetValue(UIName, out baseUIFromAll);

            if (baseUIFromAll != null)
            {
                showUIs.Add(UIName, baseUIFromAll);
                baseUIFromAll.ActivateTrue();
            }
        }

        void ExitUIAndDisPlayOther(string UIName)
        {
            BaseUI baseUI;

            if (string.IsNullOrEmpty(UIName))
                return;
            showUIs.TryGetValue(UIName, out baseUI);
            if (baseUI == null)
                return;
            //把当前窗口失活移除
            baseUI.ActivateFalse();
            showUIs.Remove(UIName);
            //把正在显示集合 栈集合都激活
            foreach (var item in showUIs)
            {
                item.Value.ReActivateTrue();
            }

            foreach (var item in staUIs)
            {
                item.ReActivateTrue();
            }
        }

        bool ClearStack()
        {
            if (staUIs != null && staUIs.Count >= 1)
            {
                staUIs.Clear();
                return true;
            }
            return false;
        }


        #region 显示"UI管理器"内部核心数据，测试使用

        /// <summary>
        /// 显示所有UI窗体的数量
        /// </summary>
        /// <returns></returns>
        public int ShowCacheUIsCount()
        {
            return cacheUIs?.Count ?? 0;
        }
        /// <summary>
        /// 显示当前窗体的数量
        /// </summary>
        /// <returns></returns>
        public int ShowShowUIsCount()
        {
            return showUIs?.Count ?? 0;
        }
        /// <summary>
        /// 显示栈窗体的数量
        /// </summary>
        /// <returns></returns>
        public int ShowStaUIsCount()
        {
            return staUIs?.Count ?? 0;
        }
        #endregion
    }
}
