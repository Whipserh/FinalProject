using System.Diagnostics.CodeAnalysis;
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
    private Rigidbody rb;
    private Vector2 playerInputControls;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = 0;
        rb = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {
        //Get player controls
        playerInputControls = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
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
