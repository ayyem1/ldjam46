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
    private Vector3 pullVelocity;

    public Rigidbody Body { get; private set; }

    public bool oldWayEngaged = false;

    private void Start()
    {
        Body = GetComponent<Rigidbody>();
    }

    private void AccelerateTowardsDesiredVelocity(Vector3 desiredVelocity, float maxSpeedChange)
    {
        this.playerVelocity.x = Mathf.MoveTowards(playerVelocity.x, desiredVelocity.x, maxSpeedChange);
        this.playerVelocity.z = Mathf.MoveTowards(playerVelocity.z, desiredVelocity.z, maxSpeedChange);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            oldWayEngaged = !oldWayEngaged;
        }

        if (oldWayEngaged)
        {
            this.maxAcceleration = 30f;
            this.maxDeceleration = 15f;
        }
        else
        {
            this.maxAcceleration = 100f;
            this.maxDeceleration = 30f;
        }

        if (this.oldWayEngaged == false)
        {
            playerVelocity = Body.velocity;
        }
        
        if (Input.GetMouseButton(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(mouseRay, out hit))
            {
                Vector3 desiredDirection = hit.point - this.transform.position;
                Vector3 desiredVelocity = desiredDirection.normalized * maxSpeed;
                float maxSpeedChange = this.maxAcceleration * Time.deltaTime;

                this.AccelerateTowardsDesiredVelocity(desiredVelocity, maxSpeedChange);
                if (oldWayEngaged == true)
                {
                    this.transform.localPosition += this.playerVelocity * Time.deltaTime;
                }
                
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

            if (oldWayEngaged == true)
            {
                this.transform.localPosition += this.playerVelocity * Time.deltaTime;
            }
            
        }
    }

    private void FixedUpdate()
    {
        if (oldWayEngaged == false)
        {
            Body.velocity = playerVelocity;
        }
        else
        {
            Body.velocity = Vector3.zero;
        }
        
    }

    public void SetPullVelocity(Vector3 pullVelocity)
    {
        this.pullVelocity = pullVelocity;
    }
}
