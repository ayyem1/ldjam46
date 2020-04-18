using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Pulls an object along the X-Axis towards the gameobject
/// this script is attached to.
/// </summary>
public class Pull : MonoBehaviour
{
    [SerializeField] private GameObject objectToPull = null;
    [SerializeField] [Range(0f, 100f)] private float maxPullSpeed = 10f;
    [SerializeField] [Range(0f, 100f)] private float maxAcceleration = 10f;

    // Based on how far the player is from the path.
    private Vector3 desiredPullVelocity = Vector3.zero;
    private Vector3 currentPullVelocity = Vector3.zero;

    private void Update()
    {
        Vector3 objectToPullPosition = objectToPull.transform.position;
        // We'll pull along the X-axis so we'll keep the y and z of
        // the object that is being pulled.
        Vector3 pullTowardsPosition = new Vector3(transform.position.x, objectToPullPosition.y, objectToPullPosition.z);
        
        if (Mathf.Abs(pullTowardsPosition.x - objectToPullPosition.x) > Mathf.Epsilon)
        {
            desiredPullVelocity = (pullTowardsPosition - objectToPullPosition).normalized * maxPullSpeed;
        }
        else
        {
            // In our case, we don't want the player to be pulled past
            // the pull source. IRL, this would happen due to momentum.
            currentPullVelocity = Vector3.zero;
            desiredPullVelocity = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        float maxSpeedChange = maxAcceleration * Time.deltaTime;
        currentPullVelocity.x = Mathf.MoveTowards(currentPullVelocity.x, desiredPullVelocity.x, maxSpeedChange);

        objectToPull.transform.Translate(Time.deltaTime * currentPullVelocity.x, 0f, 0f);
    }
}