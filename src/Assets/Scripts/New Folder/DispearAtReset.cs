using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispearAtReset : MonoBehaviour
{
    protected bool destroyed = false;
    // Start is called before the first frame update
    void Start()
    {
         Demo.EventCenter.AddListener(Demo.EventType.AnDeath, dispear);
        Demo.EventCenter.AddListener(Demo.EventType.KnifeStop, dispear);
    }
    public void dispear()
    {
        if (!destroyed) {
            Destroy(gameObject);
            destroyed = true;
        }
      
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        Demo.EventCenter.RemoveListener(Demo.EventType.AnDeath, dispear);
    }

    
}
