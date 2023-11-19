using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkToClick : MonoBehaviour
{
    public Vector3     topleft;
    public Vector3 bottomright;
    private bool[,] grid;

    private float f_total_cost = 0f;
    private float g_distance_from_start = 0f;
    private float h_estimated_distance_to_end = 0f;

    
    // Start is called before the first frame update
    void Start()
    {
        Collider2D playerCollider = GetComponent<Collider2D>();
        int l = (int)     topleft.x;
        int t = (int)     topleft.y;
        int r = (int) bottomright.x;
        int b = (int) bottomright.y;
        grid = new bool[r - l,t - b];
        for (int x = l; x < r; x++) {
            for (int y = b; y < t; y++) {
                if (playerCollider.bounds.Contains(new Vector3((float) x, (float) y, 0f))) {    
                    grid[x,y] = false; 
                }
                else {
                    grid[x,y] = true;
                }
            }
        }
    }

    public List<Vector3> AStar(Vector3 end) {

        (int, int) startc = ((int) transform.position.x, (int) transform.position.y);
        (int, int)   endc = ((int) end.x,                (int) end.y);

        Node s       = new Node(null, startc, 0f);
        Node e       = new Node(null,   endc, 0f);
        Node current = null;

        SortedSet<Node>       open = new SortedSet<Node>(new NodeComparer());
        HashSet<(int, int)> closed = new   HashSet<(int, int)>();
        open.Add(s);

        while (open.Count > 0 && !(closed.Contains(endc))) {
            current = open.Min;
            open.Remove(current);
            foreach ((int, int) loc in neighbors(current)) {
                if (!(closed.Contains(loc)) && grid[loc.Item1, loc.Item2]) {
                    float g = DistanceEstimate(startc, loc);
                    float h = DistanceEstimate(endc,   loc);
                    Node newNode = new Node(current, loc, g+h); 
                    open.Add(newNode);
                }
            }
            closed.Add(current.position);
        }

        List<Vector3> path = new List<Vector3>();
        Node x = e;
        path.Add(new Vector3((float) x.position.Item1, (float) x.position.Item2, 0f));
        while (x.parent != null) {
            x = x.parent;
            path.Add(new Vector3((float) x.position.Item1, (float) x.position.Item2, 0f));
        }
        path.Reverse();

        return path;
    }

    public (int, int)[] neighbors(Node n) {
        int x = n.position.Item1;
        int y = n.position.Item2;
        (int, int)[] ns = { (x + 1, y), (x - 1, y), (x, y + 1), (x, y - 1) };
        return ns;
    }

    public float DistanceEstimate((int, int) v1, (int, int) v2) {
        return Mathf.Abs(v1.Item1 - v2.Item1) + Mathf.Abs(v2.Item2 - v1.Item2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
