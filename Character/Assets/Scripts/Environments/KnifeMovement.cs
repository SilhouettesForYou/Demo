using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class KnifeMovement : MonoBehaviour
    {
        private Transform fixedAncher;
        private Vector3 fixedPoint;
        private float width;
        private float height;
        private readonly float angle = .5f;
        private float time = 0;
        private int count = 0;
        private int maxCount = 80;
        // Start is called before the first frame update
        void Start()
        {
            fixedAncher = transform.Find("FixedAncher");
            // locate the fixed point
            float pixelsPerUnitBackground = transform.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
            width = transform.GetComponent<SpriteRenderer>().sprite.rect.width / pixelsPerUnitBackground;
            height = transform.GetComponent<SpriteRenderer>().sprite.rect.height / pixelsPerUnitBackground;
            width *= transform.localScale.x;
            height *= transform.localScale.y;
            fixedAncher.position = new Vector3(transform.position.x - width / 2, transform.position.y + height / 2, 0);
            fixedAncher.position = RotateRound(fixedAncher.position, transform.position, Vector3.forward, transform.eulerAngles.z);
            fixedAncher.position = RotateRound(fixedAncher.position, transform.position, Vector3.left, transform.eulerAngles.y);
            fixedPoint = fixedAncher.position;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            time += Time.fixedDeltaTime;
            Debug.DrawLine(transform.position, fixedPoint, Color.cyan);
            if (count % maxCount < maxCount / 2)
            {
                transform.position = RotateRound(transform.position, fixedPoint, Vector3.back, angle);
            }
            else
            {
                transform.position = RotateRound(transform.position, fixedPoint, Vector3.forward, angle);
            }
            count += 1;
            if (count == maxCount)
                count = 0;
        }
        private Vector3 RotateRound(Vector3 position, Vector3 center, Vector3 axis, float angle)
        {
            Vector3 point = Quaternion.AngleAxis(angle, axis) * (position - center);
            Vector3 resultVec3 = center + point;
            resultVec3.z = 0;
            return resultVec3;
        }
    }
}

