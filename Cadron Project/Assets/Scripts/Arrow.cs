using System.Dynamic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public List<GameObject> people;
    public GameObject current;
    private Vector3 playerpos;
    private Vector3 personpos;
    public GameObject player;
    public GameObject arrow;


    // Start is called before the first frame update
    void Start()
    {
        current = people[0];
        Vector3 playerpos = new Vector3();
        Vector3 personpos = new Vector3();
    }

    public void Remove(string charname) {
        if (current.name == charname) {
            people.Remove(current);
          //  gameObject.SetActive(false);
        }
    }

    public void Activate() {
     //   gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (people.Count > 0) {
            current = people[0];
        }
        else {
            current = null;
        }
        playerpos = player.transform.position;
        if (current != null) {
            personpos = current.transform.position;
            Vector3 arrowdir = (personpos - playerpos).normalized;
            int RotationY = 0;
            int RotationX = 0;
            int RotationZ = Convert.ToInt(Mathf.Sin(arrowdir.y) * 90f / Mathf.PI);
            transform.eulerAngles = new Vector3(RotationX, RotationY, RotationZ);
            transform.position = playerpos + (arrowdir / 3);

            
        }
    }
}
