using UnityEngine;

public class EnterGameSequence : MonoBehaviour
{
    [SerializeField] private FadeTransition fader = null;

    void Start()
    {
        fader.FadeFromBlack();
    }
}
