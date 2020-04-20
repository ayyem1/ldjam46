using UnityEngine;

/// <summary>
/// Pulls an object along the X-Axis towards the gameobject
/// this script is attached to.
/// </summary>
public class Pull : MonoBehaviour
{
    [SerializeField] private GameObject objectToPull = null;
    [SerializeField] [Range(0f, 100f)] private float maxHorizontalPullSpeed = 3f;
    [SerializeField] [Range(0f, 100f)] private float maxVerticalPullSpeed = 1f;
    [SerializeField] [Range(0f, 100f)] private float maxSpecatingPullSpeed = 10f;
    private IPullable pullable = null;
    private Bounds pullSourceBounds;
    private bool isPullForcePaused = false;
    private bool isSpectatingForceApplied = false;

    private void Awake()
    {
        Waypoint.OnWaypointEntered += PausePullForce;
        Waypoint.OnWayPointExited += ResumePullForce;

        PlayerManager.OnPlayerHealthDepleted += PausePullForce;
        PlayerManager.OnPlayerSpectating += StartSpectatingPullForce;
        PlayerManager.OnPlayerHealthRestoredFromEmpty += ResumePullForce;

        EndGameSequence.OnEndGameSequenceStarted += PausePullForce;
    }

    private void PausePullForce()
    {
        isPullForcePaused = true;
    }

    private void ResumePullForce()
    {
        isPullForcePaused = false;
        isSpectatingForceApplied = false;
    }

    private void StartSpectatingPullForce()
    {
        isSpectatingForceApplied = true;
        isPullForcePaused = false;
    }


    private void Start()
    {
        pullable = objectToPull.GetComponent<IPullable>();

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        pullSourceBounds = renderer.bounds;
    }

    private void Update()
    {
        if (isPullForcePaused)
        {
            pullable.SetPullVelocity(Vector3.zero);
            return;
        }

        if (IsObjectInPullSource())
        {
            pullable.SetPullVelocity(Vector3.forward * maxVerticalPullSpeed);
        }
        else
        {
            Vector3 pullDirection = transform.position.x > objectToPull.transform.position.x ? Vector3.right : Vector3.left;
            if (isSpectatingForceApplied)
            {
                pullable.SetPullVelocity(pullDirection * maxSpecatingPullSpeed);
            }
            else
            {
                pullable.SetPullVelocity(pullDirection * maxHorizontalPullSpeed);
            }
        }
    }

    private bool IsObjectInPullSource()
    {
        float objectToPullX = objectToPull.transform.position.x;
        bool isGreaterThanMin = objectToPullX > pullSourceBounds.min.x;
        bool isLessThanMax = objectToPullX < pullSourceBounds.max.x;
        return isGreaterThanMin && isLessThanMax;
    }

    private void OnDestroy()
    {
        Waypoint.OnWaypointEntered -= PausePullForce;
        Waypoint.OnWayPointExited -= ResumePullForce;

        PlayerManager.OnPlayerHealthDepleted -= PausePullForce;
        PlayerManager.OnPlayerSpectating -= StartSpectatingPullForce;
        PlayerManager.OnPlayerHealthRestoredFromEmpty -= ResumePullForce;

        EndGameSequence.OnEndGameSequenceStarted -= PausePullForce;

    }
}