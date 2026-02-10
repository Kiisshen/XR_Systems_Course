using UnityEngine;
using UnityEngine.InputSystem;

public class LightSwitchScript : MonoBehaviour
{
    public InputActionReference xButton;
    public Light targetLight;

    private bool isLightOn = true;

    void Start()
    {
        xButton.action.Enable();
        if (targetLight == null)
            targetLight = GetComponent<Light>();

        targetLight.enabled = isLightOn;
    }

    void Update()
    {
        if (xButton.action.WasPressedThisFrame())
        {
            isLightOn = !isLightOn;
            targetLight.enabled = isLightOn;

            Debug.Log("X pressed, light is now: " + isLightOn);
        }
    }
}
