using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class PlayerSwinState : MachineBehaviourState<AssistPlayerControl>
    {

        private static PlayerSwinState m_Instance;
        public static PlayerSwinState Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new PlayerSwinState();
                return m_Instance;
            }
        }
        public override void Enter()
        {
            
        }

        public override void Excute()
        {
            
        }

        public override void Exit()
        {
         
        }
    }
}
