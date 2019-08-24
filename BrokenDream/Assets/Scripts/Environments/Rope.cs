using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class Rope : MonoBehaviour
    {
        private Vector3 destiny;
        private float distance = 2.5f;

        private LineRenderer line;
        private GameObject knifeBar;
        private GameObject lastNode;
        private List<GameObject> nodes = new List<GameObject>();

        private int vertexCount = 2;

        void Start()
        {
            //lastNode = Instantiate(Resources.Load("Prefabs/Hook")) as GameObject;
            knifeBar = GameObject.FindGameObjectWithTag("KnifeBar");
            lastNode = transform.gameObject;
            line = GetComponent<LineRenderer>();
            destiny = GameObject.FindGameObjectWithTag("Nail").transform.position;
            distance = Vector3.Distance(destiny, knifeBar.transform.position);
            CreateNode();
            lastNode.GetComponent<HingeJoint2D>().connectedBody = knifeBar.GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            RenderLine();
        }


        private void RenderLine()
        {
            line.positionCount = vertexCount - 1;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (i == 0)
                    line.SetPosition(i, destiny);
                else
                    line.SetPosition(i, nodes[i].transform.position);
            }
            line.SetPosition(nodes.Count, knifeBar.transform.position);
        }

        private void CreateNode()
        {
            Vector3 posToCreate = knifeBar.transform.position - lastNode.transform.position;
            posToCreate.Normalize();
            posToCreate *= distance;
            posToCreate += lastNode.transform.position;

            GameObject go = Instantiate(Resources.Load("Prefabs/Node")) as GameObject;
            go.transform.position = posToCreate;
            go.transform.SetParent(transform);
            lastNode.GetComponent<HingeJoint2D>().connectedBody = go.GetComponent<Rigidbody2D>();
            lastNode = go;

            nodes.Add(lastNode);
            vertexCount++;
        }
    }
}
