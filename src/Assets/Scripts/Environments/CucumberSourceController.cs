using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CucumberSourceController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject slicePrefab;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D  other) 
    {
       
    }
    void OnTriggerExit2D(Collider2D  other) 
    {
      
        if (other.gameObject.layer != LayerMask.NameToLayer("Cutter"))
            return;
        if (slicePrefab == null)
            return;

       GameObject go = Instantiate(slicePrefab,slicePrefab.transform.position,slicePrefab.transform.rotation);
       go.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        go.AddComponent<DispearAtReset>();
       go.AddComponent<Injurious>();
        //Debug.Log("cucum");
      // go.AddComponent<DispearAtReset>();
    }
}
