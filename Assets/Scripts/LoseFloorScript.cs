using UnityEngine;

public class LoseFloorScript : MonoBehaviour
{
    public GameObject gameManager;
    private GamePlatformScript gameManagerScript;

    private void Start()
    {
        gameManagerScript = gameManager.GetComponent<GamePlatformScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit lose collider");
            gameManagerScript.FullResetGame();
        }
    }
}
