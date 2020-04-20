using System;
using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// This script will display text and effects when
/// a player is within range and interacts with it.
/// NOTE: We don't disable the waypoints when a player
/// is 'spectating' back to the main path. We depend
/// on the player's collider being turned off.
/// </summary>
public class Waypoint : MonoBehaviour
{
    public static Action OnWaypointEntered;
    public static Action OnWayPointExited;

    public bool WasInteractedWith { get; private set; }

    [SerializeField] public WaypointInfo waypointInfo = null;
    [SerializeField] public TextMesh dialogTextMesh = null;
    [SerializeField] public TextMesh waypointName = null;
    [SerializeField] public SpriteRenderer waypointSprite = null;
    [SerializeField] public GameObject displayBeforeInteraction = null;
    [SerializeField] private SpriteRenderer tooltipIcon = null;
    [SerializeField] private GameObject parentGameObject = null;
    [SerializeField] private float fadeTime = 1f;

    private bool isPlayerInRange = false;

    private IEnumerator tooltipFadeCoroutine = null;
    private IEnumerator displayEffectsCoroutine = null;

    [SerializeField]
    AudioSource audio;

    private void Awake()
    {
        EndGameSequence.OnEndGameSequenceStarted += DisableWaypointIfNotInteractedWith;
    }

    private void DisableWaypointIfNotInteractedWith()
    {
        if (WasInteractedWith)
        {
            parentGameObject.SetActive(false);
        }
    }

    private void Start()
    {
        dialogTextMesh.text = waypointInfo.dialog;
        waypointName.text = waypointInfo.waypointName;
        waypointSprite.sprite = waypointInfo.waypointImage;
        dialogTextMesh.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        OnWaypointEntered?.Invoke();
        if (!WasInteractedWith)
        {
            if (tooltipFadeCoroutine != null)
            {
                StopCoroutine(tooltipFadeCoroutine);
                tooltipFadeCoroutine = null;
            }

            tooltipFadeCoroutine = FadeSpriteAlphaIn(tooltipIcon, 1f);
            StartCoroutine(tooltipFadeCoroutine);
        }

        isPlayerInRange = true;
    }

    private IEnumerator FadeSpriteAlphaIn(SpriteRenderer sprite, float alphaValue)
    {
        sprite.gameObject.SetActive(true);
        Color color = sprite.color;
        alphaValue = Mathf.Min(1f, alphaValue);
        while (sprite.color.a < alphaValue)
        {
            color.a += Time.deltaTime / fadeTime;
            if (color.a > 1f)
                color.a = 1f;

            sprite.color = color;
            yield return null;
        }

        sprite.color = color;
    }

    private IEnumerator FadeSpriteAlphaOut(SpriteRenderer sprite, float alphaValue)
    {
        Color color = sprite.color;
        float step = Time.deltaTime / fadeTime;
        alphaValue = Mathf.Max(0f, alphaValue);
        while (color.a > alphaValue)
        {
            color.a -= Time.deltaTime / fadeTime;
            if (color.a < 0f)
                color.a = 0f;

            sprite.color = color;
            yield return null;
        }

        sprite.color = color;
        sprite.gameObject.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        OnWayPointExited?.Invoke();

        if (tooltipFadeCoroutine != null)
        {
            StopCoroutine(tooltipFadeCoroutine);
            tooltipFadeCoroutine = null;
        }

        tooltipFadeCoroutine = FadeSpriteAlphaOut(tooltipIcon, 0f);
        StartCoroutine(tooltipFadeCoroutine);

        isPlayerInRange = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && isPlayerInRange && !WasInteractedWith)
        {
            WasInteractedWith = true;
            Interact();
        }
    }

    public void Interact()
    {
        StartCoroutine(DisplayEffects());

        float pitchShift = UnityEngine.Random.Range(-0.5f, 0.5f);
        this.audio.pitch += pitchShift;
        this.audio.Play();

        if (tooltipFadeCoroutine != null)
        {
            StopCoroutine(tooltipFadeCoroutine);
            tooltipFadeCoroutine = null;
        }

        // Reset player alpha.
        Color c = tooltipIcon.color;
        c.a = 0f;
        tooltipIcon.color = c;
        tooltipIcon.gameObject.SetActive(false);
    }

    private IEnumerator DisplayEffects()
    {
        displayBeforeInteraction.SetActive(false);
        waypointSprite.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        SubtitleManager.instance.DisplaySubtitle(waypointInfo.dialog);
    }
}
