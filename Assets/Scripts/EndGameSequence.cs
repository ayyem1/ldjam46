using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameSequence : MonoBehaviour
{
    public static Action OnEndGameSequenceStarted;

    [SerializeField]
    private GameObject endGameCameraObject = null;
    [SerializeField]
    private FadeTransition fader = null;

    [SerializeField]
    private Terrain[] terrains;
    [SerializeField]
    private Material finishMaterial;

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
        OnEndGameSequenceStarted?.Invoke();
        fader.FadeToBlack();
        fader.fadeToBlackComplete += this.TransitionToBirdsEyeView;
    }

    private void TransitionToBirdsEyeView()
    {
        fader.fadeToBlackComplete -= this.TransitionToBirdsEyeView;

        foreach (Terrain curTerrain in this.terrains)
        {
            curTerrain.materialTemplate = this.finishMaterial;
        }

        this.endGameCameraObject.SetActive(true);
        fader.FadeFromBlack();
        fader.fadeFromBlackComplete += this.FinalizeEndgameState;
    }

    private void FinalizeEndgameState()
    {
        fader.fadeFromBlackComplete -= this.FinalizeEndgameState;
        this.showingEndGame = true;

        SubtitleManager.instance.DisplayEndingText();
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("StartingScene");
    }
}
