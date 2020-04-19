using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameSequence : MonoBehaviour
{
    [SerializeField]
    private FadeTransition fader;
    private bool gameStarted = false;

    private void Start()
    {
        this.fader.FadeFromBlack();   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && this.gameStarted == false)
        {
            this.fader.FadeToBlack();
            this.fader.fadeToBlackComplete += this.TransitionToGameScreen;
            this.gameStarted = true;
        }
    }

    private void TransitionToGameScreen()
    {
        SceneManager.LoadScene("MainScene");
    }
}
