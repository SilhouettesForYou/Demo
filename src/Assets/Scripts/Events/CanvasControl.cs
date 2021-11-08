using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Demo
{
    public class CanvasControl : BaseUI
    {
        public static string passTime;
        public static string sceneName;
        private Text time;
        private string skillName;

        int hour;
        int minute;
        int second;
        int millisecond;
        float timeSpend = 0.0f;
        void Awake()
        {
            Image leftBtn = GameObject.Find("RunLeft").GetComponent<Image>();
            Image rightBtn = GameObject.Find("RunRight").GetComponent<Image>();
            Image jumpBtn = GameObject.Find("Jump").GetComponent<Image>();
            Image interactiveBtn = GameObject.Find("Interactive").GetComponent<Image>();
            Image skillBtn = GameObject.Find("Attack").GetComponent<Image>();
            Image pause = GameObject.Find("Pause").GetComponent<Image>();
            time = GameObject.Find("CostTime").GetComponent<Text>();
            Time.timeScale = 1;
            Scene scene = SceneManager.GetActiveScene();
            sceneName = scene.name;
            if (scene.name.Equals("Level-1"))
            {
                Sprite sprite = Resources.Load("Sprites/Frezee", typeof(Sprite)) as Sprite;
                skillBtn.overrideSprite = sprite;
                skillName = "Sprites/Frezee";
                AudioManager.PlayBackground("MusicLevelOneBackground");
            }
            else if (scene.name.Equals("Level-2"))
            {
                Sprite sprite = Resources.Load("Sprites/Sword", typeof(Sprite)) as Sprite;
                skillBtn.overrideSprite = sprite;
                skillName = "Sprites/Sword";
                AudioManager.PlayBackground("MusicLevelTwoBackground");
            }

            UIEventListener leftBtnListener = leftBtn.gameObject.AddComponent<UIEventListener>();
            leftBtnListener.OnKeyDown += delegate (GameObject button)
            {
                EventCenter.Braodcast<bool>(EventType.RunLeft, true);
                EventCenter.Braodcast<bool>(EventType.LeftBtnUp, false);

                Sprite sprite = Resources.Load("Sprites/GoLeftDown", typeof(Sprite)) as Sprite;
                leftBtn.overrideSprite = sprite;
            };
            leftBtnListener.OnKeyUp += delegate (GameObject button)
            {
                EventCenter.Braodcast<bool>(EventType.RunLeft, false);
                EventCenter.Braodcast<bool>(EventType.LeftBtnUp, true);

                Sprite sprite = Resources.Load("Sprites/GoLeft", typeof(Sprite)) as Sprite;
                leftBtn.overrideSprite = sprite;
            }; 

            UIEventListener rightBtnListener = rightBtn.gameObject.AddComponent<UIEventListener>();
            rightBtnListener.OnKeyDown += delegate (GameObject button)
            {
                EventCenter.Braodcast<bool>(EventType.RunRight, true);
                EventCenter.Braodcast<bool>(EventType.RightBtnUp, false);

                Sprite sprite = Resources.Load("Sprites/GoRightDown", typeof(Sprite)) as Sprite;
                rightBtn.overrideSprite = sprite;
            };
            rightBtnListener.OnKeyUp += delegate (GameObject button)
            {
                EventCenter.Braodcast<bool>(EventType.RunRight, false);
                EventCenter.Braodcast<bool>(EventType.RightBtnUp, true);

                Sprite sprite = Resources.Load("Sprites/GoRight", typeof(Sprite)) as Sprite;
                rightBtn.overrideSprite = sprite;
            };

            UIEventListener jumpBtnListener = jumpBtn.gameObject.AddComponent<UIEventListener>();
            jumpBtnListener.OnKeyDown += delegate (GameObject button)
            {
                EventCenter.Braodcast<bool>(EventType.Jump, true);

                Sprite sprite = Resources.Load("Sprites/JumpDown", typeof(Sprite)) as Sprite;
                jumpBtn.overrideSprite = sprite;
            };
            jumpBtnListener.OnKeyUp += delegate (GameObject button)
            {
                EventCenter.Braodcast<bool>(EventType.Jump, false);

                Sprite sprite = Resources.Load("Sprites/Jump", typeof(Sprite)) as Sprite;
                jumpBtn.overrideSprite = sprite;
            };

            UIEventListener interactiveBtnListener = interactiveBtn.gameObject.AddComponent<UIEventListener>();
            interactiveBtnListener.OnKeyDown += delegate (GameObject button)
            {
                EventCenter.Braodcast<bool>(EventType.Interactive, true);
                EventCenter.Braodcast<bool>(EventType.InteractiveUp, false);

                Sprite sprite = Resources.Load("Sprites/InteractiveDown", typeof(Sprite)) as Sprite;
                interactiveBtn.overrideSprite = sprite;
            };
            interactiveBtnListener.OnKeyUp += delegate (GameObject button)
            {
                EventCenter.Braodcast<bool>(EventType.Interactive, false);
                EventCenter.Braodcast<bool>(EventType.InteractiveUp, true);

                Sprite sprite = Resources.Load("Sprites/Interactive", typeof(Sprite)) as Sprite;
                interactiveBtn.overrideSprite = sprite;
            };

            UIEventListener skillBtnListener = skillBtn.gameObject.AddComponent<UIEventListener>();
            skillBtnListener.OnKeyDown += delegate (GameObject button)
            {
                EventCenter.Braodcast<bool>(EventType.Skill, true);
                Sprite sprite = Resources.Load(skillName + "Down", typeof(Sprite)) as Sprite;
                skillBtn.overrideSprite = sprite;
            };
            skillBtnListener.OnKeyUp += delegate (GameObject button)
            {
                EventCenter.Braodcast<bool>(EventType.Skill, false);
                Sprite sprite = Resources.Load(skillName, typeof(Sprite)) as Sprite;
                skillBtn.overrideSprite = sprite;
            };

            UIEventListener pauseBtnListener = pause.gameObject.AddComponent<UIEventListener>();
            pauseBtnListener.OnKeyDown += delegate (GameObject button)
            {
                OpenUI(UINameConst.PauseUI);
                Time.timeScale = 0;
            };
        }

        void Update()
        {
            timeSpend += Time.deltaTime;

            hour = (int)timeSpend / 3600;
            minute = ((int)timeSpend - hour * 3600) / 60;
            second = (int)timeSpend - hour * 3600 - minute * 60;
            millisecond = (int)((timeSpend - (int)timeSpend) * 1000);

            time.text = string.Format("{0:D2}:{1:D2}.{2:D3}", minute, second, millisecond);
            passTime = string.Format("{0:D2} min {1:D2} s", minute, second);
        }
    }
}
