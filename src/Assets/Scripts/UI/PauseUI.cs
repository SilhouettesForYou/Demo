using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Demo
{
    public class PauseUI : BaseUI
    {
        void Awake()
        {
            curUIType.type = UIFormType.Normal;
            curUIType.mode = UIFormShowMode.Normal;
            curUIType.lucencyType = UIFormLucencyType.Lucency;


            RegisterBtnOnClick("Continue", Continue);
            RegisterBtnOnClick("Restart", Restart);
            RegisterBtnOnClick("BackToMainMenu", BackToMainMenu);
            RegisterBtnOnClick("Settings", Settings);
        }

        private void Continue(GameObject go)
        {
            Time.timeScale = 1;
            Close();
        }

        private void Restart(GameObject go)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Close();
        }

        private void BackToMainMenu(GameObject go)
        {
            SceneManager.LoadScene(0);
            UIManager.lastUIName = "PauseUI";
            Close();
        }

        private void Settings(GameObject go)
        {
            OpenUI(UINameConst.SettingsUI);
        }
    }
}
