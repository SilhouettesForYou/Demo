using System.Collections;
using UnityEngine;

namespace Demo
{
    public class ExtractorHoodSwitch : MonoBehaviour
    {
        private Transform turnOn;
        private Transform turnOff;
        private Transform extractorHood;
        private Transform trigger;
        private BuoyancyEffector2D buoyancy;
        private BoxCollider2D boxCollider;

        private float radius = 0.1f;
        private float maxSize = 15.0f;
        private bool isTurnOn = false;
        private bool isCollider = false;
        private bool isBeginFloating = false;

        Vector2 boxSize;
        void Awake()
        {
            turnOn = transform.Find("TurnOn");
            turnOff = transform.Find("TurnOff");
            extractorHood = transform.Find("ExtractorHood");
            trigger = transform.Find("Trigger");
            buoyancy = extractorHood.GetComponent<BuoyancyEffector2D>();
            boxCollider = extractorHood.GetComponent<BoxCollider2D>();
            boxSize = boxCollider.size;
            boxCollider.size = Vector2.zero;
        }

        void FixedUpdate()
        {
            if (InputManager.InteractiveBtnDown && isCollider)
            {
                if (!isTurnOn)
                {
                    turnOff.gameObject.SetActive(false);
                    turnOn.gameObject.SetActive(true);
                    isTurnOn = true;
                    boxCollider.size = boxSize;

                }
                else
                {
                    turnOff.gameObject.SetActive(true);
                    turnOn.gameObject.SetActive(false);
                    isTurnOn = false;
                    boxCollider.size = Vector2.zero;
                }
            }

            if (turnOn)
            {
                if (!isBeginFloating)
                {
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(trigger.position, radius, 1 << LayerMask.NameToLayer("Player"));
                    foreach (var item in colliders)
                    {
                        if (item.transform.name == "An'")
                        {
                            isBeginFloating = true;

                        }
                    }
                }
                else if (boxCollider.size.x < maxSize)
                {
                    buoyancy.density += 0.05f;
                }
            }
            else
            {
                buoyancy.density = 1;
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.transform.name == "An'")
            {
                isCollider = true;
                StartCoroutine(DelayPress());
            }
        }


        IEnumerator DelayPress()
        {
            yield return new WaitForSeconds(0.2f);
            isCollider = false;
        }
    }
}
