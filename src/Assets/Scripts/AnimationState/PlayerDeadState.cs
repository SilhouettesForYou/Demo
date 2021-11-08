using System.Collections;
using UnityEngine;

namespace Demo
{
    public class PlayerDeadState : MachineBehaviourState<Pankapu>
    {
        private static PlayerDeadState m_Instance;
        public static PlayerDeadState Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new PlayerDeadState();
                return m_Instance;
            }
        }

        private float timer = 0;

        private PlayerDeadState() { }
        public override void Enter()
        {

        }

        public override void Excute()
        {
            if (owner.isDead)
            {
                EventCenter.Braodcast<bool>(EventType.FrezeeAll, true);
                timer += Time.deltaTime;
                if (timer > 0.5f)
                {
                    Delay();
                    timer = 0;
                }
            }

            if (owner.isAnDead)
            {
                EventCenter.Braodcast<FocusOn>(EventType.FocusOn, FocusOn.FocusOnPankapu);
                owner.isDead = false;
                owner.isAnDead = false;
            }
        }

        public override void Exit()
        {

        }

        private void Delay()
        {
            EventCenter.Braodcast<FocusOn>(EventType.FocusOn, FocusOn.FocusOnAn);
        }
    }
}
