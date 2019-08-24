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
        private Animator animator;
        private Transform waterFlow;
        // Start is called before the first frame update
        void Start()
        {
            isTrigger = false;
            EventCenter.AddListener(EventType.TurnWaterFaucetOff, TurnOutfallOff);
            waterFlow = transform.parent.Find("WaterFlow");
            animator = waterFlow.GetComponent<Animator>();
            yScale = waterFlow.localScale.y;
            waterFlow.gameObject.SetActive(false);
            //waterFlow.localScale = new Vector3(waterFlow.localScale.x, 0, waterFlow.localScale.z);
        }

        // Update is called once per frame
        void Update()
        {
            if (off)
            {
                animator.SetBool("Flow", false);
                waterFlow.gameObject.SetActive(false);
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!isTrigger)
            {
                EventCenter.Braodcast(EventType.TurnWaterFaucetOn);
                isTrigger = true;
                waterFlow.gameObject.SetActive(true);
                animator.SetBool("Flow", true);
            }
        }

        void TurnOutfallOff()
        {
            off = true;
        }
    }

    

}
