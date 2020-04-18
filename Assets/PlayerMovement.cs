using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float maxSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        //CameraFollow.instance.SetPointOfInterest(this.gameObject);
    }

    private Vector3 GetMovementDirectionVector()
    {
        Vector3 moveDirection = Camera.main.ScreenToViewportPoint(Input.mousePosition) - Camera.main.WorldToViewportPoint(this.gameObject.transform.position);
        Debug.LogError("Mouse Vector: " + Camera.main.ScreenToViewportPoint(Input.mousePosition) + " ObjectPosition: " + moveDirection);
        return moveDirection;
    }

    private float GetRotationAngle(Vector3 mousePoint)
    {
        float numerator = Vector3.Dot(this.transform.up - this.transform.position, mousePoint - this.transform.position);
        float denominator = (this.transform.up - this.transform.position).magnitude * (mousePoint - this.transform.position).magnitude;
        return Mathf.Acos(numerator/denominator);
    }

    // Update is called once per frame
    void Update()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit))
        {
            Vector3 movementVector = hit.point - this.transform.position;
            this.transform.Rotate(this.transform.forward, this.GetRotationAngle(hit.point));
            if (movementVector.magnitude > 0.1f)
            {
                this.transform.Translate(movementVector.normalized * this.maxSpeed * Time.deltaTime);
            }
        }
    }
}
