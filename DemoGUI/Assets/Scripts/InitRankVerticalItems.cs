using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class InitRankVerticalItems : MonoBehaviour
    {
        private ListView rankVerticalListView;
        void Awake()
        {
            rankVerticalListView = GetComponent<ListView>();
            for (int i = 0; i < 10; i++)
            {
                GameObject item = Instantiate(Resources.Load<GameObject>("UIPrefabs/RankItems")) as GameObject;
                rankVerticalListView.AddItem(item.gameObject);
            }
        }
    }
}
