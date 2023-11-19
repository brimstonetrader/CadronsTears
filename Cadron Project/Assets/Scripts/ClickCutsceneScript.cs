using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCutsceneScript : MonoBehaviour
{
    public string charname;
    public string[] dialogLines;
    public LetterDialog letterdialog;
    public Sprite portrait;
    public Texture2D cursortexture;
    public Arrow arrow;

    public void OnMouseEnter(){
        if(GameManager.Instance.IsPaused() == false){
            arrow.Visible(false);
            Cursor.SetCursor(cursortexture, Vector2.zero, CursorMode.Auto);
        }
    }
    public void OnMouseDown(){
        if(GameManager.Instance.IsPaused() == false){
           arrow.Remove(charname);
           arrow.Visible(false);
           Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
           letterdialog.SetDialog();
           GameManager.Instance.ToCutscene(dialogLines, charname, portrait);
           letterdialog.UpdateLetter();
        }
        
    }

    public void OnMouseExit(){
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        arrow.Visible(true);
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