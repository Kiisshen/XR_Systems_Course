using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePlatformScript : MonoBehaviour
{
    public InputActionReference action;

    public TMP_Text text;
    public Transform player;
    public Transform magnifyingGlass;

    public GameObject warningBeep;
    public GameObject resetBeep;
    public GameObject breakPlatformBeep;
    public GameObject platformElement;
    public GameObject safeMarkerPrefab;

    private GameObject safeMarkerObject;
    private GameObject[] platformElements;
    private GameObject safePlatform;

    private int lastCountdownSecond = -1;
    private int currentScore = 0;
    private int lastScore = 0;

    private float timer;
    public float warningPeriod = 5f;
    public float activePeriod = 10f;
    public float resetTime = 3f;
    public float offSet = 1f;
    public float markerOffset = 0.1f;
    public int platformSize = 15;

    private bool isGameRunning = false;

    private enum PlatformState
    {
        Active,
        Warning,
        OnlySafe,
        Resetting
    }

    private PlatformState currentState;

    void Start()
    {
        platformElements = new GameObject[platformSize * platformSize];

        int index = 0;
        for (int x = 0; x < platformSize; x++)
        {
            for (int y = 0; y < platformSize; y++)
            {
                GameObject created = Instantiate(
                    platformElement,
                    transform.position + new Vector3(x * offSet, 0, y * offSet),
                    Quaternion.identity,
                    transform
                );

                platformElements[index] = created;
                index++;
            }
        }

        currentState = PlatformState.Active;
        timer = activePeriod;

        TeleportPlayerToCenter();

        action.action.Enable();
        UpdateUI();
    }

    void Update()
    {
        if (action.action.WasPressedThisFrame())
        {
            isGameRunning = !isGameRunning;
            if (isGameRunning)
            {
                Debug.Log("Game Started / Resumed");
                UpdateUI();
            }
            else
            {
                Debug.Log("Game Paused");
                UpdateUI();
            }
        }

        if (!isGameRunning)
            return;

        timer -= Time.deltaTime;

        switch (currentState)
        {
            case PlatformState.Active:
                if (timer <= 0)
                {
                    ChooseSafePlatform();
                    currentState = PlatformState.Warning;
                    timer = warningPeriod;
                }
                break;

            case PlatformState.Warning:
                int currentSecond = Mathf.CeilToInt(timer);

                if (currentSecond <= 3 && currentSecond > 0)
                {
                    if (currentSecond != lastCountdownSecond)
                    {
                        Instantiate(warningBeep, transform.position, Quaternion.identity);
                        lastCountdownSecond = currentSecond;
                    }
                }

                if (timer <= 0)
                {
                    Instantiate(breakPlatformBeep, transform.position, Quaternion.identity);

                    DisableAllExceptSafe();
                    currentState = PlatformState.OnlySafe;
                    timer = resetTime;
                    lastCountdownSecond = -1;
                }
                break;

            case PlatformState.OnlySafe:
                if (timer <= 0)
                {
                    Instantiate(resetBeep, transform.position, Quaternion.identity);

                    ResetPlatform();
                    currentState = PlatformState.Active;
                    timer = activePeriod;
                    currentScore++;
                }
                break;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (text == null) return;

        string timeDisplay = "";

        if (currentState == PlatformState.Active)
            timeDisplay = $"Next Round In: {Mathf.Floor(timer)}";

        else if (currentState == PlatformState.Warning)
            timeDisplay = $"Platforms Disappear In: {Mathf.Floor(timer)}";

        else if (currentState == PlatformState.OnlySafe)
            timeDisplay = $"Resetting In: {Mathf.Floor(timer)}";

        string uiText =
            $"{timeDisplay}\n" +
            $"Current Score: {currentScore}\n" +
            $"Last Score: {lastScore}";

        if (!isGameRunning)
            uiText += "\nGame Paused, press X to resume.";
        else
            uiText += "\nTo pause, press X.";

            text.text = uiText;
    }
    void ChooseSafePlatform()
    {
        int randomIndex = Random.Range(0, platformElements.Length);
        safePlatform = platformElements[randomIndex];

        safeMarkerObject = Instantiate(
            safeMarkerPrefab,
            safePlatform.transform
        );

        safeMarkerObject.transform.localPosition = new Vector3(0, markerOffset, 0);
    }

    void DisableAllExceptSafe()
    {
        foreach (GameObject tile in platformElements)
        {
            if (tile != safePlatform)
                tile.SetActive(false);
        }
    }

    void ResetPlatform()
    {
        foreach (GameObject tile in platformElements)
        {
            tile.SetActive(true);
        }

        if (safeMarkerObject != null)
            Destroy(safeMarkerObject);

        safePlatform = null;
    }

    void TeleportPlayerToCenter()
    {
        float boardSize = (platformSize - 1) * offSet;
        Vector3 centerPosition = transform.position +
                                 new Vector3(boardSize / 2f, 2f, boardSize / 2f);

        player.position = centerPosition;

        //if (magnifyingGlass.parent != transform)
        //{
        //    magnifyingGlass.position = player.position + new Vector3(0, 0.1f, 1.5f);
        //    magnifyingGlass.rotation = Quaternion.identity;
        //}

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void FullResetGame()
    {
        ResetPlatform();

        lastScore = currentScore;
        currentScore = 0;

        currentState = PlatformState.Active;
        timer = activePeriod;

        TeleportPlayerToCenter();
    }
}
