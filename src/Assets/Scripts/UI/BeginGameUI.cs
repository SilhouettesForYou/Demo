using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Demo
{
    public class BeginGameUI : BaseUI
    {

        // Start is called before the first frame update
        void Awake()
        {
            curUIType.type = UIFormType.Normal;
            curUIType.mode = UIFormShowMode.Normal;
            curUIType.lucencyType = UIFormLucencyType.Lucency;
            UIManager.isStartUILoaded = true;
            PlayerPrefs.SetInt("First", 1);
            RegisterBtnOnClick("BeginGameBtn", StartGame);
        }

        public void StartGame(GameObject go)
        {
            LoadAsyncScene.sceneName = "Level-1";
            OpenUI(UINameConst.BeforeLoadingUI);
            Close();
            //OpenUI(UINameConst.SelectLevelUI);
        }

        
    }
}
