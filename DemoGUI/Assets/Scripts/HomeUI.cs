using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class HomeUI : BaseUI
    {
        Transform achievementPanel;
        Transform rankPanel;
        void Awake()
        {
            //定义窗口性质 (默认数值)
            curUIType.type = UIFormType.Normal;
            curUIType.mode = UIFormShowMode.Normal;
            curUIType.lucencyType = UIFormLucencyType.Lucency;

            achievementPanel = UnityHelper.Find(transform, "AchievementPanel");
            rankPanel = UnityHelper.Find(transform, "RankPanel");

            //注册按钮
            RegisterBtnOnClick("AchievementBtn", ChangeToAchievementPanel);
            RegisterBtnOnClick("RankBtn", ChangeToRankPanel);
        }

        public void ChangeToAchievementPanel(GameObject go)
        {
            achievementPanel.gameObject.SetActive(true);
            rankPanel.gameObject.SetActive(false);
        }

        public void ChangeToRankPanel(GameObject go)
        {
            achievementPanel.gameObject.SetActive(false);
            rankPanel.gameObject.SetActive(true);
        }

    }
}
