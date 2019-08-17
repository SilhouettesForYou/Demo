namespace Demo
{
    public class PlayerGlobalState : MachineBehaviourState<AssistPlayerControl>
    {
        private static PlayerGlobalState m_Instance;
        public static PlayerGlobalState Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new PlayerGlobalState();
                return m_Instance;
            }
        }

        private PlayerGlobalState() { }

        public override void Enter()
        {

        }

        public override void Excute()
        {
            if (owner.isDead)
            {
                owner.OnAssistPlayerBeHurtToDead();
            }
        }

        public override void Exit()
        {

        }
    }
}
