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

    private Bounds pullSourceBounds = new Bounds();

    private void Start()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        pullSourceBounds = renderer.bounds;
    }

    private void Update()
    {
        if (IsObjectInPullSource())
        {
            if (!Input.anyKey)
            {
                objectToPull.transform.Translate(0f, 0f, Time.deltaTime * maxVerticalPullSpeed, Space.World);
            }
            return;
        }

        Vector3 pullDirection = transform.position.x > objectToPull.transform.position.x ? Vector3.right : Vector3.left;
        Vector3 desiredPullVelocity = pullDirection * maxHorizontalPullSpeed;

        objectToPull.transform.Translate(Time.deltaTime * desiredPullVelocity.x, 0f, 0f, Space.World);
    }

    private bool IsObjectInPullSource()
    {
        float objectToPullX = objectToPull.transform.position.x;
        bool isGreaterThanMin = objectToPullX > pullSourceBounds.min.x;
        bool isLessThanMax = objectToPullX < pullSourceBounds.max.x;
        return isGreaterThanMin && isLessThanMax;
    }
}