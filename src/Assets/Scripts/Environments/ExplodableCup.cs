using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodableCup : MonoBehaviour
{
    Animator animator;
    bool exploded = false;
    GameObject dropCube;
    Vector3 posDefault;
    public GameObject explodeDrop;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();   
        posDefault = transform.position;
        Demo.EventCenter.AddListener(Demo.EventType.AnDeath, reset);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        //Debug.Log("aaa");
    }
     void OnTriggerEnter2D(Collider2D  other) 
    {
        //Debug.Log("inside");
        if (other.GetComponent<Injurious>() != null) {
            animator.SetBool("Dead",true);
            exploded = true;
            StartCoroutine(dropLater(other));
        }else {
             //Debug.Log("not injurios");
        }
    }

    void drop(Collider2D fork)
    {
        dropCube = Instantiate(explodeDrop,fork.transform.position,fork.transform.rotation);
        Transform weapon =fork.transform.Find("Point");
        fork.gameObject.SetActive(false);
      
        transform.position = new Vector3(-1000,-1000,-1000);
    }
     void OnTriggerExit2D(Collider2D  other) 
    {
        
    }
    IEnumerator dropLater(Collider2D fork)
    {
        yield return new WaitForSeconds(1f);
        drop(fork);
    }

    void reset()
    {
    
        gameObject.SetActive(true);
        Destroy(dropCube);
        transform.position = posDefault;
         animator.SetBool("Dead",false);
    }
}
