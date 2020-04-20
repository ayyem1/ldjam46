using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager instance;

    private float totalDisplayTime = 5f;
    private float fadeRate = 0.5f;

    [SerializeField]
    private TextMeshProUGUI messageMesh;
    [SerializeField]
    private TextMeshProUGUI titleMesh;

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
        this.messageMesh.text = subtitleText;

        StartCoroutine(this.CycleFade(this.messageMesh));
    }

    public void DisplayTitle()
    {
        StartCoroutine(this.CycleFade(this.titleMesh));
    }

    private IEnumerator CycleFade(TextMeshProUGUI textMesh)
    {
        for (float i = 0; i < 1; i += Time.fixedDeltaTime * this.fadeRate)
        {
            textMesh.color = new Color(textMesh.color.r, textMesh.color.b, textMesh.color.g, i);
            yield return null;
        }
        
        yield return new WaitForSeconds(this.totalDisplayTime);

        for (float i = 1; i >= 0; i -= Time.fixedDeltaTime * this.fadeRate)
        {
            textMesh.color = new Color(textMesh.color.r, textMesh.color.b, textMesh.color.g, i);
            yield return null;
        }
    }
}
