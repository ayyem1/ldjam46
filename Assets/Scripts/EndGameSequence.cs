using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameSequence : MonoBehaviour
{
    [SerializeField]
    private GameObject endGameCameraObject = null;
    [SerializeField]
    private FadeTransition fader = null;

    private bool showingEndGame = false;

    // Update is called once per frame
    void Update()
    {
        if (this.showingEndGame && Input.GetMouseButtonDown(0))
        {
            this.showingEndGame = false;

            fader.FadeToBlack();
            fader.fadeToBlackComplete += this.RestartGame;
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
        fader.FadeToBlack();
        fader.fadeToBlackComplete += this.TransitionToBirdsEyeView;
    }

    private void TransitionToBirdsEyeView()
    {
        fader.fadeToBlackComplete -= this.TransitionToBirdsEyeView;
        this.endGameCameraObject.SetActive(true);
        fader.FadeFromBlack();
        fader.fadeFromBlackComplete += this.FinalizeEndgameState;
    }

    private void FinalizeEndgameState()
    {
        fader.fadeFromBlackComplete -= this.FinalizeEndgameState;
        this.showingEndGame = true;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("StartingScene");
    }
}
