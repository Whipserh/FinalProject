using System.Diagnostics.CodeAnalysis;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private float MaxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float angularAcceleration;
    [SerializeField] private float angularHaltingSpeed;
    private float speed;
    private Vector2 playerInputControls;

    public Transform leftSpawn, rightSpawn;
    public GameObject canonBallPrefab;
    public Transform canonBallsTransforms;
    public float power;
    [SerializeField] private float reloadTime = 1;
    private float lastShotTime = 0;

    [SerializeField]private GameObject playerCamera;
    private Rigidbody rb;
    

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        speed = 0;
        rb = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {
        //Get player controls
        playerInputControls = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Debug.Log(Time.time - lastShotTime);
        if (Input.GetKeyDown(KeyCode.Space) && Time.time - lastShotTime  >= reloadTime)
            fireCanonBall();
    }
    public float getFacingAngle()
    {
        Vector3 facingDirectOnPlane = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        return Mathf.Atan2(facingDirectOnPlane.x, facingDirectOnPlane.z) * Mathf.Rad2Deg;
    }
    public void fireCanonBall()
    {
        //reset reload timer
        lastShotTime = Time.time;

        //choose which side to fire from
        Vector3 directionToCamera = (transform.position - playerCamera.transform.position);
        Vector3 directionOnPlane = Vector3.ProjectOnPlane(directionToCamera, Vector3.up).normalized;
        
        float dotProduct = Vector3.Dot(Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized, directionOnPlane);
        Debug.Log(dotProduct);

        Transform spawnLocation = (dotProduct > 0 )? rightSpawn.transform: leftSpawn.transform;


        //create the canonball and shoot it
        GameObject canonBall = Instantiate(canonBallPrefab, spawnLocation.position, spawnLocation.rotation, canonBallsTransforms);
        Rigidbody rbCanonBall = canonBall.GetComponent<Rigidbody>();
        rbCanonBall.AddForce(power * canonBall.transform.forward);
    }

    private void FixedUpdate()
    {

        //increase the speed of the ship based off of the player's input
        float speedChange = (playerInputControls.y > 0) ? acceleration : deceleration;
        speed = Mathf.Clamp(speed + (speedChange * playerInputControls.y * Time.fixedDeltaTime), 0, MaxSpeed);
        if (speed < 0.01) speed = 0;

        //get the direction that we are facing by projecting the forward vector onto the horizontal plane
        Vector3 newVelocity = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized * speed;

        rb.linearVelocity = new Vector3(newVelocity.x, rb.linearVelocity.y, newVelocity.z);

        //rotate the ship
        rb.angularVelocity += transform.up * angularAcceleration * Time.fixedDeltaTime * playerInputControls.x;
        if (rb.angularVelocity.magnitude < angularHaltingSpeed) rb.angularVelocity = Vector3.zero;

        
    }
}
