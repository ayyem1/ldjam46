using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameSequence : MonoBehaviour
{
    [SerializeField]
    private GameObject endGameCameraObject;
    private bool showingEndGame = false;

    // Update is called once per frame
    void Update()
    {
        if (this.showingEndGame && Input.GetMouseButtonDown(0))
        {
            this.showingEndGame = false;

            GameManager.instance.fader.FadeToBlack();
            GameManager.instance.fader.fadeToBlackComplete += this.RestartGame;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            this.GoToEndGameState(other.gameObject);
        }
    }

    private void GoToEndGameState(GameObject playerObject)
    {
        GameManager.instance.fader.FadeToBlack();
        GameManager.instance.fader.fadeToBlackComplete += this.TransitionToBirdsEyeView;
    }

    private void TransitionToBirdsEyeView()
    {
        GameManager.instance.fader.fadeToBlackComplete -= this.TransitionToBirdsEyeView;
        this.endGameCameraObject.SetActive(true);
        GameManager.instance.fader.FadeFromBlack();
        GameManager.instance.fader.fadeFromBlackComplete += this.FinalizeEndgameState;
    }

    private void FinalizeEndgameState()
    {
        GameManager.instance.fader.fadeFromBlackComplete -= this.FinalizeEndgameState;
        this.showingEndGame = true;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("StartingScene");
    }
}
