using UnityEngine;

namespace Demo
{
    public class SettingsUI : BaseUI
    {
        void Awake()
        {
            curUIType.type = UIFormType.Normal;
            curUIType.mode = UIFormShowMode.Normal;
            curUIType.lucencyType = UIFormLucencyType.Lucency;

            //注册按钮
            RegisterBtnOnClick("SettingCancel", Cancel);
            RegisterBtnOnClick("SettingConfirm", Confirm);
        }

        public void Cancel(GameObject go)
        {
            Close();
        }

        public void Confirm(GameObject go)
        {
            Close();
        }
    }
}
