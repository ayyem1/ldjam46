using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    public PlayerMovement playerCharacter;

    private Camera thisCamera;
    private Transform cameraTransform;
    private float cameraDistance;

    private float verticalViewportThreshold = 0.3f;
    private float horizontalViewportThreshold = 0.3f;

    void Awake()
    {
        CameraFollow.instance = this;
        this.thisCamera = this.gameObject.GetComponentInChildren<Camera>();
        this.cameraTransform = this.gameObject.transform;
        this.cameraDistance = this.cameraTransform.position.y;
    }

    private bool IsPlayerPastHorizontalThreshold(float playerViewportXPosition)
    {
        return (playerViewportXPosition > (1.0f - this.horizontalViewportThreshold)) || 
            (playerViewportXPosition < (0.0f + this.horizontalViewportThreshold));
    }

    void LateUpdate()
    {
        
        Vector3 playerViewportPosition = thisCamera.WorldToViewportPoint(this.playerCharacter.gameObject.transform.position);

        if (this.playerCharacter != null && (playerViewportPosition.y > this.verticalViewportThreshold || 
            this.IsPlayerPastHorizontalThreshold(playerViewportPosition.x)))
        {
            this.UpdateCameraPosition();
        }
    }

    private void UpdateCameraPosition()
    {
        Vector3 worldSpaceCenteredPosition = this.thisCamera.ViewportToWorldPoint(new Vector3(0.5f, this.cameraDistance, this.verticalViewportThreshold));

        Vector3 shiftVector = new Vector3(this.playerCharacter.transform.position.x - worldSpaceCenteredPosition.x, 0,
            this.playerCharacter.transform.position.z - worldSpaceCenteredPosition.z);

        this.cameraTransform.Translate(shiftVector.normalized * this.playerCharacter.moveSpeed);
    }
}