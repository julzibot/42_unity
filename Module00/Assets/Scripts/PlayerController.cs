using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    public float moveSpeed = 30f;
    public float jumpForce = 0.1f;
    public Transform cameraTransform;
    public float groundCheckRadius = 0.2f;
    public float[] times = new float[5];
    public KeyCode[] keys = new KeyCode[] { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.W, KeyCode.Space };
    public KeyCode[] arrowKeys = new KeyCode[] { KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.UpArrow};

    private Rigidbody rb;
    private bool isGrounded;
    private bool onLava;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
            times[i] = 0f;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = 0f;
        float moveY = 0f;
        float moveZ = 0f;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.2f);
        // onLava = 
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKeyDown(keys[i]) || Input.GetKeyDown(arrowKeys[i]))
                times[i] = Time.time;
            else if (Input.GetKeyUp(keys[i]) || Input.GetKeyUp(arrowKeys[i]))
                times[i] = 0f;
            // UnityEngine.Debug.Log("times[i]: " + times[i]);
            if (times[i] > 0f)
            {
                if (i == 0 || i == 2)
                    moveX += (i - 1);
                else if (i == 1 || i == 3)
                    moveZ += (i - 2);
            }
        }
        if (moveX == 0f && moveZ == 0f)
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        if (Input.GetKeyDown(keys[4]) && isGrounded)
        {
            times[4] = Time.time;
            rb.AddForce(Vector3.up * 3f, ForceMode.Impulse);
        }
        else if (Input.GetKey(keys[4]))
            moveY = Mathf.Max(10f - (Time.time - times[4]), 0f) ;


        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();
        // Calculate the movement vector
        // float moveX = Input.GetAxis("Horizontal");
        // float moveZ = Input.GetAxis("Vertical");
        Vector3 groundMovement = camForward * moveZ + camRight * moveX;
        Vector3 upMovement = Vector3.up * moveY;
        groundMovement.Normalize();
        upMovement.Normalize();
        Vector3 finalMovement = groundMovement + upMovement;
        finalMovement *= 0.05f;
        // movement *= moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + finalMovement);
        cameraTransform.position = transform.position + -7f * camForward + 5f * Vector3.up;
        cameraTransform.LookAt(transform);
    }
}

// public class LavaContact : MonoBehaviour
// {
//     public Transform lavaCheck;
//     public LayerMask lavaLayer;
//     public float checkRadius = 0.1f;

//     void Update()
//     {

//     }
// }
