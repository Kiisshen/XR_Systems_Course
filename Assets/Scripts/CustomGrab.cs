using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomGrab : MonoBehaviour
{
    public List<Transform> nearObjects = new List<Transform>();
    public InputActionReference action;
    public CustomGrab otherHand = null;
    public Transform grabbedObject = null;
    public bool grabbing = false;

    Vector3 prevPos;
    Quaternion prevRot;

    public bool doubleRotation = false;

    private void Start()
    {
        action.action.Enable();

        foreach (CustomGrab c in transform.parent.GetComponentsInChildren<CustomGrab>())
        {
            if (c != this)
                otherHand = c;
        }

        prevPos = transform.position;
        prevRot = transform.rotation;
    }

    void Update()
    {
        float grabMultiply = 1f;

        if (otherHand != null &&
            otherHand.grabbing &&
            otherHand.grabbedObject == grabbedObject)
        {
            grabMultiply = 0.5f;
        }

        grabbing = action.action.IsPressed();
        if (grabbing)
        {
            Debug.Log("Grabbingngngnggnn with: " + transform.gameObject.name);
            if (!grabbedObject)
                grabbedObject = nearObjects.Count > 0 ? nearObjects[0] : otherHand.grabbedObject;

            if (grabbedObject)
            {
                Vector3 deltaPos = (transform.position - prevPos) * grabMultiply;
                Quaternion deltaRot = transform.rotation * Quaternion.Inverse(prevRot);
                deltaRot = Quaternion.Slerp(Quaternion.identity, deltaRot, grabMultiply);

                if (doubleRotation)
                    deltaRot = Quaternion.Slerp(Quaternion.identity, deltaRot, 2f);

                Vector3 offset = grabbedObject.position - prevPos;
                offset = deltaRot * offset;

                grabbedObject.position = prevPos + offset;
                grabbedObject.position += deltaPos;
                grabbedObject.rotation = deltaRot * grabbedObject.rotation;
            }
        }
        else if (grabbedObject)
            grabbedObject = null;

        prevPos = transform.position;
        prevRot = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform t = other.transform;
        if (t && t.tag.ToLower() == "grabbable")
            nearObjects.Add(t);
    }

    private void OnTriggerExit(Collider other)
    {
        Transform t = other.transform;
        if (t && t.tag.ToLower() == "grabbable")
            nearObjects.Remove(t);
    }
}