using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Action;

public class ObjectScaler : MonoBehaviour
{
    [Header("Behaviour Adjustments")]
    [Tooltip("Adjusts the amount and object is scaled each second. Adjust for different feel")]
    [SerializeField] private float scaleUnitsPerSecond = 2f;

    [Tooltip("Minimum amount in the y-direction the object must move for any scaling to actually occur. Ideally should be  very small")]
    [SerializeField] private float minimumMovementForScale = 0.001f;

    [Tooltip("Minimum local scale the objectToScale to be set to.")]
    [SerializeField] private float minimumLocalScale = 0.5f;

    [Tooltip("Maximum local scale the objectToScale to be set to.")]
    [SerializeField] private float maximumLocalScale = 6.0f;

    [Header("Required Components")]
    [Tooltip("Object that will be scaled according to the trackedObject's upward motion.")]
    [SerializeField] private Transform objectToScale;

    [Tooltip("Object to track transform movement of while the boolean action is true.")]
    [SerializeField] private Transform trackedObject;

    // Associated boolean action.
    [Tooltip("Boolean action that allows scaling to occur when true.")]
    [SerializeField] private BooleanAction activationAction;

    private float lastUpdateY;

    private bool scalingEnabled = false;

    private void Update()
    {
        if(scalingEnabled)
        {
            Debug.Log("Scaling object");
            ScaleObject();
        }
        lastUpdateY = trackedObject.transform.localPosition.y;
    }

    // Scales object based on difference from last read value with frame into account.
    public void ScaleObject()
    {
        float unitDiff = (trackedObject.transform.localPosition.y - lastUpdateY);
        if (Mathf.Abs(unitDiff) < minimumMovementForScale * objectToScale.localScale.x) return;

        // Normalize difference to allow custom values to affect as desired.
        unitDiff = (unitDiff < 0f) ? -1f : 1f;
        unitDiff = unitDiff * Time.deltaTime * scaleUnitsPerSecond;
        //Debug.Log("unitDiff: " + unitDiff);

        float newScaleX = Mathf.Clamp(objectToScale.localScale.x + unitDiff, minimumLocalScale, maximumLocalScale);
        float newScaleY = Mathf.Clamp(objectToScale.localScale.y + unitDiff, minimumLocalScale, maximumLocalScale);
        float newScaleZ = Mathf.Clamp(objectToScale.localScale.z + unitDiff, minimumLocalScale, maximumLocalScale);

        objectToScale.localScale = new Vector3(newScaleX, newScaleY, newScaleZ);
        //Debug.log("Scaling object to :" + objectToScale.localScale);
    }

    public void ToggleScaling(bool newVal)
    {
        scalingEnabled = newVal;
    }
}
