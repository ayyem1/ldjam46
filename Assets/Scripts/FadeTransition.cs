using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{
    [SerializeField]
    private Image fadePanel; 

    public void FadeIn()
    {

    }

    public void FadeOut()
    {

    }

    private IEnumerator DoFade()
    {
        for (float i = 1; i > 0; i -= Time.deltaTime * 3.0f)
        {
            this.fadePanel.color = new Color(this.fadePanel.color.r, this.fadePanel.color.b, this.fadePanel.color.g, i);
            yield return null;
        }

        yield return new WaitForSeconds(3.0f);

        for (float i = 0; i < 1; i += Time.deltaTime * 3.0f)
        {
            this.fadePanel.color = new Color(this.fadePanel.color.r, this.fadePanel.color.b, this.fadePanel.color.g, i);
            yield return null;
        }

        this.fadePanel.color = Color.black;
    }
}
