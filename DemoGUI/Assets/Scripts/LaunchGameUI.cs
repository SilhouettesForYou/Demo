using System;
using System.Collections.Generic;
using UnityEngine;
namespace Demo
{
    public class LaunchGameUI : BaseUI
    {
        void Awake()
        {
            OpenUI(UINameConst.BeginGameUI);
        }
    }
}
