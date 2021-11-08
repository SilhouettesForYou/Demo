using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosetController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject panPrefab;
    Collider2D collider2;
    Vector3 posInit;
    void Start()
    {
        posInit = transform.position;
        collider2 =  GetComponent<Collider2D>();
         Demo.EventCenter.AddListener(Demo.EventType.AnDeath, reset);
    }

    void reset()
    {
        transform.position = posInit;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
    bool hasLayer(LayerMask mask, int layerId) 
    {
        return (mask.value & (1<<layerId)) != 0;
    }

    public LayerMask trigerLayer;
    
    bool falling = false;
    void OnTriggerEnter2D(Collider2D  other)
    {
        
        if (hasLayer(trigerLayer,other.gameObject.layer)) {
            //Debug.Log("enter");
            if (other.transform.position.y < transform.position.y) {
                GameObject instance = (GameObject)Instantiate(panPrefab, panPrefab.transform.position, panPrefab.transform.rotation);
                Rigidbody2D rigidbody2D  = instance.GetComponent<Rigidbody2D>();
                rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                instance.AddComponent<Injurious>();
                rigidbody2D.gravityScale = 0.5f;
                collider2.isTrigger = true;
            }else {
                //抖动并下落
                if (!falling) {
                    StartCoroutine(ShakeAndFall());
                    falling = true;
                }
               
            }
        }
       
    }
    void OnTriggerExit2D(Collider2D  other)
    {
       // Debug.Log("out");
        if (hasLayer(trigerLayer,other.gameObject.layer)) {
            Debug.Log("false");
            //collider2D.isTrigger = false;
        }
        
    }


    IEnumerator ShakeAndFall()
    {
        Vector3 pos = transform.position;
        for (int i =0 ;i < 20; i++) {
            float d = i%5==0?0.05f:-0.05f;
            transform.position = new Vector3(pos.x,pos.y+d,pos.z);
            yield return i;
        }
        collider2.isTrigger = true;           
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        panPrefab.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        gameObject.GetComponent<Rigidbody2D>().mass = 100;
        PanController.falled = true;
    }
}
