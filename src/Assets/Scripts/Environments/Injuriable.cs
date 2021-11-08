using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Injuriable : MonoBehaviour
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
        //Debug.Log("dead");
       if (other.GetComponent<Injurious>() != null) {
            if (transform.name == "An'")
                Demo.EventCenter.Braodcast(Demo.EventType.PankapuDeath);
            if (transform.name == "An")
                Demo.EventCenter.Braodcast(Demo.EventType.AnDeath);
        }

    }
}
