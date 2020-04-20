using System;
using UnityEngine;
using UnityEditor;

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
    private bool isPlayerInRange = false;

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
        isPlayerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        OnWayPointExited?.Invoke();
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
        dialogTextMesh.gameObject.SetActive(true);
        // TODO: Play Effects.
    }
}
