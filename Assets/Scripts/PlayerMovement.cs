using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPullable
{
    [SerializeField, Range(0, 100)]
    private float maxSpeed = 10f;
    [SerializeField, Range(0, 100)]
    private float maxAcceleration = 30f;
    [SerializeField, Range(0, 100)]
    private float maxDeceleration = 10f;
    public Vector3 playerVelocity;
    public bool disableClick = false;

    private Vector3 pullVelocity;

    private void AccelerateTowardsDesiredVelocity(Vector3 desiredVelocity, float maxSpeedChange)
    {
        this.playerVelocity.x = Mathf.MoveTowards(playerVelocity.x, desiredVelocity.x, maxSpeedChange);
        this.playerVelocity.z = Mathf.MoveTowards(playerVelocity.z, desiredVelocity.z, maxSpeedChange);
    }

    void Update()
    {
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
        else if (this.playerVelocity.magnitude > 0 || this.pullVelocity.magnitude > 0)
        {
            float maxSpeedChange = this.maxDeceleration * Time.deltaTime;

            this.AccelerateTowardsDesiredVelocity(pullVelocity, maxSpeedChange);
            if (this.playerVelocity.magnitude < 0)
            {
                this.playerVelocity = Vector3.zero;
            }
            this.transform.localPosition += this.playerVelocity * Time.deltaTime;
        }
    }

    public void SetPullVelocity(Vector3 pullVelocity)
    {
        this.pullVelocity = pullVelocity;
    }
}
