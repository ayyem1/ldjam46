using System;
using UnityEngine;

/// <summary>
/// This script will display text and effects when
/// a player is within range and interacts with it.
/// </summary>
public class Waypoint : MonoBehaviour
{
    public static Action OnWaypointEntered;
    public static Action OnWayPointExited;

    public bool WasInteractedWith { get; private set; }

    [SerializeField] private WaypointInfo waypointInfo = null;
    [SerializeField] private TextMesh dialogTextMesh = null;
    [SerializeField] private TextMesh waypointName = null;
    private bool isPlayerInRange = false;

    private void Start()
    {
        dialogTextMesh.text = waypointInfo.dialog;
        waypointName.text = waypointInfo.waypointName;
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
