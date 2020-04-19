using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PulseObject : MonoBehaviour
{

    [SerializeField]
    private Text uiText;

    private float fadeSpeed = 1f;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Pulse());
    }

    // Update is called once per frame
    private IEnumerator Pulse()
    {
        float alphaValue = 1.0f;

        float finalValue = Mathf.Sin(alphaValue);

        while (true)
        {
            finalValue = Mathf.Sin(alphaValue);

            if (finalValue < 0)
            {
                finalValue = -finalValue;
            }
            this.uiText.color = new Color(this.uiText.color.r, this.uiText.color.b, this.uiText.color.g, finalValue);
            alphaValue += Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }
}