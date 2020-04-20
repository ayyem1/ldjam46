using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeToQuit : MonoBehaviour
{
    [SerializeField]
    private FadeTransition fader;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (this.fader != null)
            {
                this.fader.FadeToBlack();
                this.fader.fadeToBlackComplete += this.CloseGame;
            }
            else
            {
                Application.Quit();
            }
        }
    }

    private void CloseGame()
    {
        Application.Quit();
    }
}