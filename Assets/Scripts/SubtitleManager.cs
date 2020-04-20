using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager instance;

    private float totalDisplayTime = 5f;
    private float fadeRate = 0.5f;

    [SerializeField]
    private Text messageText;
    [SerializeField]
    private Text titleText;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        this.DisplayTitle();
    }

    public void DisplaySubtitle(string subtitleText)
    {
        this.messageText.text = subtitleText;

        StartCoroutine(this.CycleFade(this.messageText));
    }

    public void DisplayTitle()
    {
        StartCoroutine(this.CycleFade(this.titleText));
    }

    private IEnumerator CycleFade(Text textObject)
    {
        for (float i = 0; i < 1; i += Time.fixedDeltaTime * this.fadeRate)
        {
            textObject.color = new Color(textObject.color.r, textObject.color.b, textObject.color.g, i);
            yield return null;
        }
        
        yield return new WaitForSeconds(this.totalDisplayTime);

        for (float i = 1; i >= 0; i -= Time.fixedDeltaTime * this.fadeRate)
        {
            textObject.color = new Color(textObject.color.r, textObject.color.b, textObject.color.g, i);
            yield return null;
        }
    }
}
