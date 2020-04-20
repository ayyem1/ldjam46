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

    Coroutine currentlyDisplayingSubtitle;

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
        if (this.currentlyDisplayingSubtitle == null)
        {
            this.messageText.text = subtitleText;
            this.currentlyDisplayingSubtitle = StartCoroutine(this.CycleFade(this.messageText));
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(this.StartNewSubtitle(subtitleText));
        }
    }

    public void DisplayTitle()
    {
        StartCoroutine(this.CycleFade(this.titleText));
    }

    private IEnumerator CycleFade(Text textObject)
    {
        for (float i = 0; i < 1; i += Time.deltaTime * this.fadeRate)
        {
            textObject.color = new Color(textObject.color.r, textObject.color.b, textObject.color.g, i);
            yield return null;
        }

        for (float i = 0; i < this.totalDisplayTime; i += Time.deltaTime)
        {
            yield return null;
        }
        
        for (float i = 1; i >= 0; i -= Time.fixedDeltaTime * this.fadeRate)
        {
            textObject.color = new Color(textObject.color.r, textObject.color.b, textObject.color.g, i);
            yield return null;
        }

        this.currentlyDisplayingSubtitle = null;
    }

    private IEnumerator StartNewSubtitle(string newSubtitle)
    {
        Debug.LogError("I'm here!!");

        for (float i = this.messageText.color.a; i >= 0; i -= Time.deltaTime * 3.0f)
        {
            this.messageText.color = new Color(messageText.color.r, messageText.color.b, messageText.color.g, i);
            yield return null;
        }

        this.messageText.text = newSubtitle;
        this.currentlyDisplayingSubtitle = StartCoroutine(this.CycleFade(this.messageText));
    }
}
