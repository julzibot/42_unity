using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTriggerForwarder : MonoBehaviour
{
    public ButtonActivationScript buttonScript;

    void OnTriggerEnter(Collider other)
    {
        buttonScript.OnTriggerEnterFromChild(other);
    }

    void OnTriggerExit(Collider other)
    {
        buttonScript.OnTriggerExitFromChild(other);
    }
}
