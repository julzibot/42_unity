using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    private Dictionary<Transform, Transform> teleportPairs = new Dictionary<Transform, Transform>();
    private Dictionary<Transform, float> teleportTimers = new Dictionary<Transform, float>();
    public float teleportRadius = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
         foreach (Transform teleporter in transform)
        {
            if (teleporter.childCount >= 2)
            {
                Transform portA = teleporter.GetChild(0);
                Transform portB = teleporter.GetChild(1);
                
                teleportPairs[portA] = portB;
                teleportPairs[portB] = portA;
                teleportTimers[portA] = -3f;
                teleportTimers[portB] = -3f;
            }
        }
    }

    void Update()
    {
        foreach (var pair in teleportPairs)
        {
            Transform port = pair.Key;
            Transform destination = pair.Value;

            Collider[] hits = Physics.OverlapSphere(port.position, teleportRadius);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player") && Time.time - teleportTimers[port] > 3f)
                {
                    hit.transform.position = destination.position;
                    teleportTimers[port] = Time.time;
                    teleportTimers[destination] = Time.time;
                }
            }
        }
    }
}
