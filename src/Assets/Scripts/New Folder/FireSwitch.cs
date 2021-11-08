using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnTriggerEnter2D(Collider2D  other)
    {
        if (other.GetComponent<Demo.Pankapu>() != null &&  other.transform.position.x < transform.position.x) { //在左边
            transform.Rotate(0,0,-20);
            FlameController.trigered = true;
            //falling = true;
       }
    }
}
