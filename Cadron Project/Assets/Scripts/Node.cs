using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node parent;
    public (int, int) position;
    public float value;

    public Node(Node pr, (int, int) ps, float v)
    {
        this.parent = pr;
        this.position = ps;
        this.value = v;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
