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
        public static bool LeftBtnHeld { get; set; } = false;
        public static bool RightBtnDown { get; set; } = false;
        public static bool RightBtnHeld { get; set; } = false;
        public static bool JumpBtnDown { get; set; } = false;
        public static bool InteractiveBtnDown { get; set; } = false;
        public static bool InteractiveBtnUp { get; set; } = true;
        public static bool SkillBtnDown { get; set; } = false;
        public static bool SkillBtnHeld { get; set; } = false;
        private void Awake()
        {
            EventCenter.AddListener<bool>(EventType.FrezeeAll, IsFrezeeAll);
            EventCenter.AddListener<bool>(EventType.RunLeft, OnLeftBtnDown);
            EventCenter.AddListener<bool>(EventType.RunRight, OnRightBtnDown);
            EventCenter.AddListener<bool>(EventType.Jump, OnJumpBtnDwon);
            EventCenter.AddListener<bool>(EventType.Interactive, OnInteractiveBtnDown);
            EventCenter.AddListener<bool>(EventType.InteractiveUp, OnInteractiveBtnUp);
            EventCenter.AddListener<bool>(EventType.Skill, OnSkillBtnDown);
        }

        private void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.A))
                LeftBtnDown = true;
            if (Input.GetKeyUp(KeyCode.A))
                LeftBtnDown = false;
            if (frezeeAll)
                LeftBtnDown = false;

            if (Input.GetKey(KeyCode.D))
                RightBtnDown = true;
            if (Input.GetKeyUp(KeyCode.D))
                RightBtnDown = false;
            if (frezeeAll)
                RightBtnDown = false;

            if (Input.GetKey(KeyCode.Space))
                JumpBtnDown = true;
            if (Input.GetKeyUp(KeyCode.Space))
                JumpBtnDown = false;
            if (frezeeAll)
                JumpBtnDown = false;

            if (Input.GetKey(KeyCode.E))
                InteractiveBtnDown = true;
            if (Input.GetKeyUp(KeyCode.E))
                InteractiveBtnDown = false;
            if (frezeeAll)
                InteractiveBtnDown = false;

            if (Input.GetKey(KeyCode.Z))
                SkillBtnDown = true;
            if (Input.GetKeyUp(KeyCode.Z))
                SkillBtnDown = false;
            if (frezeeAll)
                SkillBtnDown = false;
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

        private void OnRightBtnDown(bool flag)
        {
            RightBtnDown = flag;
            if (frezeeAll)
                RightBtnDown = false;
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
            LeftBtnHeld = false;
            RightBtnDown = false;
            RightBtnHeld = false;
            JumpBtnDown = false;
            InteractiveBtnDown = false;
            InteractiveBtnUp = true;
            SkillBtnDown = false;
            SkillBtnHeld = false;
        }
    }
}
