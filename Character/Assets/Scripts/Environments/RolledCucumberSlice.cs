using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class RolledCucumberSlice : MonoBehaviour
    {
        private float time;
        private bool isRolled = true;
        // Start is called before the first frame update
        void Start()
        {
            EventCenter.AddListener(EventType.KnifeStop, StopRolling);
            RollOfCucumber();
            time = 0;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isRolled)
            {
                time += Time.fixedDeltaTime;
                if (Mathf.Abs(time - 3) < 0.001)
                {
                    RollOfCucumber();
                    time = 0;
                }
            }
        }

        public void StopRolling()
        {
            isRolled = false;
        }

        void RollOfCucumber()
        {
            GameObject cucumber = Instantiate(Resources.Load("Prefabs/CucumberSlice")) as GameObject;
            cucumber.transform.parent = transform;
        }
    }
}

