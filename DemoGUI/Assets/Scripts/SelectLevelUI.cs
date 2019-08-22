using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace Demo
{
    class SelectLevelUI : BaseUI
    {
        
        void Awake()
        {
            RegisterBtnOnClick("PersonInfo", BtnHome);
            RegisterBtnOnClick("SelectLevelClose", BtnClose);
        }

        public void BtnHome(GameObject go)
        {
            OpenUI(UINameConst.HomeUI);
        }

        public void BtnClose(GameObject go)
        {
            Close();
        }
    }
}

