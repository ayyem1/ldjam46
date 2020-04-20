using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PulseObject : MonoBehaviour
{

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private float fadeSpeed = 1f;

    private float visibleDuration = 10f;
    private float delay = 3f;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Pulse());
    }

    // Update is called once per frame
    private IEnumerator Pulse()
    {
        yield return new WaitForSeconds(this.delay);

        float alphaValue = 0.0f;

        float finalValue = Mathf.Sin(alphaValue);

        for (float i = 0; i < this.visibleDuration; i += Time.deltaTime)
        {
            finalValue = Mathf.Sin(alphaValue);

            if (finalValue < 0)
            {
                finalValue = -finalValue;
            }
            this.spriteRenderer.color = new Color(this.spriteRenderer.color.r, this.spriteRenderer.color.b, this.spriteRenderer.color.g, finalValue);
            alphaValue += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        while (this.spriteRenderer.color.a > 0)
        {
            this.spriteRenderer.color = Color.Lerp(this.spriteRenderer.color, new Color(this.spriteRenderer.color.r, this.spriteRenderer.color.b, this.spriteRenderer.color.g, 0), Time.deltaTime);
            yield return null;
        }

        this.spriteRenderer.color = new Color(this.spriteRenderer.color.r, this.spriteRenderer.color.b, this.spriteRenderer.color.g, 0);

    }
}