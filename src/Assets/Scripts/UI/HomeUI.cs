using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Demo
{
    public class HomeUI : BaseUI
    {
        private ListView listView;
        private ItemType type = ItemType.AchievementL1;

        private string achieveItem = "UIPrefabs/AchieveItem";
        private string rankItem = "UIPrefabs/RankItem";
        private string itemName = "UIPrefabs/AchieveItem";

        private Image AchieveImage;
        private Image RankImage;
        private Text level1Text;
        private Text level2Text;

        private bool rank = false;
        private bool level2 = false;
        void Awake()
        {
            curUIType.type = UIFormType.Normal;
            curUIType.mode = UIFormShowMode.Normal;
            curUIType.lucencyType = UIFormLucencyType.Lucency;

            listView = UnityHelper.Find(transform, "ScrollView").GetComponent<ListView>();
            listView.Init();
            Init();

            AchieveImage = UnityHelper.Find(transform, "AchievementTitle").GetComponent<Image>();
            RankImage = UnityHelper.Find(transform, "RankTitle").GetComponent<Image>();

            level1Text = UnityHelper.Find(transform, "LevelOneTitle").GetComponent<Text>();
            level2Text = UnityHelper.Find(transform, "LevelTwoTitle").GetComponent<Text>();

            RegisterBtnOnClick("Back", Back);
            RegisterBtnOnClick("AchievementTitle", Achievement);
            RegisterBtnOnClick("RankTitle", Rank);
            RegisterBtnOnClick("LevelOneTitle", LevelOne);
            RegisterBtnOnClick("LevelTwoTitle", LevelTwo);
        }

        private void Init()
        {
            SetItems(ItemType.AchievementL1);
            SetItems(ItemType.AchievementL2);
            SetItems(ItemType.RankL1);
            SetItems(ItemType.RankL2);
            listView.Show(ItemType.AchievementL1);
        }

        private void GetItemType()
        {
            if (!rank && !level2)
            {
                type = ItemType.AchievementL1;
                itemName = achieveItem;
            }
            else if (!rank && level2)
            {
                type = ItemType.AchievementL2;
                itemName = achieveItem;
            }
            else if (rank && ! level2)
            {
                type = ItemType.RankL1;
                itemName = rankItem;
            }
            else
            {
                type = ItemType.RankL2;
                itemName = rankItem;
            }
        }

        public void Back(GameObject go)
        {
            Close();
        }

        private void Achievement(GameObject go)
        {
            ChangeBackgroundToAchieve();
            rank = false;
            GetItemType();
            listView.Show(type);
        }

        private void Rank(GameObject go)
        {
            ChangeBackgroundToRank();
            rank = true;
            GetItemType();
            listView.Show(type);
        }

        private void LevelOne(GameObject go)
        {
            ChangeBackgroundToLevel1();
            level2 = false;
            GetItemType();
            listView.Show(type);
        }

        private void LevelTwo(GameObject go)
        {
            ChangeBackgroundToLevel2();
            level2 = true;
            GetItemType();
            listView.Show(type);
        }

        private void ChangeBackgroundToAchieve()
        {
            AchieveImage.overrideSprite = 
                Resources.Load("UIElements/TitleBackgroundSelected", typeof(Sprite)) as Sprite;
            RankImage.overrideSprite =
                Resources.Load("UIElements/TitleBackgroundUnselected", typeof(Sprite)) as Sprite;
        }

        private void ChangeBackgroundToRank()
        {
            AchieveImage.overrideSprite =
                Resources.Load("UIElements/TitleBackgroundUnselected", typeof(Sprite)) as Sprite;
            RankImage.overrideSprite =
                Resources.Load("UIElements/TitleBackgroundSelected", typeof(Sprite)) as Sprite;
        }

        private void ChangeBackgroundToLevel1()
        {
            level1Text.color = new Color(255, 255, 255);
            level2Text.color = new Color(190, 212, 220);
        }

        private void ChangeBackgroundToLevel2()
        {
            level2Text.color = new Color(255, 255, 255);
            level1Text.color = new Color(190, 212, 220);
        }

        private void SetItems(ItemType type)
        {
            GameObject item;
            Sprite sprite;
            switch (type)
            {
                case ItemType.AchievementL1:

                    item = Instantiate(Resources.Load<GameObject>(achieveItem)) as GameObject;
                    sprite = Resources.Load("UIElements/Level-1", typeof(Sprite)) as Sprite;
                    item.GetComponent<AchieveItem>().SetData(sprite, "手握苍蓝",
                        "通过第一关", "未完成");
                    listView.AddItem(item, type);

                    item = Instantiate(Resources.Load<GameObject>(achieveItem)) as GameObject;
                    sprite = Resources.Load("UIElements/Level-1", typeof(Sprite)) as Sprite;
                    item.GetComponent<AchieveItem>().SetData(sprite, "使命必达",
                        "在三分半内完成第一关", "未完成");
                    listView.AddItem(item, type);

                    item = Instantiate(Resources.Load<GameObject>(achieveItem)) as GameObject;
                    sprite = Resources.Load("UIElements/Level-1", typeof(Sprite)) as Sprite;
                    item.GetComponent<AchieveItem>().SetData(sprite, "遗迹之主",
                        "在两分四十秒内完成第一关", "未完成");
                    listView.AddItem(item, type);

                    item = Instantiate(Resources.Load<GameObject>(achieveItem)) as GameObject;
                    sprite = Resources.Load("UIElements/Level-1", typeof(Sprite)) as Sprite;
                    item.GetComponent<AchieveItem>().SetData(sprite, "第一夜安然",
                        "无死亡通过第一关", "未完成");
                    listView.AddItem(item, type);

                    break;
                case ItemType.AchievementL2:

                    item = Instantiate(Resources.Load<GameObject>(achieveItem)) as GameObject;
                    sprite = Resources.Load("UIElements/Level-2", typeof(Sprite)) as Sprite;
                    item.GetComponent<AchieveItem>().SetData(sprite, "光明降临",
                        "通过第二关", "未完成");
                    listView.AddItem(item, type);

                    item = Instantiate(Resources.Load<GameObject>(achieveItem)) as GameObject;
                    sprite = Resources.Load("UIElements/Level-2", typeof(Sprite)) as Sprite;
                    item.GetComponent<AchieveItem>().SetData(sprite, "战火指路",
                        "在三分半内完成第二关", "未完成");
                    listView.AddItem(item, type);

                    item = Instantiate(Resources.Load<GameObject>(achieveItem)) as GameObject;
                    sprite = Resources.Load("UIElements/Level-2", typeof(Sprite)) as Sprite;
                    item.GetComponent<AchieveItem>().SetData(sprite, "梦中之人",
                        "在两分四十秒内完成第二关", "未完成");
                    listView.AddItem(item, type);

                    item = Instantiate(Resources.Load<GameObject>(achieveItem)) as GameObject;
                    sprite = Resources.Load("UIElements/Level-2", typeof(Sprite)) as Sprite;
                    item.GetComponent<AchieveItem>().SetData(sprite, "第二夜安然",
                        "无死亡通过第二关", "未完成");
                    listView.AddItem(item, type);
                    break;
                case ItemType.RankL1:
                case ItemType.RankL2:
                    itemName = rankItem;
                    break;
            }
        }

        private void LevelOneAchievement()
        {

        }
    }
}
