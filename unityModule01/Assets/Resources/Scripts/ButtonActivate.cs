using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActivationScript : MonoBehaviour
{
    public int buttonType;
    public float pressDepth = 0.2f;
    public float pressSpeed = 2f;
    public float returnSpeed = 0.5f;
    // public Transform door;
    private float startY;
    private int pressCount = 0;
    private bool pressed = false;
    private Transform pressDetect;
    private Material[] platformMats;
    private int platformLayerIndex;
    private int matIndex;
    // Start is called before the first frame update
    void Start()
    {
        startY = transform.position.y;
        pressDetect = transform.GetChild(0);
        platformMats = new Material[] {
            Resources.Load<Material>("Materials/MovingPlatform1Material"),
            Resources.Load<Material>("Materials/MovingPlatform2Material"),
            Resources.Load<Material>("Materials/MovingPlatform3Material")
        };
        for (int i = 0; i < 3; i++)
        {
            if (GetComponent<Renderer>().sharedMaterial == platformMats[i])
            {
                matIndex = i;
                break;
            }
        }
        platformLayerIndex = matIndex + 6;
    }

    // Update is called once per frame
    void Update()
    {
        if (pressCount > 0 && pressed == false)
            transform.position += Vector3.down * pressSpeed * Time.deltaTime;
        else if (pressCount == 0 && transform.position.y < startY)
            transform.position += Vector3.up * returnSpeed * Time.deltaTime;
        if (transform.position.y <= startY - pressDepth && pressed == false)
        {
            pressed = true;
            foreach (Transform child in transform)
            {
                if (buttonType < 2 && child.CompareTag("Door") && (pressDetect.gameObject.layer == 0 || child.gameObject.layer == pressDetect.gameObject.layer + 3))
                    child.gameObject.SetActive(false);
                else if (buttonType == 2 && child.CompareTag("MovingPlatform"))
                {
                    matIndex = (matIndex + 1) % 3;
                    platformLayerIndex = ((platformLayerIndex - 5) % 3) + 6;
                    child.gameObject.GetComponent<Renderer>().material = platformMats[matIndex];
                    GetComponent<Renderer>().material = platformMats[matIndex];
                    child.gameObject.layer = platformLayerIndex;
                }
            }
        }
        else if (transform.position.y > startY)
            pressed = false;
    }

    public void OnTriggerEnterFromChild(Collider p)
    {
        if (p.CompareTag("Player"))
            pressCount++;
        if (buttonType == 1)
        {
            pressDetect.gameObject.layer = p.gameObject.layer - 3;
            GetComponent<Renderer>().material = p.gameObject.GetComponent<Renderer>().material;
        }
    }

    public void OnTriggerExitFromChild(Collider p)
    {
        if (p.CompareTag("Player"))
            pressCount--;
        if (pressCount < 0)
            pressCount = 0;
        // if (pressCount == 0)
        //     pressed = false;
        if (buttonType == 1)
            pressDetect.gameObject.layer = 0;
    }
}
