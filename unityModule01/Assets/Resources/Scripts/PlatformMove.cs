using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Move : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 travelVector = new Vector3(0f, 0f, 1f);
    public float travelDistance = 10f;
    public float speed = 5f;
    private float startTime;
    private float directionToggle = 1;
    List<Collider> playerColliders = new List<Collider>();
    // private Vector3 size = GetComponent<Renderer>().bounds.size;
    private Vector3 move;
    private Vector3 playerMove;
    public  Vector3 colBoxVector;
    public Vector3  colBoxCenter;
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
        if ((directionToggle == 1f && Vector3.Distance(startPos, transform.position) >= travelDistance) || (directionToggle == -1f && Vector3.Distance(startPos + travelDistance * travelVector, transform.position) >= travelDistance))
            directionToggle *= -1f;

        // travelVector.Normalize();
        move = travelVector * directionToggle * speed;
        playerMove = move;
        if (directionToggle == 1)
            playerMove = new Vector3(0f, 0f, playerMove.z);

        Collider[] hitColliders = Physics.OverlapBox(transform.position + colBoxCenter, colBoxVector, transform.rotation);
        foreach (Collider col in hitColliders)
        {
            if (col.attachedRigidbody && col.gameObject.layer == transform.gameObject.layer + 3)
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

    void OnDrawGizmos()
    {

        Gizmos.color = Color.green;
        Gizmos.matrix = Matrix4x4.TRS(colBoxCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, colBoxVector * 2); // Multiply by 2 because OverlapBox uses half-extents
    }
}
