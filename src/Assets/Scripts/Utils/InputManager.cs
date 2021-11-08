using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Demo
{
    public class InputManager : MonoBehaviour
    {
        private bool frezeeAll = false;

        public static bool LeftBtnDown { get; set; } = false;
        public static bool LeftBtnUp { get; set; } = false;
        public static bool RightBtnDown { get; set; } = false;
        public static bool RightBtnUp { get; set; } = false;
        public static bool JumpBtnDown { get; set; } = false;
        public static bool InteractiveBtnDown { get; set; } = false;
        public static bool InteractiveBtnUp { get; set; } = true;
        public static bool SkillBtnDown { get; set; } = false;
        public static bool SkillBtnUp { get; set; } = true;
        private void Awake()
        {
            EventCenter.AddListener<bool>(EventType.FrezeeAll, IsFrezeeAll);
            EventCenter.AddListener<bool>(EventType.RunLeft, OnLeftBtnDown);
            EventCenter.AddListener<bool>(EventType.LeftBtnUp, OnLeftBtnUp);
            EventCenter.AddListener<bool>(EventType.RunRight, OnRightBtnDown);
            EventCenter.AddListener<bool>(EventType.RightBtnUp, OnRightBtnUp);
            EventCenter.AddListener<bool>(EventType.Jump, OnJumpBtnDwon);
            EventCenter.AddListener<bool>(EventType.Interactive, OnInteractiveBtnDown);
            EventCenter.AddListener<bool>(EventType.InteractiveUp, OnInteractiveBtnUp);
            EventCenter.AddListener<bool>(EventType.Skill, OnSkillBtnDown);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
                LeftBtnDown = true;
            if (Input.GetKeyUp(KeyCode.A))
                LeftBtnDown = false;
            if (frezeeAll)
                LeftBtnDown = false;

            if (Input.GetKeyDown(KeyCode.D))
                RightBtnDown = true;
            if (Input.GetKeyUp(KeyCode.D))
                RightBtnDown = false;
            if (frezeeAll)
                RightBtnDown = false;

            if (Input.GetKeyDown(KeyCode.Space))
                JumpBtnDown = true;
            if (Input.GetKeyUp(KeyCode.Space))
                JumpBtnDown = false;
            if (frezeeAll)
                JumpBtnDown = false;

            if (Input.GetKeyDown(KeyCode.E))
                InteractiveBtnDown = true;
            if (Input.GetKeyUp(KeyCode.E))
                InteractiveBtnDown = false;
            if (frezeeAll)
                InteractiveBtnDown = false;

            if (Input.GetKeyDown(KeyCode.Z))
                SkillBtnDown = true;
            if (Input.GetKeyUp(KeyCode.Z))
                SkillBtnDown = false;
            if (frezeeAll)
                SkillBtnDown = false;
        }

        void OnDestroy()
        {
            EventCenter.RemoveListener<bool>(EventType.FrezeeAll, IsFrezeeAll);
            EventCenter.RemoveListener<bool>(EventType.RunLeft, OnLeftBtnDown);
            EventCenter.RemoveListener<bool>(EventType.LeftBtnUp, OnLeftBtnUp);
            EventCenter.RemoveListener<bool>(EventType.RunRight, OnRightBtnDown);
            EventCenter.RemoveListener<bool>(EventType.RightBtnUp, OnRightBtnUp);
            EventCenter.RemoveListener<bool>(EventType.Jump, OnJumpBtnDwon);
            EventCenter.RemoveListener<bool>(EventType.Interactive, OnInteractiveBtnDown);
            EventCenter.RemoveListener<bool>(EventType.InteractiveUp, OnInteractiveBtnUp);
            EventCenter.RemoveListener<bool>(EventType.Skill, OnSkillBtnDown);
        }

        private void IsFrezeeAll(bool flag)
        {
            frezeeAll = flag;
        }

        private void OnLeftBtnDown(bool flag)
        {
            LeftBtnDown = flag;
            if (frezeeAll)
                LeftBtnDown = false;
        }
        private void OnLeftBtnUp(bool flag)
        {
            LeftBtnUp = flag;
            if (frezeeAll)
                LeftBtnUp = true;
        }

        private void OnRightBtnDown(bool flag)
        {
            RightBtnDown = flag;
            if (frezeeAll)
                RightBtnDown = false;
        }

        private void OnRightBtnUp(bool flag)
        {
            RightBtnUp = flag;
            if (frezeeAll)
                RightBtnUp = true;
        }

        private void OnJumpBtnDwon(bool flag)
        {
            JumpBtnDown = flag;
            if (frezeeAll)
                JumpBtnDown = false;
        }

        private void OnInteractiveBtnDown(bool flag)
        {
            InteractiveBtnDown = flag;
            if (frezeeAll)
                InteractiveBtnDown = false;
        }

        private void OnInteractiveBtnUp(bool flag)
        {
            InteractiveBtnUp = flag;
            if (frezeeAll)
                InteractiveBtnUp = true;
        }
        private void OnSkillBtnDown(bool flag)
        {
            SkillBtnDown = flag;
            if (frezeeAll)
                SkillBtnDown = false;
        }


        public static void ClearInput()
        {
            LeftBtnDown = false;
            LeftBtnUp = true;
            RightBtnDown = false;
            RightBtnUp = true;
            JumpBtnDown = false;
            InteractiveBtnDown = false;
            InteractiveBtnUp = true;
            SkillBtnDown = false;
            SkillBtnUp = true;
        }
    }
}
