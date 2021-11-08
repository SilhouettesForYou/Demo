using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieInStuck : MonoBehaviour
{

    public  int msMaxStayTime = 5000;
    int deadTime;
    float lastPositionX;
    // Start is called before the first frame update
    void Start()
    {
        deadTime = msMaxStayTime;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("up");
        if (Mathf.Abs(transform.position.x - lastPositionX) < 0.0001f) {
            deadTime -= (int)(Time.deltaTime*1000);
            //Debug.Log(deadTime/1000);
        }else {
            deadTime = msMaxStayTime; 
        }
        lastPositionX = transform.position.x;
        if (deadTime <= 0) {
            Demo.EventCenter.Braodcast(Demo.EventType.AnDeath);
        }
    }
}
