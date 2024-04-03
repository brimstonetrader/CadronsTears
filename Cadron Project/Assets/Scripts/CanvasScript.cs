using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{

    public GameObject curtain;

    public GameObject box;
    public GameObject credits;
    public TextMeshProUGUI text;
    void Awake(){
        if (GameManager.Instance.canvas == null){
            GameManager.Instance.canvas = gameObject;
            if(curtain != null){
                GameManager.Instance.curtain = curtain;    
            }
            
            if(box != null){
                GameManager.Instance.dialogBox = box;    
            }
            if(text != null){
                GameManager.Instance.dialogText = text;    
            }
            if(credits != null){
                GameManager.Instance.creditsmenu = credits;    
            }
        }
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