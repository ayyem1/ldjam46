using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameSequence : MonoBehaviour
{
    [SerializeField]
    private FadeTransition fader;

    [SerializeField]
    private Button startButton;

    private void Start()
    {
        this.fader.FadeFromBlack();   
    }

    public void StartGame()
    {
        this.fader.FadeToBlack();
        this.fader.fadeToBlackComplete += this.TransitionToGameScreen;
        this.startButton.interactable = false;
    }

    private void TransitionToGameScreen()
    {
        SceneManager.LoadScene("MainScene");
    }
}
