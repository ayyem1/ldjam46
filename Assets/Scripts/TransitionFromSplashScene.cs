using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionFromSplashScene : MonoBehaviour
{
    [SerializeField]
    private float secondsToWaitBeforeTransition = 3.0f;
    [SerializeField]
    private string sceneToTransitionTo = string.Empty;

    [SerializeField]
    private FadeTransition fader;

    private void Awake()
    {
        StartCoroutine(TransitionAfterWait());
    }

    private IEnumerator TransitionAfterWait()
    {
        this.fader.FadeFromBlack();
        yield return new WaitForSeconds(secondsToWaitBeforeTransition);
        this.fader.FadeToBlack();
        this.fader.fadeToBlackComplete += this.LoadNextScene;
    }

    private void LoadNextScene()
    {
        if (sceneToTransitionTo != string.Empty)
        {
            SceneManager.LoadScene(sceneToTransitionTo);
        }
    }
}
