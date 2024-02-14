using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    public GameObject dialogBox;
    public TextMeshProUGUI dialogText;
    public GameObject curtain;
    public GameObject canvas;
    public GameObject player;
    private Dictionary<string, bool> letters;
    private bool gamePaused = false;
    private bool raiseLower = false;
    private bool busy;
    private int tentscene;

    private String nextscene;
    private bool newletter;

     void Awake(){
        if (Instance == null){
            Instance = this;
        } else {
        Destroy(gameObject);
    }
    }

    public bool IsLetterDelivered(string key){
        if (letters.ContainsKey(key)){
            return letters[key];
        }
        else
        return false;
    }
    public bool HaveLetter(string key){
        return letters.ContainsKey(key);
    }
    public void UpdateLetterStatus(string key){
        if(letters.ContainsKey(key)){
            letters[key] = !letters[key];
        }
    }
    public void AddLetter(string k){
        letters.Add(k, false);
        newletter = true;
    }

    public void playerBusy(bool b) {
        if (b) { player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition; busy = true;  }
        else   { player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation; busy = false; }
    }

    public bool isBusy () { return busy; }
    public void setBusy (bool b) { busy = b; }

    public void ClearLetters(){
        letters = new Dictionary<string, bool>();

    }
    IEnumerator ColorLerpFunction(bool fadeout, float duration)
    {
        float time = 0;
        raiseLower = true;
        Image curtainImg = curtain.GetComponent<Image>();
        Color startValue;
        Color endValue;
        if (fadeout) {
            startValue = new Color(0, 0, 0, 0);
            endValue = new Color(0, 0, 0, 1);
        } else {
            startValue = new Color(0, 0, 0, 1);
            endValue = new Color(0, 0, 0, 0);
        }
        while (time < duration)
        {
            curtainImg.color = Color.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        curtainImg.color = endValue;
        raiseLower = false;
    }
     IEnumerator LoadYourAsyncScene(string scene)
    {
        StartCoroutine(ColorLerpFunction(true, 1));
        while (raiseLower)
        {
            yield return null;
        }
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

    while(!asyncLoad.isDone)
    {
        yield return null;
    }
    
    StartCoroutine(ColorLerpFunction(false, 1));
    
    }

    public void ChangeScene(string scene){
        StartCoroutine(LoadYourAsyncScene(scene));
        ClearLetters();
    }
    public void DialogShow(string text) {
        dialogBox.SetActive(true);
        setBusy(true);
        StopAllCoroutines();
        StartCoroutine(TypeText(text));
    }
    IEnumerator TypeText(string text) {
        dialogText.text = "";
        foreach(char c in text.ToCharArray()) {
            dialogText.text += c;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void DialogHide(){
        dialogBox.SetActive(false);
        setBusy(false);
        Arrow arrow = player.transform.GetChild(0).GetComponent<Arrow>();
        arrow.itsMouseExit();
    }

    public void ShowButtons(){
        canvas.transform.Find("BackpackButton").gameObject.SetActive(true);
        canvas.transform.Find("MapButton").gameObject.SetActive(true);
    }
    public void HideButtons(){
        
        canvas.transform.Find("BackpackButton").gameObject.SetActive(false);
        canvas.transform.Find("MapButton").gameObject.SetActive(false);
    }

    public void ToCutscene(string[] dialog, string name, Sprite portrait){
        CutSceneDialog dialogscript = dialogBox.GetComponent<CutSceneDialog>();
         dialogscript.lines = dialog;
         dialogBox.SetActive(true);
         DialogShow(dialog[0]);
         dialogscript.StartCutscene(name, portrait);
         gamePaused = true;
         if(!(SceneManager.GetActiveScene().name == "Cutscene")){
         HideButtons();     
         }
         
    //         Time.timeScale = 0f;

    }

    public void PauseGame(){
        gamePaused = true;
        Time.timeScale = 0f;
        
    }
    public void UnpauseGame(){
        gamePaused = false;
        Time.timeScale = 1f;

    }
    public void EndCutscene(){
        // if the scenes name is cutscene, then transition to the Season2 scene here
        if (SceneManager.GetActiveScene().name == "Cutscene") { ChangeScene(nextscene); }
        else{ShowButtons();}
        DialogHide();
        gamePaused = false;
        ShowButtons();
        if (newletter == true){
            GameObject bpb = canvas.transform.Find("BackpackButton").gameObject;
            bpb.GetComponent<BackpackLetterPopup>().ShowNewLetter();
            newletter = false;
        }
    //        Time.timeScale = 1f;

    }

    public bool IsPaused(){
        return gamePaused;
    }
    public Dictionary<string, bool> GetLetters(){
        return letters;
    }

    public int GetTentscene(){
        return tentscene;
    }
    public void StartTentscene(int c, String next){
        tentscene = c;
        ChangeScene("Cutscene");
        nextscene = next;

    }
    void Start()
    {
        busy = false;
        letters = new Dictionary<string, bool>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
