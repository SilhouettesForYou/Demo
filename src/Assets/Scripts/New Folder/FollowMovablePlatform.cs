using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMovablePlatform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        WithMovePlatform();
    }

    public Transform groundCheck;
    public LayerMask MovePlatformLayer;
    Vector2 lastP;
    bool onMovePlatform = false;
    bool WithMovePlatform() 
    {
        Vector2 position = groundCheck.position;
        Vector2 direction = Vector2.down;
        float distance = 0.3f;

        RaycastHit2D hit = Physics2D.Raycast(position,  direction, distance, MovePlatformLayer);
        if (hit.collider != null)
        {
            //Debug.Log("check");
            
            Vector2 p = hit.transform.position;
            if (!onMovePlatform) {
                lastP = p;
                onMovePlatform = true;
                return false;
                
            }
            Vector2 relateDis = p - lastP;
            gameObject.transform.position += new Vector3(relateDis.x,relateDis.y, 0);
            lastP = p;
         
            //mainRole.velocity = hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity;
            return true;
        }
        onMovePlatform = false;
        return false;
    }
}
