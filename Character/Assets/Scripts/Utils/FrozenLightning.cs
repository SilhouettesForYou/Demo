using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class FrozenLightning : MonoBehaviour
    {
        public Transform end;

        public float updateInterval = 0.5f;

        public int pointCount = 10;
        public float randomOffset = 0.5f;

        Transform[] branch;
        float updateTime = 0;
        Vector3[] points;
        LineRenderer lineRenderer;

        // Use this for initialization
        void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            points = new Vector3[pointCount];
            lineRenderer.positionCount = pointCount;
            lineRenderer.useWorldSpace = false;
        }

        void Update()
        {


            if (Time.time >= updateTime)
            {
                lineRenderer.positionCount = pointCount;

                points[0] = Vector3.zero;
                Vector3 Segment = (end.position - transform.position) / (pointCount - 1);

                for (int i = 1; i < pointCount - 1; i++)
                {
                    points[i] = Segment * i;
                    points[i].y += Random.Range(-randomOffset, randomOffset);
                    points[i].x += Random.Range(-randomOffset, randomOffset);
                }

                points[pointCount - 1] = end.position - transform.position;
                lineRenderer.SetPositions(points);

                updateTime += updateInterval;
            }


        }
    }
}

