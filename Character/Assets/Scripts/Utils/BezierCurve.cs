using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Demo
{
    enum FaceDirection { right, left };
    class Fish
    {
        private Transform fish { get; set; }

        private Transform bottom;
        public FaceDirection facing { get; set; }
        private Vector3 position { get; set; }
        private Vector3[] controlPoints { get; set; }

        private float[] constrainX = new float[2];
        private float[] constrainY = new float[2];
        private float offset = 0.5f;
        public Fish(Transform fish, int initNum, FaceDirection f)
        {
            this.fish = fish;
            position = fish.position;
            Constrain();
            controlPoints = GenerateControlPoints(initNum, f);
        }

        public void SetControlPoints(int num)
        {
            position = fish.position;
            if (facing == FaceDirection.left)
            {
                facing = FaceDirection.right;
                fish.GetComponent<SpriteRenderer>().flipX = !fish.GetComponent<SpriteRenderer>().flipX;
            }
            else if (facing == FaceDirection.right)
            {
                facing = FaceDirection.left;
                fish.GetComponent<SpriteRenderer>().flipX = !fish.GetComponent<SpriteRenderer>().flipX;
            }

            controlPoints = GenerateControlPoints(num, facing);
        }

        private void Constrain()
        {
            bottom = fish.parent;
            float pixelsPerUnit = bottom.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
            float width = bottom.GetComponent<SpriteRenderer>().sprite.rect.width / pixelsPerUnit;
            float height = bottom.GetComponent<SpriteRenderer>().sprite.rect.height / pixelsPerUnit;
            width *= bottom.localScale.x;
            height *= bottom.localScale.y;
            constrainX[0] = bottom.position.x - width / 2;
            constrainX[1] = bottom.position.x + width / 2;
            constrainY[0] = bottom.position.y - height / 2;
            constrainY[1] = bottom.position.y + height / 2;
        }


        private Vector3 ComputeBezierCurve(Vector3[] points, float t)
        {
            List<Vector3> curve = new List<Vector3>();
            for (int k = 1; k < points.Length; k++)
            {
                for (int i = 0; i < points.Length - k; i++)
                {
                    Vector3 p_ik;
                    if (k == 1)
                    {
                        Vector3 p0 = points[i];
                        Vector3 p1 = points[i + 1];
                        p_ik = new Vector3(p0.x * (1 - t) + p1.x * t, p0.y * (1 - t) + p1.y * t, 0);
                        curve.Add(p_ik);
                        continue;
                    }
                    float x = curve[i].x * (1 - t) + curve[i + 1].x * t;
                    float y = curve[i].y * (1 - t) + curve[i + 1].y * t;
                    curve[i] = new Vector3(x, y, 0);
                }
            }
            return curve[0];
        }

        private Vector3[] GenerateControlPoints(int numOfControl, FaceDirection facing)
        {
            int direction = 1;
            float bandX = (constrainX[1] - constrainX[0]) / numOfControl;
            List<Vector3> points = new List<Vector3>();
            points.Add(position);
            if (facing == FaceDirection.left)
                direction = -1;
            else if (facing == FaceDirection.right)
                direction = 1;
            for (int i = 0; i < numOfControl; i++)
            {
                float x;
                float y;
                if (i % 2 == 0)
                    y = Random.Range(constrainY[0] + offset, (constrainY[0] + constrainY[1]) / 2);
                else
                    y = Random.Range((constrainY[0] + constrainY[1]) / 2, constrainY[1] - offset);
                x = Random.Range(position.x + direction * i * bandX, position.x + direction * (i + 1) * bandX);
                points.Add(new Vector3(x, y, 0));
            }
            return points.ToArray();
        }

        public void MoveFish(float t)
        {
            Vector3 point = ComputeBezierCurve(controlPoints, t);
            fish.transform.position = point;
        }
    }
    public class BezierCurve : MonoBehaviour
    {
        private float time = 0;
        Fish fish1;
        Fish fish2;
        Fish fish3;
        void Awake()
        {
            fish1 = new Fish(transform.Find("Fish-1"), 5, FaceDirection.right);
            fish1.facing = FaceDirection.right;

            fish2 = new Fish(transform.Find("Fish-2"), 3, FaceDirection.right);
            fish2.facing = FaceDirection.right;

            fish3 = new Fish(transform.Find("Fish-3"), 4, FaceDirection.left);
            fish3.facing = FaceDirection.left;
        }

        void Update()
        {
            time += 0.01f;
            if (time <= 1)
            {
                fish1.MoveFish(time);
                fish2.MoveFish(time);
                fish3.MoveFish(time);
            }
            else
            {
                time = 0;
                fish1.SetControlPoints(5);
                fish2.SetControlPoints(3);
                fish3.SetControlPoints(4);
            }
        }
    }
}
