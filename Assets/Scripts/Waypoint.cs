using UnityEngine;

/// <summary>
/// This script will display text and effects when
/// a player is within range and interacts with it.
/// </summary>
public class Waypoint : MonoBehaviour
{
    public bool WasInteractedWith { get; private set; }

    private bool isPlayerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        isPlayerInRange = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        isPlayerInRange = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && isPlayerInRange && !WasInteractedWith)
        {
            Interact();
        }
    }

    public void Interact()
    {
        // Display text.
        // Play effects.
    }
}
