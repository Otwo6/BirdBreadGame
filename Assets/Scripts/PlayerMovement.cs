using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(OwnerNetworkAnimator))]
public class PlayerMovement : NetworkBehaviour
{
	float speed = 10f; // Forward speed
    public float maxSpeed = 10f;
    public float turnSpeed = 50f; // Rotation speed
    public float liftStrength = 10f; // How much lift the plane generates
    public float gravityScale = 9.8f; // Simulated gravity strength
    private Rigidbody rb;
	public float flapHeight;

    [SerializeField] OwnerNetworkAnimator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // We control gravity manually for smoother gliding
    }

    void Update()
    {
        // Forward movement
        Vector3 forwardMovement = transform.forward * speed * Time.deltaTime;
        rb.MovePosition(rb.position + forwardMovement);

        // Pitch (up/down) control
        float pitch = Input.GetAxis("Vertical") * turnSpeed * Time.deltaTime;
        transform.Rotate(Vector3.right * pitch);

        // Yaw (left/right) control
        float yaw = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * yaw);

		if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * flapHeight, ForceMode.Impulse);
            anim.SetTrigger("Flap Wings");
            print("JUMP JUMP");
        }
    }

    void FixedUpdate()
    {
        // Calculate lift based on forward speed and pitch angle
        float lift = Mathf.Max(0, Vector3.Dot(transform.up, Vector3.up)) * liftStrength;
        rb.AddForce(Vector3.up * lift - Vector3.up * gravityScale, ForceMode.Acceleration);
    }

    public void StopVelocity()
    {
        rb.linearVelocity = new Vector3(0f, 0f, 0f);
        speed = 2f;

        StartCoroutine(RegainSpeed());
    }

    IEnumerator RegainSpeed()
    {
        while(speed < maxSpeed)
        {
            speed += Time.deltaTime * 2f;
            yield return null;
        }

        speed = maxSpeed;
    }
}
