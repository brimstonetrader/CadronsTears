using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsAutoscroll : MonoBehaviour
{
    private Scrollbar scrollbar;
    // Start is called before the first frame update
    void Start()
    {
        scrollbar = gameObject.GetComponent<Scrollbar>();
    }


    public void ToBeginning(){
        scrollbar.value = 1;
    }
    // Update is called once per frame
    void Update()
    {
        if(scrollbar.value > 0 && Input.GetAxisRaw("Mouse ScrollWheel") == 0){
            scrollbar.value -= 0.00001f;    
        }
        
    }
}
