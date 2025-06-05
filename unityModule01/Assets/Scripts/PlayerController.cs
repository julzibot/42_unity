using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public Transform transform;
    public Rigidbody rb;
    public float jumpForce;
    public float groundCheckRadius;
    public float speed;
    public Transform groundCheckTransform;
    public bool onGround;
    public Vector3 startPosition;
    public float endPosition;

    public Player(Transform child, Rigidbody rigidbody, float jumpForce, float groundCheckRadius, float speed, Transform groundCheckTransform, Vector3 startPos, float endPos)
    {
        this.transform = child;
        this.rb = rigidbody;
        this.jumpForce = jumpForce;
        this.groundCheckRadius = groundCheckRadius;
        this.speed = speed;
        this.groundCheckTransform = groundCheckTransform;
        this.onGround = true;
        this.startPosition = startPos;
        this.endPosition = endPos;
    }
}

public class PlayerController : MonoBehaviour
{
    // public List<Rigidbody> playerRigidbodies = new List<Rigidbody>();
    public KeyCode[] playerSwitch = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3};
    public List<Player> players = new List<Player>();
    public Transform cameraTransform;
    public Transform[] endCircles;
    private int activePlayerIndex = 0;
    private bool cameraSwitch = false;
    private float switchTimer;
    private bool mustJump;
    private float descentStart;
    private bool fallStart;
    private Vector3 cameraStartPos;
    public int endCounter = 0;

    void Start()
    {
        int j = 0;
        float[] jumpForces = new float[] {94f, 11f, 23f};
        float[] playerSpeeds = new float[] {3f, 4f, 5.5f};
        float[] groundCheckRadius = new float[] {1.95f, 0.45f, 0.25f};
        float[] endPositions = new float[] {54.4f, 58.2f, 60.2f};
        
        foreach(Transform child in transform)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Debug.Log("index: " + j);
                players.Add(new Player(child, rb, jumpForces[j], groundCheckRadius[j], playerSpeeds[j], child.Find("groundCheck"), rb.position, endPositions[j]));
                j++;
            }
        }
        for (int k = 0; k < 3; k++)
        {
            endCircles[k].gameObject.SetActive(false);
            endCircles[k].gameObject.GetComponent<Collider>().enabled = false;
        }
        cameraStartPos = cameraTransform.position;
    }

    void Update()
    {
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            for (int i = 0; i < 3; i++)
                players[i].rb.velocity = new Vector3(0f, players[i].rb.velocity.y, 0f);
        }
        for (int i = 0; i < 3; i++)
        {
            if (Input.GetKeyDown(playerSwitch[i]) && i != activePlayerIndex)
            {
                activePlayerIndex = i;
                cameraSwitch = true;
                switchTimer = Time.time;
            }
        }
        Player activePlayer = players[activePlayerIndex];

        //JUMP HANDLING
        Vector3 colBoxVector = new Vector3(activePlayer.groundCheckRadius, 0.2f, activePlayer.groundCheckRadius);
        Collider[] hitColliders = Physics.OverlapBox(activePlayer.groundCheckTransform.position, colBoxVector, activePlayer.groundCheckTransform.rotation);
        activePlayer.onGround = false;
        foreach (Collider col in hitColliders)
        {
            if (col.attachedRigidbody != activePlayer.rb)
            {
                activePlayer.onGround = true;
                break;
            }
        }
        if (activePlayer.onGround && Input.GetKeyDown(KeyCode.Space))
            mustJump = true;

        // PLAYER SWITCH HANDLING
        Vector3 cameraTargetPos = activePlayer.rb.position + new Vector3(23f, 8f, 0f);
        if (cameraSwitch)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraTargetPos, 0.025f);
            if (Vector3.Distance(cameraTransform.position, cameraTargetPos) < 0.07f || Time.time - switchTimer > 0.4)
                cameraSwitch = false;
        }
        else
            cameraTransform.position = cameraTargetPos;

        // RESET
        if (Input.GetKeyDown(KeyCode.R))
        {
            activePlayerIndex = 0;
            for (int r = 0; r < players.Count; r++)
            {
                players[r].rb.MovePosition(players[r].startPosition);
            }
            cameraTransform.position = cameraStartPos;
        }

        //EXIT
        // Debug.Log("Claire pos: " + players[1].rb.position.z + " / " + players[2].rb.position.z);
        for (int p = 0; p < 3; p++)
        {
            if (!endCircles[p].gameObject.activeSelf && players[p].rb.position.z > players[p].endPosition - 0.2f && players[p].rb.position.z < players[p].endPosition + 0.2f && players[p].rb.position.y > 0f && players[p].rb.position.y < 11f)
            {
                endCircles[p].gameObject.SetActive(true);
                endCounter += 1;
            }
            else if (endCircles[p].gameObject.activeSelf && (players[p].rb.position.z < players[p].endPosition - 0.2f || players[p].rb.position.z > players[p].endPosition + 0.2f || players[p].rb.position.y < 0f || players[p].rb.position.y > 11f))
            {
                endCircles[p].gameObject.SetActive(false);
                endCounter -= 1;
            }
        }
        if (endCounter == 3)
            Debug.Log("SUCCESS ! GOING TO NEXT STAGE");
    }

    void FixedUpdate()
    {
        // PHYSIC-BASED MOVEMENT
        Rigidbody rb = players[activePlayerIndex].rb;
        float move = Input.GetAxis("Horizontal");
        // Vector3 groundMovement = new Vector3(0f, 0f, move);
        rb.AddForce(new Vector3(0f, 0f, move * players[activePlayerIndex].speed * rb.mass / Time.fixedDeltaTime), ForceMode.Force);
        if (rb.velocity.z > players[activePlayerIndex].speed)
            rb.velocity = new Vector3(0f, rb.velocity.y, players[activePlayerIndex].speed);
        else if (rb.velocity.z < -1f * players[activePlayerIndex].speed)
            rb.velocity = new Vector3(0f, rb.velocity.y, -1f * players[activePlayerIndex].speed);
        // rb.velocity = new Vector3(0f, 0f, move * players[activePlayerIndex].speed);
        // rb.MovePosition(rb.position + groundMovement * players[activePlayerIndex].speed * Time.fixedDeltaTime);
        if (mustJump)
        {
            rb.AddForce(Vector3.up * players[activePlayerIndex].jumpForce, ForceMode.Impulse);
            mustJump = false;
            fallStart = true;
        }
        if (rb.velocity.y < 0)
        {
            if (fallStart)
            {
                descentStart = Time.time;
                fallStart = false;
            }
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * Mathf.Max((1.35f - (Time.time - descentStart)), 1f), rb.velocity.z);
        }
    }
}
