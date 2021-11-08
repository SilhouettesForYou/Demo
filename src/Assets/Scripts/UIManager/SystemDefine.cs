using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Demo
{
    public enum UIFormType
    {
        Normal, Fixed, PopUp
    }

    public enum UIFormShowMode
    {
        Normal, ReverseChange, HideOther
    }

    public enum UIFormLucencyType
    {
        Lucency, Translucence, ImPenetrable, Pentrate
    }

    public class SystemDefine 
    {
        public const string canvasPath = "UIPrefabs/Canvas";
        public const string canvasTag = "Canvas";
        public const string normalNode = "Normal";
        public const string fixedNode = "Fixed";
        public const string popUpNode = "PopUp";
        public const string ScriptsMgrNode = "ScriptsMgr";
    }
}


