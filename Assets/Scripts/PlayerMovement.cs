using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rgbd;
    public float movementForce=30;
    public float fallMultiplier = 1.5f;
    public float jumpForce = 2;
    public bool isGrounded = false;

    // Start is called before the first frame update
    void Start()
    {
        rgbd = GetComponent<Rigidbody>();
        rgbd.drag = 2;
    }

    // Update is called once per frame
    void Update()
    {
        float moveForward = Input.GetAxis("Vertical");
        float moveHorizonal = Input.GetAxis("Horizontal");
        Vector3 movementVector = new Vector3(moveHorizonal,0,moveForward);
        rgbd.AddForce(movementVector*movementForce, ForceMode.Force);

        
        float jump = Input.GetAxis("Jump");
        Vector3 jumpVector = new Vector3(0, jump, 0);
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rgbd.AddForce(jumpVector * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (rgbd.velocity.y < 0)
        {
            rgbd.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }
}
