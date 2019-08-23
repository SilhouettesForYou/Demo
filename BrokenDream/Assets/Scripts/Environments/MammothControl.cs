using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class MammothControl : MonoBehaviour
    {
        private bool isTrigger;
        private bool off = false;
        private float yScale;
        private readonly float delta = 0.25f;

        public delegate void TurnOutfallOn();
        public static event TurnOutfallOn turnOn;

        private Transform waterFlow;
        // Start is called before the first frame update
        void Start()
        {
            isTrigger = false;
            PoolWater.turnOff += TurnOutfallOff;

            Transform outfall = transform.parent.Find("Outfall");
            waterFlow = outfall.Find("WaterFlow");
            yScale = waterFlow.localScale.y;
            waterFlow.gameObject.SetActive(false);
            //waterFlow.localScale = new Vector3(waterFlow.localScale.x, 0, waterFlow.localScale.z);
        }

        // Update is called once per frame
        void Update()
        {
            if (off)
            {
                if (yScale > 0)
                {
                    yScale -= Time.deltaTime * delta;
                    waterFlow.localScale = new Vector3(waterFlow.localScale.x, yScale, waterFlow.localScale.z);
                }
                else
                {
                    waterFlow.gameObject.SetActive(false);
                }
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!isTrigger)
            {
                turnOn();
                isTrigger = true;
                waterFlow.gameObject.SetActive(true);
            }
        }

        void TurnOutfallOff()
        {
            off = true;
        }
    }

    

}
