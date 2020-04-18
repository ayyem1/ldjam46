using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float maxSpeed = 10f;
    private float minMagnitude = 0.5f;
    public Vector3 playerVelocity;
    public float decelerationRate = 0.01f;
    public bool disableClick = false;

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
                Vector3 velocityVector = hit.point - this.transform.position;

                if (velocityVector.magnitude > this.minMagnitude)
                {
                    if (velocityVector.magnitude > this.maxSpeed)
                    {
                        velocityVector = velocityVector.normalized * this.maxSpeed;
                    }

                    Vector3 accelerationVector = velocityVector.normalized * this.maxSpeed;
                    velocityVector += accelerationVector * Time.deltaTime;

                    this.playerVelocity = velocityVector * Time.deltaTime;

                    this.transform.localPosition += playerVelocity;
                }
            }
        }
    }
}
