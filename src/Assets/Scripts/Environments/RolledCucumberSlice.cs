using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class RolledCucumberSlice : MonoBehaviour
    {
        private float time;
        private bool isRolled = true;
        private Transform _cucumber;
        // Start is called before the first frame update
        void Start()
        {
            EventCenter.AddListener(EventType.KnifeStop, StopRolling);
            _cucumber = transform.Find("Cucumber");
            time = 0;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isRolled)
            {
                time += Time.fixedDeltaTime;
                if (time > 3.2f)
                {
                    RollOfCucumber();
                    time = 0;
                }
            }
        }

        void OnDestroy()
        {

            EventCenter.RemoveListener(EventType.KnifeStop, StopRolling);
        }

        public void StopRolling()
        {
            isRolled = false;
        }

        void RollOfCucumber()
        {
            Debug.Log("nimabi");
            GameObject cucumber = Instantiate(Resources.Load("Prefabs/CucumberSlice")) as GameObject;
            cucumber.transform.position = _cucumber.position;
            cucumber.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            cucumber.transform.parent = transform;
        }
    }
}

