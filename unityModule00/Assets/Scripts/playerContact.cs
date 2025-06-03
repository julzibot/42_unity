using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerContact : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Lava caught you ! GAME OVER");
            Destroy(other.gameObject);
        }
    }
}
