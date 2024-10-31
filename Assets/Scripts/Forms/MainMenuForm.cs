using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuForm : BaseForm, IForm
{
    
    public void OnActive()
    {
        gameObject.SetActive(true);
    }

    public void OnDisable()
    {
        gameObject.SetActive(false);
    }

    public void OnNewGamePressed()
    {
        
    }

    public void OnJoinGamePressed()
    {
        
    }
}
