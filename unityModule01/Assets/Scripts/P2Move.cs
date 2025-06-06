using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2Move : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 travelVector = new Vector3(0f, 1f, 0f);
    public float travelDistance = 10f;
    public float speed = 5f;
    private float startTime;
    private float directionToggle = 1;
    List<Collider> playerColliders = new List<Collider>();
    private Vector3 move;
    private Vector3 playerMove;
    // Update is called once per frame
    void Start()
    {
        startPos = transform.position;
        startTime = Time.time;
        travelVector = travelVector.normalized;
    }

    void Update()
    {
        // DIRECTION TOGGLE
        playerColliders.Clear();
        if ((Vector3.Distance(startPos, transform.position) >= travelDistance && directionToggle == 1f) || (Vector3.Distance(startPos, transform.position) < 0.1f && directionToggle == -1f))
            directionToggle *= -1f;

        // travelVector.Normalize();
        move = travelVector * directionToggle * speed;
        playerMove = move;
        if (directionToggle == 1)
            playerMove = new Vector3(0f, 0f, playerMove.z);

        Vector3 colBoxVector = new Vector3(2f, 2.4f, 0.2f);
        Collider[] hitColliders = Physics.OverlapBox(transform.position + new Vector3(0f, 0.6f, 0f), colBoxVector, transform.rotation);
        foreach (Collider col in hitColliders)
        {
            if (col.attachedRigidbody)
            {
                playerColliders.Add(col);
                // col.attachedRigidbody.position += move;
            }
        }
    }

    void FixedUpdate()
    {
        transform.position += move * Time.fixedDeltaTime;
        foreach (Collider p in playerColliders)
            p.GetComponent<Rigidbody>().MovePosition(p.GetComponent<Rigidbody>().position + playerMove * Time.fixedDeltaTime);
    }

    // VISUALIZE MOVING PLATFORM COLLISION BOX

    // void OnDrawGizmos()
    // {
    //     Vector3 colBoxVector = new Vector3(2f, 1.65f, 0.25f);
    //     Vector3 center = transform.position + new Vector3(0f, 0.65f, 0f);

    //     Gizmos.color = Color.green;
    //     Gizmos.matrix = Matrix4x4.TRS(center, transform.rotation, Vector3.one);
    //     Gizmos.DrawWireCube(Vector3.zero, colBoxVector * 2); // Multiply by 2 because OverlapBox uses half-extents
    // }
}
