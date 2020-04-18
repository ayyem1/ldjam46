using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float maxSpeed = 10f;
    private float minMagnitude = 0.5f;
    public float moveSpeed;

    void Update()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit))
        {
            Vector3 movementVector = hit.point - this.transform.position;
            this.transform.LookAt(hit.point, Vector3.forward);
            if (movementVector.magnitude > this.minMagnitude)
            {
                this.moveSpeed = movementVector.magnitude;
                if (this.moveSpeed > this.maxSpeed)
                {
                    this.moveSpeed = this.maxSpeed;
                }

                this.moveSpeed = this.moveSpeed * Time.deltaTime;

                this.transform.Translate(this.transform.up * this.moveSpeed);
            }
        }
    }
}
