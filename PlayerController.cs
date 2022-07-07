using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public float groundDrag;

    [Header("Keybinds")]
    public KeyCode jumpkey = KeyCode.Space;

    void Start(){
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        ResetJump();
    }

    void MyInput(){
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpkey) && readyToJump && grounded){
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump),jumpCooldown);
        }
    }

    void MovePlayer(){
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
    
        if(grounded){
            rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
        }
        else if(!grounded){
            rb.AddForce(moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force);

        }
    }

    void speedControl(){
        Vector3 flatVel = new Vector3(rb.velocity.x,0f,rb.velocity.z);
        if(flatVel.magnitude > speed){
            Vector3 limitedVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(limitedVel.x,rb.velocity.y,limitedVel.z);
        }
    }

    void Jump(){
        rb.velocity = new Vector3(rb.velocity.x,0f,rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ResetJump(){
        readyToJump = true;
    }

    void Update(){
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        MyInput();
        speedControl();
        if(grounded){
            rb.drag = groundDrag;
        }
        else{ rb.drag = 0; }
    }

    void FixedUpdate(){
        MovePlayer();
    }



}
