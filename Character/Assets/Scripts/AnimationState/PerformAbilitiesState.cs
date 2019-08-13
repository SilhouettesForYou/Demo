using System.Collections;
using UnityEngine;

namespace Demo
{
    public class PerformAbilitiesState : MachineBehaviourState<AssistPlayerControl>
    {

        private static PerformAbilitiesState m_Instance;
        public static PerformAbilitiesState Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new PerformAbilitiesState();
                return m_Instance;
            }
        }
        private GameObject frozenEffect;
        private bool iceFlag = false;
        private PerformAbilitiesState() { }

        public override void Enter()
        {
            
        }

        public override void Excute()
        {
            frozenEffect = owner.transform.Find("Frozen").gameObject;
            frozenEffect.SetActive(true);
            // place the ice block
            if (!iceFlag && !owner.isPerformSkilUnderWater)
                owner.PlaceIceBlock();
            else if (!iceFlag && owner.isPerformSkilUnderWater)
                owner.PlaceIceCubeUnderWater();
            iceFlag = true;
            owner.isPerformSkill = false;
            owner.StartCoroutine(PerformDuration());
            if (owner.isPerformSkilUnderWater)
                owner.StateMachine.ChangeState(PlayerSwinState.Instance);
            else
                owner.StateMachine.ChangeState(PlayerIdleState.Instance);
        }

        public override void Exit()
        {
            owner.StopCoroutine(PerformDuration());
        }

        IEnumerator PerformDuration()
        {
            yield return new WaitForSeconds(0.5f);
            frozenEffect.SetActive(false);
            iceFlag = false;
        }
    }
}
