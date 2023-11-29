using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WalkToClick : MonoBehaviour
{
    public Vector3     topleft;
    public Vector3 bottomright;
    private bool[,] grid;
    public Tilemap tilemap;
    public Collider2D obstacle;
    private Dictionary<Vector3, Vector3> parentmap;
    private Rigidbody2D rb;
    private Stack<Vector3> path;
    private int t;
    private int b;
    private int l;
    private int r;
    private Vector3 mouseWorldPos;
    private bool newDestination;
    
    void Start()
    {
        newDestination = false;
        path = new Stack<Vector3>();
        //Assemble 2D grid of all integer points, with boolean false if this point 
        //has something that collides with the player on it.
        l = (int) Mathf.Round(    topleft.x);
        t = (int) Mathf.Round(    topleft.y);
        r = (int) Mathf.Round(bottomright.x);
        b = (int) Mathf.Round(bottomright.y);
        grid = new bool[t - b,r - l];
        for (int x = 0; x < r - l; x++) {
            for (int y = 0; y < t - b; y++) {
                Vector3Int cellPosition = new Vector3Int(x + l, y + b, 0);
                Vector3 cellCenter = tilemap.GetCellCenterWorld(cellPosition);
//                print($"cell position: {cellPosition}, cell center: {cellCenter}");
                // Check if the cell is occupied by a tile
                grid[y, x] = !IsCellOccupied(cellCenter);
//                if (grid[y,x]) { print("hit! " + (x + l) + ", " + (y + b)); }
            }
        }
        rb = GetComponent<Rigidbody2D>();
    }

    private bool IsCellOccupied(Vector3 cellCenter)
    {
        // Convert world position to tilemap position
        Vector3Int cellPosition = tilemap.WorldToCell(cellCenter);
        // int[] next = {-1, 0, 1};
        // foreach (int a in next) {
        //     foreach (int b in next) {
                if (tilemap.HasTile(new Vector3Int(cellPosition.x, cellPosition.y, 0))) {
                    return true;
                }
        //     }
        // }
        return false;
    }


    public Stack<Vector3> AStar(Vector3 end) {

        parentmap = new Dictionary<Vector3, Vector3>();
        
        (int, int) startc = ((int) Mathf.Round(transform.position.x), (int) Mathf.Round(transform.position.y));
        (int, int)   endc = ((int) Mathf.Round(               end.x), (int) Mathf.Round(               end.y));

        Node s       = new Node(startc, DistanceEstimate(startc, endc));
        Node e       = new Node(endc,   DistanceEstimate(startc, endc));
        Node current = s;

        //open contains all nodes, sorted by heuristic value
        SortedSet<Node>       open = new SortedSet<Node>(new NodeComparer());
        open.Add(s);

        //closed contains all previously explored positions
        HashSet<(int, int)> closed = new   HashSet<(int, int)>();

        while (open.Count > 0 && !(closed.Contains(endc))) {
            current = open.Min;
            open.Remove(current);
            foreach ((int, int) loc in neighbors(current)) {
                if (!(closed.Contains(loc)) && grid[loc.Item2 - b, loc.Item1 - l]) {
                    // value = dist from start + dist from end
                    float g = DistanceEstimate(startc, loc);
                    float h = DistanceEstimate(endc,   loc);
                    Node newNode = new Node(loc, g+h); 
                    open.Add(newNode);
                    parentmap[vec(newNode.position)] = vec(current.position);
                }
            }
            closed.Add(current.position);
        }

        Stack<Vector3> path = new Stack<Vector3>();
        Vector3 x = vec(endc);
        path.Push(vec(endc));
        foreach (KeyValuePair<Vector3, Vector3> vc  in parentmap) {
            print("(" + vc.Key.x + ", " + vc.Key.y + ")");
        }
        while (parentmap.ContainsKey(x)) {
            x = parentmap[x];
            // print(x.x);
            // print(x.y);
            path.Push(x);
        }

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
        if (Input.GetMouseButtonDown(0)) {
            mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            // rb.velocity = new Vector2(mouseWorldPos.x - transform.position.x, mouseWorldPos.y - transform.position.y);
            Vector3Int v3int = new Vector3Int((int) Mathf.Round(mouseWorldPos.x), (int) Mathf.Round(mouseWorldPos.y), 0);
            Vector3 v3 = tilemap.GetCellCenterWorld(v3int);
            if (!IsCellOccupied(v3)) {
                path = AStar(mouseWorldPos);
                string ot = "";
                foreach (Vector3 p in path) { 
                    ot +=  $"({p.x}, {p.y})";
                }
                print(ot);
            }
        }
        if (path.Count > 0) {
            Vector3 targetDirection = (path.Peek() - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, path.Peek(), 2 * Time.deltaTime);
            rb.velocity = new Vector2(targetDirection.x, targetDirection.y);
            if (Vector3.Distance(transform.position, path.Peek()) < 0.05) {
                path.Pop();                
                newDestination = true;
            } 
            // if (newDestination) {
                if      (targetDirection.x >  0.01) { PlayerMovement.SetHorizontal(1); } 
                else if (targetDirection.x < -0.01) { PlayerMovement.SetHorizontal(-1); } 
                else if (targetDirection.y >  0.01) { PlayerMovement.SetVertical(1); } 
                else if (targetDirection.y < -0.01) { PlayerMovement.SetVertical(1); } 
                else                                { PlayerMovement.SetHorizontal(1); } 
                newDestination = false;
            // }
        }
    }   
    void OnCollisionEnter2D(Collision2D collision) {
        path = new Stack<Vector3>();
        newDestination = true;
    }

    Vector3 vec((int, int) coord) {
        Vector3 v = new Vector3((float) coord.Item1,(float) coord.Item2, 0f);
        return v;
    }
}
