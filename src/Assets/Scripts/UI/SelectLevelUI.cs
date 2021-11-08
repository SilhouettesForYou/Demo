using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Demo
{
    class SelectLevelUI : BaseUI
    {
        private Transform passTime;
        private Text ID;

        private static char[] constant = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public static string GenerateRandomNumber(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(10);
            System.Random rd = new System.Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(10)]);
            }
            return newRandom.ToString();
        }

        void Awake()
        {
            AudioManager.PlayBackground("MusicMainMenu");
            RandomUserImage();
            RegisterBtnOnClick("Level-1", LoadLevelOne);
            RegisterBtnOnClick("Level-2", LoadLevelTwo);
            RegisterBtnOnClick("UserImage", GoHome);
            RegisterBtnOnClick("Settings", Settings);
            ID = UnityHelper.Find(transform, "ID").GetComponent<Text>();
            
            ID.text = "ID: " + GenerateRandomNumber(8);
            //Debug.Log(ID.text);
            if (UIManager.isLogin)
            {
                if (!string.IsNullOrEmpty(PlayerPrefs.GetString("LevelOnePass")))
                {
                    passTime = UnityHelper.Find(UnityHelper.Find(transform, "Level-1"), "PassTime");
                    passTime.gameObject.SetActive(true);
                    passTime.Find("Time").GetComponent<Text>().text = PlayerPrefs.GetString("LevelOnePass");
                }

                if (!string.IsNullOrEmpty(PlayerPrefs.GetString("LevelTwoPass")))
                {

                    Transform level2 = UnityHelper.Find(transform, "Level-2");
                    Sprite sprite = Resources.Load("UIElements/Level-2", typeof(Sprite)) as Sprite;
                    level2.GetComponent<Image>().overrideSprite = sprite;
                    UnityHelper.Find(level2, "Lock").gameObject.SetActive(false);

                    passTime = UnityHelper.Find(UnityHelper.Find(transform, "Level-2"), "PassTime");
                    passTime.gameObject.SetActive(true);
                    passTime.Find("Time").GetComponent<Text>().text = PlayerPrefs.GetString("LevelTwoPass");
                }
            }

            if (LevelPassInfo.levelOnePass)
            {
                passTime = UnityHelper.Find(UnityHelper.Find(transform, PassLevel.level), "PassTime");
                passTime.gameObject.SetActive(true);
                passTime.Find("Time").GetComponent<Text>().text = CanvasControl.passTime;
                PlayerPrefs.SetString("LevelOnePass", CanvasControl.passTime);


                Transform level2 = UnityHelper.Find(transform, "Level-2");
                Sprite sprite = Resources.Load("UIElements/Level-2", typeof(Sprite)) as Sprite;
                level2.GetComponent<Image>().overrideSprite = sprite;
                UnityHelper.Find(level2, "Lock").gameObject.SetActive(false);

            }
            if (LevelPassInfo.levelTwoPass)
            {
                passTime = UnityHelper.Find(UnityHelper.Find(transform, PassLevel.level), "PassTime");
                passTime.gameObject.SetActive(true);
                passTime.Find("Time").GetComponent<Text>().text = CanvasControl.passTime;

                PlayerPrefs.SetString("LevelTwoPass", CanvasControl.passTime);

                passTime = UnityHelper.Find(UnityHelper.Find(transform, "Level-1"), "PassTime");
                passTime.gameObject.SetActive(true);
                passTime.Find("Time").GetComponent<Text>().text = 
                    PlayerPrefs.GetString("LevelOnePass", CanvasControl.passTime);

            }
        }



        private void LoadLevelOne(GameObject go)
        {
            LoadAsyncScene.sceneName = "Level-1";
            AudioManager.StopBackgroundMusic();
            OpenUI(UINameConst.LoadingUI);
            Close();
        }

        private void LoadLevelTwo(GameObject go)
        {
            if (LevelPassInfo.levelOnePass)
            {
                LoadAsyncScene.sceneName = "Level-2";
                OpenUI(UINameConst.LoadingUI);
                Close();
            }
            

        }

        private void GoHome(GameObject go)
        {
            OpenUI(UINameConst.HomeUI);
        }

        private void Settings(GameObject go)
        {
            OpenUI(UINameConst.SettingsUI);
        }

        private void RandomUserImage()
        {
            Image image = UnityHelper.Find(transform, "UserImage").GetComponent<Image>();
            string imageID = UIManager.userImageID.ToString();
            Sprite sprite = Resources.Load("Sprites/UserImages/" + imageID, typeof(Sprite)) as Sprite;
            image.overrideSprite = sprite;
        }
    }
}

