using UnityEngine;

namespace Demo
{
    public class PlayerPushGragState : MachineBehaviourState<Pankapu>
    {
        private static PlayerPushGragState m_Instance;
        public static PlayerPushGragState Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new PlayerPushGragState();
                return m_Instance;
            }
        }

        private bool isBroadcastAttached = false;
        private bool isBroadcastRelease = false;

        private PlayerPushGragState() { }
        public override void Enter()
        {
            //Debug.Log("Begin Drag or Push")
        }

        public override void Excute()
        {
            string pushabelName = owner.CloseEnoughToPushable();
            if (owner.isDead)
            {
                owner.StateMachine.ChangeState(PlayerDeadState.Instance);
            }
            if (owner.isCloseToPushable && InputManager.InteractiveBtnDown)
            {
                owner.isPrePushing = true;
                if (!isBroadcastAttached)
                {
                    EventCenter.Braodcast<bool, Rigidbody2D, string>(EventType.Attach, true, owner.rigid, pushabelName);
                    EventCenter.Braodcast<bool>(EventType.Facing, owner.isFacingRight);
                    EventCenter.Braodcast<bool>(EventType.Push, true);
                    isBroadcastAttached = true;
                }
                
                owner.isReleaseBox = false;
                owner.MoveWithBox();
            }
            else
            {
                owner.isPrePushing = false;
                if (!isBroadcastRelease)
                {
                    EventCenter.Braodcast<bool, Rigidbody2D, string>(EventType.Attach, false, null, "");
                    EventCenter.Braodcast<bool>(EventType.Facing, owner.isFacingRight);
                    EventCenter.Braodcast<bool>(EventType.Push, false);
                    isBroadcastRelease = true;
                }
                
                owner.StateMachine.ChangeState(PlayerIdleState.Instance);
            }
        }

        public override void Exit()
        {
            owner.push = false;
            owner.drag = false;
            isBroadcastAttached = false;
            isBroadcastRelease = false;
        }
    }
}
