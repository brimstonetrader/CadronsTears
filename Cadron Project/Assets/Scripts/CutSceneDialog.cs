using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class CutSceneDialog : MonoBehaviour
{
    public string[] lines;
    private int line;

    public TextMeshProUGUI charactername;
    public GameObject characterportrait;
    public GameObject backButton;
    public GameObject nextButton;
    public TextMeshProUGUI nexttext;

    public bool speaking = false;
    public void nextClick(){
<<<<<<< HEAD
        if(line >= lines.Length - 1)
        {
        GameManager.Instance.playerBusy(false);
        GameManager.Instance.EndCutscene();
        }
        else {
            line += 1;
            if (line == (lines.Length - 1)) {
                // set button text to done
                nexttext.text = "Done";
            }
            backButton.SetActive(true);
            // nextButton.SetActive(false);
            GameManager.Instance.DialogShow(lines[line]); 
            //StartCoroutine(WaitingForNext());            
        }        
=======
        if(GameManager.Instance.CheckTextDone(lines[line])){
            if(line >= lines.Length - 1)
            {
            GameManager.Instance.EndCutscene();
            speaking = false;
            }
            else{
                line += 1;
                if(line == (lines.Length - 1)){
                // set button text to done
                nexttext.text = "Done";
                }
                backButton.SetActive(true);
            // nextButton.SetActive(false);
                GameManager.Instance.DialogShow(lines[line]); 
                //StartCoroutine(WaitingForNext());
                
                
            }
        }
        else{
            GameManager.Instance.FinishText(lines[line]);
        }    
>>>>>>> upstream/main
    }

    public void done(string nextscene) {
        GameManager.Instance.ChangeScene(nextscene);
    }

    public void backClick(){
        if(line > 0){
            line--;
            GameManager.Instance.DialogShow(lines[line]);
            nexttext.text = "Next";
            if (line == 0) {
                backButton.SetActive(false);
            }
        }
    }
    public void StartCutscene(string name, Sprite portrait){
        GameManager.Instance.playerBusy(true); 
        line = 0;
        if(lines.Length > 1){
            nexttext.text = "Next";
        }
        backButton.SetActive(false);
        charactername.text = name;
        characterportrait.GetComponent<Image>().overrideSprite = portrait;
        speaking = true;
        StartCoroutine(WaitingForNext());
    }

    IEnumerator WaitingForNext()
    {
        while(speaking){
            if(Input.GetKeyDown(KeyCode.E)){
                if(GameManager.Instance.CheckTextDone(lines[line])){
                    nextClick();
                }
                else{
                    GameManager.Instance.FinishText(lines[line]);
                }
            }
            yield return null;
        }
       
    }
    
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (nextButton.activeSelf) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                nextClick();
            }
        }
    }
}
