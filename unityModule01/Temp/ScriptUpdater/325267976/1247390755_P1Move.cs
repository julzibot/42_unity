using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Move : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 travelVector = new Vector3(0f, 1f, 1f);
    public float travelDistance = 10f;
    public float speed = 5f;
    public float startTime;
    float directionToggle = 1;
    List<Collider> playerColliders = new List<Collider>();
    private Vector3 move;
    // Update is called once per frame
    void Start()
    {
        startPos = transform.position;
        startTime = Time.time;
    }

    void Update()
    {
        // DIRECTION TOGGLE
        playerColliders.Clear();
        if ((Vector3.Distance(startPos, transform.position) >= travelDistance && directionToggle == 1f) || (Vector3.Distance(startPos, transform.position) < 0.1f && directionToggle == -1f))
            directionToggle *= -1f;

        // Vector3 move = travelVector * directionToggle * speed * Time.deltaTime;
        move = travelVector * directionToggle * speed;
        transform.position += move;

        Vector3 colBoxVector = new Vector3(1f, 0.2f, 1.70f);
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
        foreach (Collider p in playerColliders)
        {
            p.GetComponent<Rigidbody>().velocity += move;
        }
    }
}
