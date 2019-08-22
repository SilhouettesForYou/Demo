using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class HomeUI : BaseUI
    {
        private float smoothSpeed = 5;
        private bool isAchievementPanelOn = true;
        private bool isRankPanelOn = false;
        private Vector2 central;
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
            central = achievementPanel.position;

            //注册按钮
            RegisterBtnOnClick("AchievementBtn", ChangeToAchievementPanel);
            RegisterBtnOnClick("RankBtn", ChangeToRankPanel);
            RegisterBtnOnClick("BackBtn", Back);
        }

        public void ChangeToAchievementPanel(GameObject go)
        {
            if (!isAchievementPanelOn)
            {
                Show(achievementPanel);
                Hide(rankPanel);
                isAchievementPanelOn = true;
                isRankPanelOn = false;
            }
        }

        public void ChangeToRankPanel(GameObject go)
        {
            if (!isRankPanelOn)
            {
                Show(rankPanel);
                Hide(achievementPanel);
                isRankPanelOn = true;
                isAchievementPanelOn = false;
            }
        }

        public void Back(GameObject go)
        {
            Close();
        }

        private void Show(Transform panel)
        {
            panel.GetComponent<CanvasGroup>().alpha = 1;
            panel.GetComponent<CanvasGroup>().interactable = true;
            panel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        private void Hide(Transform panel)
        {
            panel.GetComponent<CanvasGroup>().alpha = 0;
            panel.GetComponent<CanvasGroup>().interactable = false;
            panel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }
}
