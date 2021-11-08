using System.Collections;
using System;
using UnityEngine;

namespace Demo
{
    public class ExtractorHoodSwitch : MonoBehaviour
    {
        public PankapuConfig config;
        private Transform socket;
        private Transform plugOff;
        private Transform plugIn;
        private Transform trigger;
        private Transform plugTrigger;
        private BuoyancyEffector2D buoyancy;
        private BoxCollider2D boxCollider;

        private float radius = 1.0f;
        private float maxDensity = 5.0f;
        private bool isTurnOn = false;
        private bool isCollider = false;
        private bool isBeginFloating = false;
        private bool isPressing = false;

        Vector2 boxSize;
        void Awake()
        {
            socket = transform.Find("Socket");
            plugOff = transform.Find("PlugOff");
            plugIn = transform.Find("PlugIn");
            trigger = transform.Find("Trigger");
            plugTrigger = transform.Find("PlugTrigger");
            buoyancy = GetComponent<BuoyancyEffector2D>();
            boxCollider = GetComponent<BoxCollider2D>();
            boxSize = boxCollider.size;
            boxCollider.size = Vector2.zero;

            EventCenter.AddListener(EventType.AnRespawn, Reset);

            Physics2D.IgnoreCollision(boxCollider, FindObjectOfType<An>().GetComponent<CapsuleCollider2D>());
        }

        void FixedUpdate()
        {
            CheckPlug(plugTrigger);
            if (InputManager.InteractiveBtnDown && isCollider)
            {
                isPressing = true;
            }
            if (isPressing && InputManager.InteractiveBtnUp)
            {
                if (!isTurnOn)
                {
                    plugIn.gameObject.SetActive(true);
                    socket.gameObject.SetActive(false);
                    plugOff.gameObject.SetActive(false);
                    isTurnOn = true;
                    boxCollider.size = boxSize;

                }
                else
                {
                    plugOff.gameObject.SetActive(true);
                    socket.gameObject.SetActive(true);
                    plugIn.gameObject.SetActive(false);
                    isTurnOn = false;
                    boxCollider.size = Vector2.zero;
                }
                isPressing = false;
            }
            if (isTurnOn)
            {
                if (!isBeginFloating)
                {
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(trigger.position, radius, config.pankapu);
                    foreach (var item in colliders)
                    {
                        if (item.transform.name == "An'")
                        {
                            isBeginFloating = true;
                        }
                    }
                }
                else
                {
                    if (buoyancy.density < maxDensity)
                        buoyancy.density += 0.05f;
                }
            }
            else
            {
                buoyancy.density = 0;
            }
        }

        void Destroy()
        {
            EventCenter.RemoveListener(EventType.AnRespawn, Reset);
        }
        private void CheckPlug(Transform plug)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(plug.position, radius, 1 << LayerMask.NameToLayer("Pankapu"));
            string[] names = new string[colliders.Length];
            for (int i = 0; i < colliders.Length; i++)
                names[i] = colliders[i].transform.name;
            if (Array.IndexOf(names, "An'") != -1)
            {
                isCollider = true;
            }
            else
            {
                isCollider = false;
            }
        }
        private void Reset()
        {
            isTurnOn = false;
            isCollider = false;
            isBeginFloating = false;
            isPressing = false;
            plugOff.gameObject.SetActive(true);
            socket.gameObject.SetActive(true);
            plugIn.gameObject.SetActive(false);
            if (boxCollider)
                boxCollider.size = Vector2.zero;
        }
    }
}
