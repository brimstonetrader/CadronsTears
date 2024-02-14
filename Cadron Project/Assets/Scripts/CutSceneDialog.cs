using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CutSceneDialog : MonoBehaviour
{
    public string[] lines;
    private int line;

    public TextMeshProUGUI charactername;
    public GameObject characterportrait;
    public GameObject backButton;
    public GameObject nextButton;
    public GameObject doneButton;
    public TextMeshProUGUI nexttext;

    public void nextClick(){
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
                doneButton.SetActive(true);

            }
            backButton.SetActive(true);
            // nextButton.SetActive(false);
            GameManager.Instance.DialogShow(lines[line]); 
            //StartCoroutine(WaitingForNext());            
        }        
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
    }

    //IEnumerator WaitingForNext()
    //{
       // yield return new WaitForSeconds(1);
        //nextButton.SetActive(true);
        //backButton.SetActive(true);
    //}
    
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
