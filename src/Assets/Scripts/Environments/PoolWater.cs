using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class PoolWater : MonoBehaviour
    {
        private bool done;
        private bool isInflowTrigger;
        private bool isLeak;
        private bool isLeakTrigger;
        private bool isSendLeaking;
        private bool triggerUp = false;
        private bool triggerDown = false;

        private Transform water;
        private Transform waterSwitch;
        private Transform drops;
        private Animator animator;

        private float height = 0;
        private float pixelsPerUnitWater;
        private float widthOfWater;
        private float heightOfWater;
        private float yOfWaterBottom;
        private Vector3 scaleOfWater;
        private Vector3 scaleOfParent;

        public float deltaOfWaterScale = 2f;
        public float scaleRate = 0.05f;
        private float checkRadius = 0.1f;

        // Start is called before the first frame update
        void Awake()
        {
            done = false;
            isInflowTrigger = false;
            isLeak = false;
            isLeakTrigger = false;
            isSendLeaking = false;

            // Find some transforms
            water = transform.Find("Bottom");
            waterSwitch = transform.Find("Swer").Find("Switch");
            drops = transform.Find("Swer").Find("Drops");
            animator = transform.Find("Swer").Find("Trigger").GetComponent<Animator>();

            // Find the ancher of water bottom.
            pixelsPerUnitWater = water.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
            widthOfWater = water.GetComponent<SpriteRenderer>().sprite.rect.width / pixelsPerUnitWater;
            heightOfWater = water.GetComponent<SpriteRenderer>().sprite.rect.height / pixelsPerUnitWater;
            widthOfWater *= water.localScale.x;
            heightOfWater *= water.localScale.y;
            widthOfWater *= transform.localScale.x;
            heightOfWater *= transform.localScale.y;
            scaleOfWater = water.localScale;
            scaleOfParent = transform.localScale;

            // scale the water bottom
            water.localScale = new Vector3(water.localScale.x, 0, water.localScale.z);
            yOfWaterBottom = water.position.y - heightOfWater / 2;
            water.position = new Vector3(water.position.x, yOfWaterBottom, water.localScale.z);
            yOfWaterBottom = water.localPosition.y;

            EventCenter.AddListener(EventType.TurnWaterFaucetOn, TurnOutfallOn);
            EventCenter.AddListener(EventType.WaterLeak, SetWaterLeak);
            EventCenter.AddListener(EventType.AnRespawn, Reset);
        }

        // Update is called once per frame
        void Update()
        {
            animator.SetBool("Up", triggerUp);
            animator.SetBool("Down", triggerDown);

            CheckSwitchInteractive();
            if (isInflowTrigger)
            {
                EventCenter.Braodcast<bool>(EventType.FrezeeAll, true);
                EventCenter.Braodcast<FocusOn>(EventType.FocusOn, FocusOn.FocusOnPool);
                SurfaceUp(water, scaleOfWater, deltaOfWaterScale, yOfWaterBottom);
                SetAncher(water, widthOfWater, heightOfWater);
            }
            if (done)
            {
                EventCenter.Braodcast<bool>(EventType.FrezeeAll, false);
                EventCenter.Braodcast<FocusOn>(EventType.FocusOn, FocusOn.FocusOnPankapu);
                isInflowTrigger = false;
                EventCenter.Braodcast(EventType.TurnWaterFaucetOff);
            }

            if (isLeak && !isLeakTrigger)
            {
                SurfaceDown(water, scaleOfWater, deltaOfWaterScale, scaleRate, yOfWaterBottom);
                if (height < heightOfWater / 3)
                {
                    Debug.Log("Frezee");
                    EventCenter.Braodcast<bool>(EventType.FrezeeAll, true);
                }
            }
        }

        void OnDestroy()
        {
            EventCenter.RemoveListener(EventType.TurnWaterFaucetOn, TurnOutfallOn);
            EventCenter.RemoveListener(EventType.WaterLeak, SetWaterLeak);
            EventCenter.RemoveListener(EventType.AnRespawn, Reset);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.name == "An'")
            {
                EventCenter.Braodcast<Transform>(EventType.DivingInPool, transform.Find("Water").Find("Bottom"));
                
            }
        }

        private void SetAncher(Transform parent, float width, float height)
        {
            Transform leftTop = parent.Find("AncherLeftTop");
            Transform rightTop = parent.Find("AncherRightTop");
            Transform leftBottom = parent.Find("AncherLeftBottom");
            Transform rightBottom = parent.Find("AncherRightBottom");

            leftTop.position = new Vector3(parent.position.x - width / 2, parent.position.y - height / 2, parent.position.z);
            rightTop.position = new Vector3(parent.position.x + width / 2, parent.position.y - height / 2, parent.position.z);
            leftBottom.position = new Vector3(parent.position.x - width / 2, parent.position.y + height / 2, parent.position.z);
            rightBottom.position = new Vector3(parent.position.x + width / 2, parent.position.y + height / 2, parent.position.z);
        }

        private void SurfaceUp(Transform t, Vector3 originalScale, float delta, float yOfBottom)
        {
            Vector3 updatedScale = t.localScale;
            if (updatedScale.y < originalScale.y)
            {
                updatedScale.y += Time.deltaTime * delta;
                float y = yOfBottom + (heightOfWater / (originalScale.y * scaleOfParent.y)) * updatedScale.y / 2;
                Vector3 pos = new Vector3(t.localPosition.x, y, t.localPosition.z);
                t.localPosition = pos;
                t.localScale = updatedScale;
            }
            else
            {
                done = true;
            }
        }

        private void SurfaceDown(Transform t, Vector3 originalScale, float delta, float scaleRate, float yOfBottom)
        {
            Vector3 updatedScale = t.localScale;
            if (updatedScale.y > 0)
            {
                updatedScale.y -= (delta * scaleRate * Time.deltaTime);
                t.localScale = updatedScale;
                height = heightOfWater / (originalScale.y * scaleOfParent.y) * updatedScale.y;
                float y = yOfBottom + (heightOfWater / (originalScale.y * scaleOfParent.y)) * updatedScale.y / 2;
                Vector3 pos = new Vector3(t.localPosition.x, y, t.localPosition.z);
                t.localPosition = pos;
            }
            else
            {
                isLeak = false;
            }
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(5.0f);
            isLeak = true;
            drops.gameObject.SetActive(true);
        }

        private void CheckSwitchInteractive()
        {
            Collider2D collider = Physics2D.OverlapCircle(waterSwitch.position, checkRadius,
                1 << LayerMask.NameToLayer("Pankapu"));

            if (collider != null && !InputManager.InteractiveBtnUp)
            {
                isLeakTrigger = true;
                drops.gameObject.SetActive(false);
                triggerUp = true;
                triggerDown = false;
            }
            else
            {
                triggerUp = false;
                triggerDown = true;
                isLeakTrigger = false;
                drops.gameObject.SetActive(true);
            }
        }

        private void TurnOutfallOn()
        {
            isInflowTrigger = true;
        }

        private void SetWaterLeak()
        {
            if (!isSendLeaking)
            {
                StartCoroutine(Delay());
                isSendLeaking = true;
            }
        }

        private void Reset()
        {
            done = false;
            isInflowTrigger = false;
            isLeak = false;
            isLeakTrigger = false;
            isSendLeaking = false;
            triggerUp = false;
            triggerDown = false;
            if (water != null)
                water.localScale = new Vector3(water.localScale.x, 0, water.localScale.z);
        }

    }
}
