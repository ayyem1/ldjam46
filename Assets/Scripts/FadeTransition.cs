using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{
    [SerializeField]
    private Image fadePanel;

    public delegate void FadeComplete();
    public event FadeComplete fadeToBlackComplete;
    public event FadeComplete fadeFromBlackComplete;

    public void FadeToBlack()
    {
        StartCoroutine(this.FadePanelIn());
    }

    public void FadeFromBlack()
    {
        StartCoroutine(this.FadePanelOut());
    }

    private IEnumerator FadePanelIn()
    {
        for (float i = 0; i < 1; i += Time.deltaTime * 3.0f)
        {
            this.fadePanel.color = new Color(this.fadePanel.color.r, this.fadePanel.color.b, this.fadePanel.color.g, i);
            yield return null;
        }

        this.fadePanel.color = Color.black;

        if (this.fadeToBlackComplete != null)
        {
            this.fadeToBlackComplete();
        }
    }

    private IEnumerator FadePanelOut()
    {
        for (float i = 1; i > 0; i -= Time.deltaTime * 3.0f)
        {
            this.fadePanel.color = new Color(this.fadePanel.color.r, this.fadePanel.color.b, this.fadePanel.color.g, i);
            yield return null;
        }

        if (this.fadeFromBlackComplete != null)
        {
            this.fadeFromBlackComplete();
        }
    }
}
