using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OBSOLETE!!!!! WE'RE USING TRAILRENDERER NOW
/// (Keeping this just in case we encounter some hiccups...)
/// </summary>

public class PlayerMovement : MonoBehaviour
{
    private float maxSpeed = 10f;
    private float minMagnitude = 0.5f;
    public float moveSpeed;
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
                Vector3 movementVector = hit.point - this.transform.position;
                this.transform.LookAt(hit.point, Vector3.up);
                if (movementVector.magnitude > this.minMagnitude)
                {
                    this.moveSpeed = movementVector.magnitude;
                    if (this.moveSpeed > this.maxSpeed)
                    {
                        this.moveSpeed = this.maxSpeed;
                    }

                    Debug.LogError("Move Speed: " + this.moveSpeed);

                    this.moveSpeed = this.moveSpeed * Time.deltaTime;

                    this.transform.Translate(this.transform.forward * this.moveSpeed, Space.World);
                }
            }
        }
        else
        {
            StartCoroutine(this.DeceleratePlayer());
        }
    }

    private IEnumerator DeceleratePlayer()
    {
        while (Input.GetMouseButton(0) == false && this.moveSpeed > 0.01f)
        {
            this.moveSpeed -= Mathf.Lerp(this.moveSpeed, 0, this.decelerationRate);
            Debug.LogError("Decelerating to new move speed: " + this.moveSpeed);
            this.transform.Translate(this.transform.forward * this.moveSpeed, Space.World);
            yield return null;
        }

        this.moveSpeed = 0;
    }
}
