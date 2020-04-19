using UnityEngine;
using System.Collections;

public class BobPlayer : MonoBehaviour
{
    public float verticalAmplitude;
    public float verticalTime;
    public float horizontalAmplitude;
    public float horizontalTime;

    private Coroutine bobCoroutine;

    [SerializeField]
    private Transform playerLightLifeTransform;
    private Vector3 newLocalScale;
    public float playerLightAmplitude;

    // Use this for initialization
    void Start()
    {
        this.bobCoroutine = StartCoroutine(BobPlayerCoroutine());
    }

    void OnDisable()
    {
        StopCoroutine(this.bobCoroutine);
    }

    //Meant to attenuate the player's shadow, but it doesn't play nice with when the player goes above max health
    private void LateUpdate()
    {
        if (this.playerLightLifeTransform.localScale.magnitude > 0)
        {
            this.playerLightLifeTransform.localScale += this.newLocalScale;
        }
    }

    IEnumerator BobPlayerCoroutine()
    {
        float timeElapsed = 0;

        Vector3 startPos = transform.localPosition;

        while (true)
        {
            timeElapsed += Time.deltaTime;

            transform.localPosition = startPos + Vector3.up * verticalAmplitude * Mathf.Sin(2 * Mathf.PI * timeElapsed / verticalTime) +
                Vector3.right * horizontalAmplitude * Mathf.Sin(2 * Mathf.PI * timeElapsed / horizontalTime);

            this.newLocalScale = -this.playerLightLifeTransform.localScale * this.playerLightAmplitude * Mathf.Sin(2 * Mathf.PI * timeElapsed / verticalTime);

            yield return 0;
        }
    }
}