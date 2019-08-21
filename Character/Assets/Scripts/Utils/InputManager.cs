using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
   public class InputManager : MonoBehaviour
    {
        private bool frezeeAll = false;

        public static bool LeftBtnDown = false;
        public static bool LeftBtnHeld = false;
        public static bool RightBtnDown = false;
        public static bool RightBtnHeld = false;
        public static bool JumpBtnDown = false;
        public static bool InteractiveBtnDown = false;
        public static bool InteractiveBtnHeld = false;
        public static bool SkillBtnDown = false;
        public static bool SkillBtnHeld = false;
        private void Awake()
        {
            EventCenter.AddListener<bool>(EventType.FreezeAll, IsFrezeeAll);
            EventCenter.AddListener<bool>(EventType.RunLeft, OnLeftBtnDown);
            EventCenter.AddListener<bool>(EventType.RunRight, OnRightBtnDown);
            EventCenter.AddListener<bool>(EventType.Jump, OnJumpBtnDwon);
            EventCenter.AddListener<bool>(EventType.Interactive, OnInteractiveBtnDown);
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
                InteractiveBtnDown = flag;
        }

        private void OnSkillBtnDown(bool flag)
        {
            SkillBtnDown = flag;
            if (frezeeAll)
                SkillBtnDown = false;
        }


    }
}
