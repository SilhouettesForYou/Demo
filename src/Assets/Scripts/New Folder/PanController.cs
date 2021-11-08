using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanController : ExplodableAddon
{
    // Start is called before the first frame update
    Explodable exp;
    public static bool falled = false;
    float height = 0;
    double y = 0;
    void Start()
    {
        exp = gameObject.GetComponent<Explodable>();
        SpriteRenderer tr = gameObject.GetComponent<SpriteRenderer>();
        height = tr.bounds.size.y * transform.localScale.x;
        y = transform.position.y;
    }
    public LayerMask ground;
    // Update is called once per frame
    void Update()
    {
        Vector2 dt =  Vector2.down*height*0.7f;
        Vector3 end = new Vector3(transform.position.x + dt.x, transform.position.y + dt.y, transform.position.z);
        Debug.DrawLine(gameObject.transform.position, end);
        Collider2D hitCollider = Physics2D.Raycast(transform.position,Vector2.down,height ,ground).collider;
        if (hitCollider != null && transform.position.y < y ) {
            Debug.Log("boom");
            exp.explode();
        }
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public override void OnFragmentsGenerated(List<GameObject> fragments)
    {
        foreach (GameObject go in fragments) {
            go.AddComponent<DispearAtReset>();
        }
    }
}
