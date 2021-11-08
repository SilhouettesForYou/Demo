using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    int fallingDis = -6;
    float msFalling = 0.6f;

    IEnumerator fishFalling(GameObject gameObject)
    {
        Vector2 pos = gameObject.transform.position;
        FishGo fishGo = gameObject.GetComponent<FishGo>();
        float speed = fishGo.swimmingSpeed;
        fishGo.swimmingSpeed = 0;
        int countStep = 20;
        float deltaY = -0.5f;
        for (int i = 0; i < countStep; i++) {
            pos.y += deltaY;
            gameObject.transform.position = pos;
            deltaY *= 1-1.0f/countStep;
           yield return i;
        }
        fishGo.swimmingSpeed = speed;
    }
    void OnTriggerEnter2D(Collider2D  other)
    {
        if (other.tag == "Fish") {
            StartCoroutine (fishFalling(other.gameObject));
        }
        //other.isTrigger = true;
    }
    void OnTriggerExit2D(Collider2D  other)
    {
        //Debug.Log("enter");
         if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
          
         }
        //other.isTrigger = false;
    }
}
