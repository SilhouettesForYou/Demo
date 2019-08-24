using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Demo
{
    public class CanvasControl : MonoBehaviour
    {
        private Text time;
        private bool isPause = false;

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

            UIEventListener leftBtnListener = leftBtn.gameObject.AddComponent<UIEventListener>();
            leftBtnListener.OnKeyDown += delegate (GameObject button)
            {
                EventCenter.Braodcast<bool>(EventType.RunLeft, true);
            };
            leftBtnListener.OnKeyUp += delegate (GameObject button)
            {
                EventCenter.Braodcast<bool>(EventType.RunLeft, false);
            }; 

            UIEventListener rightBtnListener = rightBtn.gameObject.AddComponent<UIEventListener>();
            rightBtnListener.OnKeyDown += delegate (GameObject button)
            {
                EventCenter.Braodcast<bool>(EventType.RunRight, true);
            };
            rightBtnListener.OnKeyUp += delegate (GameObject button)
            {
                EventCenter.Braodcast<bool>(EventType.RunRight, false);
            };

            UIEventListener jumpBtnListener = jumpBtn.gameObject.AddComponent<UIEventListener>();
            jumpBtnListener.OnKeyDown += delegate (GameObject button)
            {
                EventCenter.Braodcast<bool>(EventType.Jump, true);
            };
            jumpBtnListener.OnKeyUp += delegate (GameObject button)
            {
                EventCenter.Braodcast<bool>(EventType.Jump, false);
            };

            UIEventListener interactiveBtnListener = interactiveBtn.gameObject.AddComponent<UIEventListener>();
            interactiveBtnListener.OnKeyDown += delegate (GameObject button)
            {
                EventCenter.Braodcast<bool>(EventType.Interactive, true);
                EventCenter.Braodcast<bool>(EventType.InteractiveUp, false);
            };
            interactiveBtnListener.OnKeyUp += delegate (GameObject button)
            {
                EventCenter.Braodcast<bool>(EventType.Interactive, false);
                EventCenter.Braodcast<bool>(EventType.InteractiveUp, true);
            };

            UIEventListener skillBtnListener = skillBtn.gameObject.AddComponent<UIEventListener>();
            skillBtnListener.OnKeyDown += delegate (GameObject button)
            {
                EventCenter.Braodcast<bool>(EventType.Skill, true);
            };
            skillBtnListener.OnKeyUp += delegate (GameObject button)
            {
                EventCenter.Braodcast<bool>(EventType.Skill, false);
            };

            UIEventListener pauseBtnListener = pause.gameObject.AddComponent<UIEventListener>();
            pauseBtnListener.OnKeyDown += delegate (GameObject button)
            {
                Time.timeScale = isPause ? 1 : 0;
                isPause = !isPause;
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
            
        }
    }
}
