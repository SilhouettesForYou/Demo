using System.Collections;
using UnityEngine;

namespace Demo
{
    public class PlayerAttackState : MachineBehaviourState<Pankapu>
    {
        private static PlayerAttackState m_Instance;
        public static PlayerAttackState Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new PlayerAttackState();
                return m_Instance;
            }
        }

        private bool isFire = false;

        public override void Enter()
        {

        }

        public override void Excute()
        {
            if (!isFire)
                owner.FireInTheHole();
            isFire = true;
            owner.isAttack = false;
            owner.IsGrounded();
            owner.StateMachine.ChangeState(owner.StateMachine.PreState);
        }

        public override void Exit()
        {
            owner.StartCoroutine(PerformDuration());
        }

        IEnumerator PerformDuration()
        {
            yield return new WaitForSeconds(0.5f);
            isFire = false;
        }
    }
}
