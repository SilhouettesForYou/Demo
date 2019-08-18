using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class UIType
    {
        public bool isClearStack = false;
        public UIFormType type = UIFormType.Normal;
        public UIFormShowMode mode = UIFormShowMode.Normal;
        public UIFormLucencyType lucencyType = UIFormLucencyType.Lucency;
    }

    public class BaseUI : MonoBehaviour
    {
        public UIType curUIType { get; set; } = new UIType();

        #region four state of form

        public virtual void ActivateTrue()
        {
            gameObject.SetActive(true);
        }
        public virtual void ActivateFalse()
        {
            gameObject.SetActive(false);
        }
        public virtual void ReActivateTrue()
        {
            gameObject.SetActive(true);
        }
        public virtual void Freeze()
        {
            gameObject.SetActive(true);
        }

        #endregion

        #region common methods

        public void RegisterBtnOnClick(string btnName, EventTriggerListener.VoidDelegate del)
        {
            Transform button = UnityHelper.Find(gameObject.transform, btnName);
            EventTriggerListener.Get(button?.gameObject).onClick = del;
        }

        public void RegisterBtnOnClick(Transform button, EventTriggerListener.VoidDelegate del)
        {
            EventTriggerListener.Get(button?.gameObject).onClick = del;
        }

        public void OpenUI(string UIName)
        {
            //UIManager.GetInstance().ShowUI(UIName);
            UIManager.instance.ShowUI(UIName);
        }

        public void Close()
        {
            string UIName;
            UIName = GetType().ToString();
            UIManager.instance.CloseUI(UIName);
        }

        #endregion
    }
}
