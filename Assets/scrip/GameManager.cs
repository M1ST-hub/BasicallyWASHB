using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public List<GameObject> players;
    public GameObject[] spawnPoints;
    public GameObject[] endPoints;
    public GameObject[] restartPoints;

    [Header("Timers")]
    public GameObject gameTimer;
    public GameObject preGameTimer;
    public GameObject postGameTimer;
    public GameObject canvas;
    public bool isGameStarted = false;

    private GameObject timmy;
    private GameObject playTime;
    private GameObject endTimer;

    //private PlayerController playerController;
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
        GetPlayers();

        foreach (GameObject player in players)
        {
            Rigidbody rb = player.GetComponentInChildren<Rigidbody>();
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            player.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.localPosition;
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
        GetPlayers();

        foreach (GameObject player in players)
        {
            Rigidbody rb = player.GetComponentInChildren<Rigidbody>();
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            player.transform.position = endPoints[Random.Range(0, endPoints.Length)].transform.localPosition;
        }

        //Destroy(playTime);
        if (IsServer)
            SpawnPostGameTimerRpc();

        Debug.Log("GameEnd");
    }

    [Rpc(SendTo.Everyone)]
    public void GameRestartRpc()
    {
        GetPlayers();

        foreach (GameObject player in players)
        {
            Rigidbody rb = player.GetComponentInChildren<Rigidbody>();
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            player.transform.position = restartPoints[Random.Range(0, restartPoints.Length)].transform.localPosition;
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
            timmy = Instantiate(preGameTimer, canvas.transform);
            timmy.GetComponent<NetworkObject>().Spawn();
            timmy.transform.SetParent(canvas.transform);
        }

    }

    [Rpc(SendTo.Everyone)]
    public void SpawnGameTimerRpc()
    {
        if (IsServer)
        {
            playTime = Instantiate(gameTimer, canvas.transform);
            playTime.GetComponent<NetworkObject>().Spawn();
            playTime.transform.SetParent(canvas.transform);
        }

    }

    [Rpc(SendTo.Everyone)]
    public void SpawnPostGameTimerRpc()
    {
        if (IsServer)
        {
            endTimer = Instantiate(postGameTimer, canvas.transform);
            endTimer.GetComponent<NetworkObject>().Spawn();
            endTimer.transform.SetParent(canvas.transform);
        }
    }



}
