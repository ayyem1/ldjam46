using UnityEngine;

/// <summary>
/// Pulls an object along the X-Axis towards the gameobject
/// this script is attached to.
/// </summary>
public class Pull : MonoBehaviour
{
    [SerializeField] private GameObject objectToPull = null;
    [SerializeField] [Range(0f, 100f)] private float maxHorizontalPullSpeed = 10f;
    [SerializeField] [Range(0f, 100f)] private float maxVerticalPullSpeed = 1f;

    private IPullable pullable = null;
    private Bounds pullSourceBounds;
    private Vector3 horizontalPullVelocity;
    private Vector3 verticalPullVelocity;
    private Vector3 currentPullDirection;

    private void Start()
    {
        pullable = objectToPull.GetComponent<IPullable>();

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        pullSourceBounds = renderer.bounds;

        horizontalPullVelocity = Vector3.zero;
        verticalPullVelocity = new Vector3(0f, 0f, maxVerticalPullSpeed);
        currentPullDirection = Vector3.zero;
    }

    private void Update()
    {
        if (IsObjectInPullSource())
        {
            if (!Input.anyKey)
            {
                pullable.SetPullVelocity(verticalPullVelocity);
                currentPullDirection = Vector3.forward;
            }

            return;
        }

        Vector3 newPullDirection = transform.position.x > objectToPull.transform.position.x ? Vector3.right : Vector3.left;
        if (newPullDirection != currentPullDirection)
        {
            currentPullDirection = newPullDirection;
            horizontalPullVelocity.x = currentPullDirection.x * maxHorizontalPullSpeed;
            pullable.SetPullVelocity(horizontalPullVelocity);
        }
    }

    private bool IsObjectInPullSource()
    {
        float objectToPullX = objectToPull.transform.position.x;
        bool isGreaterThanMin = objectToPullX > pullSourceBounds.min.x;
        bool isLessThanMax = objectToPullX < pullSourceBounds.max.x;
        return isGreaterThanMin && isLessThanMax;
    }
}