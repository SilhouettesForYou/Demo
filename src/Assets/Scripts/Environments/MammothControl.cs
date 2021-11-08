using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class MammothControl : MonoBehaviour
    {
        private bool isWaterFlow;
        private bool isTrigger;
        private bool off = false;
        private float yScale;
        private Animator animator;
        private Transform waterFlow;
        // Start is called before the first frame update
        void Awake()
        {
            isTrigger = false;
            EventCenter.AddListener(EventType.TurnWaterFaucetOff, TurnOutfallOff);
            EventCenter.AddListener(EventType.AnRespawn, Reset);
            waterFlow = transform.parent.Find("WaterFlow");
            animator = waterFlow.GetComponent<Animator>();
            yScale = waterFlow.localScale.y;
            waterFlow.gameObject.SetActive(false);
            //waterFlow.localScale = new Vector3(waterFlow.localScale.x, 0, waterFlow.localScale.z);
        }

        // Update is called once per frame
        void Update()
        {
            animator.SetBool("Flow", isWaterFlow);
            if (off)
            {
                isWaterFlow = false;
                waterFlow.gameObject.SetActive(false);
            }
        }

        void OnDestroy()
        {
            EventCenter.RemoveListener(EventType.TurnWaterFaucetOff, TurnOutfallOff);
            EventCenter.RemoveListener(EventType.AnRespawn, Reset);
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!isTrigger && collider.GetComponent<Pankapu>() != null)
            {
                EventCenter.Braodcast(EventType.TurnWaterFaucetOn);
                isTrigger = true;
                waterFlow.gameObject.SetActive(true);
                isWaterFlow = true;
                AudioManager.PlayEffect("MusicWaterFlow");
            }
        }

        void TurnOutfallOff()
        {
            off = true;
        }

        private void Reset()
        {
            isTrigger = false;
            isWaterFlow = false;
        }
    }

    

}
