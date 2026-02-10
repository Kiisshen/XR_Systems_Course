using System.Runtime.CompilerServices;
using UnityEngine;

public class MagnifyerLensScript : MonoBehaviour
{
    public GameObject playerHead;
    public Camera lensCamera;

    void Update()
    {
        lensCamera.transform.LookAt(playerHead.transform.position);
        lensCamera.transform.Rotate(0f, 180f, 0f, Space.Self);
    }
}
