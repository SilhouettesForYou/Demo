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

        private float close = 1.0f;
        private float far = 2.5f;

        private GameObject frozenEffect;
        private Transform end1;
        private Transform end2;
        Vector3 end1Position;
        Vector3 end2Position;

        private bool iceFlag = false;
        private PerformAbilitiesState() { }

        public override void Enter()
        {
            frozenEffect = owner.transform.Find("Frozen").gameObject;
            end1 = frozenEffect.transform.Find("End-1");
            end2 = frozenEffect.transform.Find("End-2");

            end1Position = end1.position;
            end2Position = end2.position;

            if (owner.isPerformSkilUnderWater)
            {
                end1.position = new Vector3(end1.position.x, end1.position.y + far * 1.5f, 0);
                end2.position = new Vector3(end2.position.x, end2.position.y + far * 1.5f, 0);
            }
            else
            {
                if (owner.isFacingRight)
                {
                    end1.position = new Vector3(end1.position.x + close, end1.position.y, 0);
                    end2.position = new Vector3(end2.position.x + far, end2.position.y, 0);
                }
                else
                {
                    end1.position = new Vector3(end1.position.x - close, end1.position.y, 0);
                    end2.position = new Vector3(end2.position.x - far, end2.position.y, 0);
                }
            }
        }

        public override void Excute()
        {
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
            end1.position = end1Position;
            end2.position = end2Position;
        }
    }
}
