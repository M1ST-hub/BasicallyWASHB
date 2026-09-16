    using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public List<GameObject> players;
    public GameObject[] redSpawns;
    public GameObject[] blueSpawns;
    public GameObject[] yellowSpawns;
    public GameObject[] greenSpawns;
    public GameObject[] endPoints;
    


    [Header("Timers")]
    public GameObject gameTimer;
    public GameObject preGameTimer;
    public GameObject postGameTimer;
    public GameObject canvas;
    public bool isGameStarted = false;

    private GameObject timmy;
    private GameObject playTime;
    private GameObject endTimer;

    public enum  GameMode
    {
       TeamDeathmatch,
       CaptureTheFlag,
       KingOfTheHill,
       Dual
    }

    public enum TeamColor
    {
        Red,
        Blue,
        Green,
        Yellow
    }


    //public PlayerMovement pc;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        Timer.gameStart = false;
        Timer.gameEnd = false;
        //NetworkManager.Singleton.OnClientConnectedCallback += PlayerJoined;
    }

    void Update()
    {

    }


    [Rpc(SendTo.Everyone)]
    public void GameStartRpc()
    {
        //if (!IsServer) return; // Only the server should control position changes

        GetPlayers();

        int colorIndex = 0; // Start with the first color
        foreach (GameObject player in players)
        {
            Rigidbody rb = player.GetComponentInChildren<Rigidbody>();
            if (rb != null)
            {
                // Stop momentum
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            Debug.Log(player, player);

            player.GetComponent<Target>().currentGameMode = GameMode.Dual; // Set the game mode for the player
            player.GetComponent<Target>().currentTeamColor = (TeamColor)colorIndex; // Set the team color for the player
            colorIndex = (colorIndex + 1) % System.Enum.GetValues(typeof(TeamColor)).Length; // Move to the next color

            Vector3 spawnPosition = Vector3.zero; // Default spawn position

            if (colorIndex == (int)TeamColor.Red)
            {
                 spawnPosition = redSpawns[Random.Range(0, redSpawns.Length)].transform.position + Vector3.up * 1f;
            }
            else if (colorIndex == (int)TeamColor.Blue)
            {
                 spawnPosition = blueSpawns[Random.Range(0, blueSpawns.Length)].transform.position + Vector3.up * 1f;
            }
            else if (colorIndex == (int)TeamColor.Green)
            {
                 spawnPosition = greenSpawns[Random.Range(0, greenSpawns.Length)].transform.position + Vector3.up * 1f;
            }
            else if (colorIndex == (int)TeamColor.Yellow)
            {
                 spawnPosition = yellowSpawns[Random.Range(0, yellowSpawns.Length)].transform.position + Vector3.up * 1f;
                // Safe spawn with a slight upward offset to ensure grounded check will work
            }
                player.transform.position = spawnPosition;


            // Reset rotation if needed
            player.transform.rotation = Quaternion.identity;

            // Optional debug
            Debug.Log($"Spawned {player.name} at {spawnPosition}");

            Debug.Log("Set player spawn position to: " + spawnPosition);
        }

        //Destroy(timmy);

        if (IsServer)
            SpawnGameTimerRpc();

        Timer.gameStart = true;

        Debug.Log("GameStart");
    }


    [Rpc(SendTo.Everyone)]
    public void GameEndRpc()
    {
        if (!IsServer) return; // Only the server should control position changes

        GetPlayers();

        foreach (GameObject player in players)
        {
            Rigidbody rb = player.GetComponentInChildren<Rigidbody>();
            if (rb != null)
            {
                // Stop momentum
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // Safe spawn with a slight upward offset to ensure grounded check will work
            Vector3 endPosition = endPoints[Random.Range(0, endPoints.Length)].transform.position + Vector3.up * 1f;
            player.transform.position = endPosition;

            // Reset rotation if needed
            player.transform.rotation = Quaternion.identity;

            // Optional debug
            Debug.Log($"Spawned {player.name} at {endPosition}");
        }

        //Destroy(playTime);
        if (IsServer)
            SpawnPostGameTimerRpc();

        Debug.Log("GameEnd");
    }

    [Rpc(SendTo.Everyone)]
    public void GameRestartRpc()
    {
        if (!IsServer) return; // Only the server should control position changes

        GetPlayers();

        foreach (GameObject player in players)
        {
            Rigidbody rb = player.GetComponentInChildren<Rigidbody>();
            if (rb != null)
            {
                // Stop momentum
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // Safe spawn with a slight upward offset to ensure grounded check will work
            Vector3 restartPosition = endPoints[Random.Range(0, endPoints.Length)].transform.position + Vector3.up * 1f;
            player.transform.position = restartPosition;

            // Reset rotation if needed
            player.transform.rotation = Quaternion.identity;

            // Optional debug
            Debug.Log($"Spawned {player.name} at {restartPosition}");
        }

        //Destroy(endTimer);
        if (IsServer)
            SpawnTimerRpc();

        Timer.gameStart = false;

        Debug.Log("Game Restarted");
    }



  

    private void GetPlayers()
    {
        players.Clear();
        foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
        {
            players.Add(client.PlayerObject.gameObject);
        }
    }

    private void PlayerJoined(ulong clientId)
    {
        foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
        {
            if (client.ClientId == clientId)
            {
                players.Add(client.PlayerObject.gameObject);
            }
        }

        // NetworkManager.ConnectedClientsIds;
    }

    [Rpc(SendTo.Everyone)]
    public void SpawnTimerRpc()
    {

        if (IsServer)
        {
            timmy = Instantiate(preGameTimer, preGameTimer.transform.localPosition, Quaternion.identity, canvas.transform);
            timmy.GetComponent<NetworkObject>().Spawn();
            timmy.transform.SetParent(canvas.transform);
            timmy.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -170, 0);
        }

    }

    [Rpc(SendTo.Everyone)]
    public void SpawnGameTimerRpc()
    {
        if (IsServer)
        {
            playTime = Instantiate(gameTimer, gameTimer.transform.localPosition, Quaternion.identity, canvas.transform);
            playTime.GetComponent<NetworkObject>().Spawn();
            playTime.transform.SetParent(canvas.transform);
            playTime.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -170, 0);
        }

    }

    [Rpc(SendTo.Everyone)]
    public void SpawnPostGameTimerRpc()
    {
        if (IsServer)
        {
            endTimer = Instantiate(postGameTimer, postGameTimer.transform.localPosition, Quaternion.identity, canvas.transform);
            endTimer.GetComponent<NetworkObject>().Spawn();
            endTimer.transform.SetParent(canvas.transform);
            endTimer.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -170, 0);
        }
    }



}
