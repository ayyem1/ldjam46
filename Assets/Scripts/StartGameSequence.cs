using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameSequence : MonoBehaviour
{
    [SerializeField]
    private FadeTransition fader;

    [SerializeField]
    private Button startButton;

    //oh ho ho hO HO HO HO HO HO!!!!
    //I see I've piqued your curiousity as to the absolute tomfoolery I have
    //done to make this work!  BEHOLD MY GENIUS!!!
    private void Start()
    {
        this.fader.FadeToBlack();   
    }

    public void StartGame()
    {
        this.fader.FadeFromBlack();
        this.fader.fadeFromBlackComplete += this.TransitionToGameScreen;
        this.startButton.interactable = false;
    }

    private void TransitionToGameScreen()
    {
        SceneManager.LoadScene("MainScene");
    }
}
