using System;
using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// This script will display text and effects when
/// a player is within range and interacts with it.
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
    [SerializeField] public GameObject tooltipIcon = null;
    [SerializeField] private GameObject parentGameObject = null;

    private bool isPlayerInRange = false;
    private bool isDisabled = false;

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
        if (isDisabled || other.gameObject.tag != "Player")
            return;

        OnWaypointEntered?.Invoke();
        tooltipIcon.SetActive(!WasInteractedWith);
        isPlayerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (isDisabled || other.gameObject.tag != "Player")
            return;

        OnWayPointExited?.Invoke();
        tooltipIcon.SetActive(false);
        isPlayerInRange = false;
    }

    private void Update()
    {
        if (!isDisabled && Input.GetMouseButtonDown(1) && isPlayerInRange && !WasInteractedWith)
        {
            WasInteractedWith = true;
            tooltipIcon.SetActive(false);
            Interact();
        }
    }

    public void Interact()
    {
        StartCoroutine(DisplayEffects());
    }

    private IEnumerator DisplayEffects()
    {
        displayBeforeInteraction.SetActive(false);
        waypointSprite.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        SubtitleManager.instance.DisplaySubtitle(this.waypointInfo.dialog);
    }
}
