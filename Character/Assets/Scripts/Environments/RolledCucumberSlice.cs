using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class RolledCucumberSlice : MonoBehaviour
    {
        private float time;
        // Start is called before the first frame update
        void Start()
        {
            RollOfCucumber();
            time = 0;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            time += Time.fixedDeltaTime;
            if (Mathf.Abs(time - 2) < 0.001)
            {
                RollOfCucumber();
                time = 0;
            }
        }

        void RollOfCucumber()
        {
            GameObject cucumber = Instantiate(Resources.Load("Prefabs/CucumberSlice")) as GameObject;
            cucumber.transform.parent = transform;
        }
    }
}

