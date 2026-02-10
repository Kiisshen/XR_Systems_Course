using UnityEngine;

public class MoonOrbitScript : MonoBehaviour
{
    public float rotationSpeed = 1.0f;
    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
