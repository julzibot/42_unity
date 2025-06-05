using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AlignGroundChecks : MonoBehaviour
{
    [MenuItem("Tools/Align GroundCheck to Bottom")]
    static void AlignAllGroundChecks()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            Collider col = obj.GetComponent<Collider>();
            Transform groundCheck = obj.transform.Find("groundCheck");

            if (col != null && groundCheck != null)
            {
                Bounds bounds = col.bounds;
                Vector3 bottomCenter = bounds.center - new Vector3(0, bounds.extents.y, 0);

                Vector3 localPos = obj.transform.InverseTransformPoint(bottomCenter);
                groundCheck.localPosition = localPos - new Vector3(0, 0.05f, 0); // optional offset
                Debug.Log($"Moved GroundCheck of {obj.name} to {groundCheck.localPosition}");
            }
        }
    }
}
