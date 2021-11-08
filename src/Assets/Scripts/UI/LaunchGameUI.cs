using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Demo
{
    public class LaunchGameUI : BaseUI
    {

        void Awake()
        {
            if (UIManager.isStartUILoaded && PlayerPrefs.GetInt("First") == 1)
            {
                if (!string.IsNullOrEmpty(UIManager.lastUIName) && 
                    UIManager.lastUIName.Equals("PauseUI"))
                {
                    OpenUI(UINameConst.SelectLevelUI);
                    Close();
                }
                else if (!string.IsNullOrEmpty(UIManager.lastUIName) &&
                    UIManager.lastUIName.Equals("GameUI"))
                {
                    if (LevelPassInfo.levelOnePass && UIManager.lastLevelName.Equals("Level-1"))
                    {
                        OpenUI(UINameConst.PlayOneUI);
                    }
                    if (LevelPassInfo.levelTwoPass && UIManager.lastLevelName.Equals("Level-2"))
                    {
                        OpenUI(UINameConst.PlayTwoUI);
                    }
                }
                
            }
            else
            {
                OpenUI(UINameConst.BeginGameUI);
                PlayerPrefs.SetInt("First", 1);
                UIManager.userImageID = Random.Range(0, 9);
            }
        }
    }
}
