using UnityEngine;

/// <summary>
/// Pulls an object along the X-Axis towards the gameobject
/// this script is attached to.
/// </summary>
public class Pull : MonoBehaviour
{
    [SerializeField] private GameObject objectToPull = null;
    [SerializeField] [Range(0f, 100f)] private float maxPullSpeed = 10f;

    private Bounds pullSourceBounds = new Bounds();

    private void Start()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        pullSourceBounds = renderer.bounds;
    }

    private void Update()
    {
        if (isObjectInPullSource())
        {
            return;
        }

        Vector3 pullDirection = transform.position.x > objectToPull.transform.position.x ? Vector3.right : Vector3.left;
        Vector3 desiredPullVelocity = pullDirection * maxPullSpeed;

        objectToPull.transform.Translate(Time.deltaTime * desiredPullVelocity.x, 0f, 0f, Space.World);
    }

    private bool isObjectInPullSource()
    {
        float objectToPullX = objectToPull.transform.position.x;
        bool isGreaterThanMin = objectToPullX > pullSourceBounds.min.x;
        bool isLessThanMax = objectToPullX < pullSourceBounds.max.x;
        return isGreaterThanMin && isLessThanMax;
    }
}