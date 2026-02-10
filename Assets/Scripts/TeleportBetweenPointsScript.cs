using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class TeleportBetweenPointsScript : MonoBehaviour
{
    public InputActionReference teleportKey;

    public GameObject player;
    public Transform viewingPlatform;
    public Transform startingPosition;

    private bool isAtViewingPlatform = false;

    void Start()
    {
        teleportKey.action.Enable();
        player.transform.position = startingPosition.position;
        player.transform.rotation = startingPosition.rotation;
    }

    void Update()
    {
        if (teleportKey.action.WasPressedThisFrame())
        {
            isAtViewingPlatform = !isAtViewingPlatform;

            if (isAtViewingPlatform)
            {
                player.transform.position = viewingPlatform.position;
            }
            else
            {
                player.transform.position = startingPosition.position;
            }
        }
    }
}
