using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0, 100)]
    private float maxSpeed = 10f;
    [SerializeField, Range(0, 100)]
    private float maxAcceleration = 30f;
    [SerializeField, Range(0, 100)]
    private float maxDeceleration = 10f;
    public Vector3 playerVelocity;
    public bool disableClick = false;

    private Vector3 accelerationVector;

    private void AccelerateTowardsDesiredVelocity(Vector3 desiredVelocity, float maxSpeedChange)
    {
        this.playerVelocity.x = Mathf.MoveTowards(playerVelocity.x, desiredVelocity.x, maxSpeedChange);
        this.playerVelocity.z = Mathf.MoveTowards(playerVelocity.z, desiredVelocity.z, maxSpeedChange);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            this.disableClick = !this.disableClick;
        }

        if (Input.GetMouseButton(0) || this.disableClick)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(mouseRay, out hit))
            {
                Vector3 desiredDirection = hit.point - this.transform.position;
                Vector3 desiredVelocity = desiredDirection.normalized * maxSpeed;
                float maxSpeedChange = this.maxAcceleration * Time.deltaTime;

                this.AccelerateTowardsDesiredVelocity(desiredVelocity, maxSpeedChange);

                this.transform.localPosition += this.playerVelocity * Time.deltaTime;
            }
        }
        else if (this.playerVelocity.magnitude > 0)
        {
            float maxSpeedChange = this.maxDeceleration * Time.deltaTime;

            this.AccelerateTowardsDesiredVelocity(Vector3.zero, maxSpeedChange);
            if (this.playerVelocity.magnitude < 0)
            {
                this.playerVelocity = Vector3.zero;
            }
            this.transform.localPosition += this.playerVelocity * Time.deltaTime;
        }
    }
}
