using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPullable
{
    [SerializeField, Range(0, 100)]
    private float maxSpeed = 10f;
    [SerializeField, Range(0, 100)]
    private float maxAcceleration = 30f;
    [SerializeField, Range(0, 100)]
    private float maxDeceleration = 10f;

    private Vector3 playerVelocity;
    private Vector3 desiredPlayerVelocity;
    private float desiredMaxSpeedChange;

    private Vector3 pullVelocity;

    private bool isMovementInputPaused;
    private Collider movementCollider = null;

    public Rigidbody Body { get; private set; }

    //public bool oldWayEngaged = false;

    private void Awake()
    {
        PlayerManager.OnPlayerHealthDepleted += PauseVelocityMovement;
        PlayerManager.OnPlayerHealthRestoredFromEmpty += ResumeVelocityMovement;
    }

    private void PauseVelocityMovement()
    {
        Body.velocity = Vector3.zero;
        Body.angularVelocity = Vector3.zero;
        movementCollider.enabled = false;

        isMovementInputPaused = true;
    }
    
    private void ResumeVelocityMovement()
    {
        movementCollider.enabled = true;
        isMovementInputPaused = false;
    }

    private void OnDestroy()
    {
        PlayerManager.OnPlayerHealthDepleted -= PauseVelocityMovement;
        PlayerManager.OnPlayerHealthRestoredFromEmpty -= ResumeVelocityMovement;
    }

    private void Start()
    {
        Body = GetComponent<Rigidbody>();
        movementCollider = GetComponent<Collider>();
    }

    private void AccelerateTowardsDesiredVelocity(Vector3 desiredVelocity, float maxSpeedChange)
    {
        this.playerVelocity.x = Mathf.MoveTowards(playerVelocity.x, desiredVelocity.x, maxSpeedChange);
        this.playerVelocity.z = Mathf.MoveTowards(playerVelocity.z, desiredVelocity.z, maxSpeedChange);
    }

    private void Update()
    {
        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    oldWayEngaged = !oldWayEngaged;
        //}

        //if (oldWayEngaged)
        //{
        //    this.maxAcceleration = 30f;
        //    this.maxDeceleration = 15f;
        //}
        //else
        //{
        //    this.maxAcceleration = 100f;
        //    this.maxDeceleration = 30f;
        //}
        // 
        //if (this.oldWayEngaged == false)
        //{
        //    playerVelocity = Body.velocity;
        //}

        playerVelocity = Body.velocity;
        if (Input.GetMouseButton(0) && !isMovementInputPaused)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(mouseRay, out hit))
            {
                Vector3 desiredDirection = hit.point - this.transform.position;
                desiredPlayerVelocity = desiredDirection.normalized * maxSpeed;
                desiredMaxSpeedChange = this.maxAcceleration;
                //if (oldWayEngaged == true)
                //{
                //    this.transform.localPosition += this.playerVelocity * Time.deltaTime;
                //}   
            }
        }
        else if (this.playerVelocity.magnitude > 0 || this.pullVelocity.magnitude > 0)
        {
            desiredPlayerVelocity = pullVelocity;
            desiredMaxSpeedChange = this.maxDeceleration;
            //if (oldWayEngaged == true)
            //{
            //    this.transform.localPosition += this.playerVelocity * Time.deltaTime;
            //}   
        }
    }

    private void FixedUpdate()
    {
        //if (oldWayEngaged == false)
        //{
        //    Body.velocity = playerVelocity;
        //}
        //else
        //{
        //    Body.velocity = Vector3.zero;
        //}
        float maxSpeedChange = desiredMaxSpeedChange * Time.deltaTime;

        this.AccelerateTowardsDesiredVelocity(desiredPlayerVelocity, maxSpeedChange);
        if (this.playerVelocity.magnitude < 0)
        {
            this.playerVelocity = Vector3.zero;
        }

        Body.velocity = playerVelocity;
    }

    public void SetPullVelocity(Vector3 pullVelocity)
    {
        this.pullVelocity = pullVelocity;
    }
}
