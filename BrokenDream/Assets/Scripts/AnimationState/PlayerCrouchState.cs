using System;
using UnityEngine;

namespace Demo
{
    public class PlayerCrouchState : MachineBehaviourState<Pankapu>
    {
        private static PlayerCrouchState m_Instance;
        public static PlayerCrouchState Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new PlayerCrouchState();
                return m_Instance;
            }
        }

        Collider2D[] colliders = null;

        private PlayerCrouchState() { }

        public override void Enter()
        {
            owner.isCrouch = true;
            if (colliders == null)
                colliders = owner.GetComponents<BoxCollider2D>();

            foreach (var item in colliders)
                item.enabled = false;
        }

        public override void Excute()
        {
            owner.MoveHorizontally();
            if (!Input.GetKey(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                owner.StateMachine.ChangeState(PlayerIdleState.Instance);
            }
        }

        public override void Exit()
        {
            owner.isCrouch = false;
            foreach (var item in colliders)
                item.enabled = true;
        }
    }
}
