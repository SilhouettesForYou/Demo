using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class InitAchievementVerticalItems : MonoBehaviour
    {
        private ListView achievementVerticalListView;
        void Awake()
        {
            achievementVerticalListView = GetComponent<ListView>();
            for (int i = 0; i < 10; i++)
            {
                GameObject item = Instantiate(Resources.Load<GameObject>("UIPrefabs/AchievementItems")) as GameObject;
                achievementVerticalListView.AddItem(item.gameObject);
            }
        }
    }
}
