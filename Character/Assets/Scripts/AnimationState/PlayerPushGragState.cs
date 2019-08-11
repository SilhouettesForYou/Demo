using UnityEngine;

namespace Demo
{
    public class PlayerPushGragState : MachineBehaviourState<AssistPlayerControl>
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

        private PlayerPushGragState() { }
        public override void Enter()
        {
            //Debug.Log("Begin Drag or Push");
        }

        public override void Excute()
        {
            owner.MoveWithBox();
            owner.DragOrPushToIdle();
            if (owner.dragableOrPushable == false || (owner.dragableOrPushable && Input.GetKeyUp(KeyCode.E)))
            {
                Debug.Log("Stop pushing.");
                owner.StateMachine.ChangeState(PlayerIdleState.Instance);
                owner.dragableOrPushable = false;
            }
        }

        public override void Exit()
        {
            owner.dragableOrPushable = false;
        }
    }
}
