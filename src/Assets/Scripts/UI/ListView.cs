using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Demo
{
    public enum ItemType { AchievementL1, AchievementL2, AchievementOther, RankL1, RankL2 };
    public class ListView : MonoBehaviour
    {
        private ScrollRect scrollRect;
        private VerticalLayoutGroup content;
        private Transform contentCache;
        [HideInInspector]
        public List<GameObject> achieveItemsL1 = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> rankItemsL1 = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> achieveItemsL2 = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> rankItemsL2 = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> achieveItemsOther = new List<GameObject>();


        void Update()
        {
            
        }

        public void Init()
        {
            scrollRect = GetComponent<ScrollRect>();
            content = scrollRect.GetComponentInChildren<VerticalLayoutGroup>(true);
            contentCache = transform.Find("Cache");
            scrollRect.content = content.transform as RectTransform;
        }

        public void AddItem(GameObject item, ItemType type)
        {
            item.transform.SetParent(contentCache);
            switch (type)
            {
                case ItemType.AchievementL1:
                    achieveItemsL1.Add(item);
                    break;
                case ItemType.RankL1:
                    rankItemsL1.Add(item);
                    break;
                case ItemType.AchievementL2:
                    achieveItemsL2.Add(item);
                    break;
                case ItemType.RankL2:
                    rankItemsL2.Add(item);
                    break;
            }
        }

        public void Show(ItemType type)
        {
            Remove();
            switch (type)
            {
                case ItemType.AchievementL1:
                    ShowAll(achieveItemsL1);
                    break;
                case ItemType.RankL1:
                    ShowAll(rankItemsL1);
                    break;
                case ItemType.AchievementL2:
                    ShowAll(achieveItemsL2);
                    break;
                case ItemType.RankL2:
                    ShowAll(rankItemsL2);
                    break;
            }
            
        }

        private void ShowAll(List<GameObject> items)
        {
            foreach (var item in items)
            {
                item.transform.SetParent(content.transform);
            }
        }

        private void Remove()
        {
            for (int i = content.transform.childCount - 1; i >= 0; i--)
            {
                Transform item = content.transform.GetChild(i);
                item.transform.SetParent(contentCache);
            }
        }


    }
}
