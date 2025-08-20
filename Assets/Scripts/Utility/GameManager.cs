using System;
using System.Collections;
using System.Linq;
using NodeCanvas.Tasks.Actions;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    [SerializeField] private GameObject playerPrefab;
    public GameObject playerInstance; // reference for the current target.
    [SerializeField] private GameObject zombiePrefab;
    public static List<GameObject> zombieInstances = new List<GameObject>();
    private int currentScore = 0;
    private bool GameIsOn = false;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private GameObject[] spawnpoints;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        spawnpoints = GameObject.FindGameObjectsWithTag("Spawnpoint");
    }
    public void HelloWorld()
    {
        Debug.Log("GM says hello world");
    }

    void Update()
    {
        // Debugging keys for game loop.
        if (Input.GetKeyDown(KeyCode.O))
        {
            StartGame();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameIsOn = false;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            SpawnZombie();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            SpawnZombie();
        }
    }

    // enemies can receive the player as target with this function
    public GameObject GetPlayer()
    {
        if (playerInstance != null)
        {
            return playerInstance;
        }
        else
        {
            Debug.LogWarning("No player instance on scene");
            return null;
        }
    }

    // when menu "play" button is pressed, init game stats.
    public void StartGame()
    {
        print("starting the game");
        cinemachineCamera.Follow = null;
        if (playerInstance != null)
        {
            Destroy(playerInstance); // remove possible player from the scene.
        }
        foreach (GameObject zombieInstance in zombieInstances) {
            Destroy(zombieInstance);
        }
        // spawn player using random spawnpoint
        SpawnPlayer();
        // init score
        currentScore = 0;
        StartCoroutine(GameLoop());
    }
    IEnumerator GameLoop()
    {
        print("We are at the game loop");
        GameIsOn = true;
        while (GameIsOn) // keep the GameLoop until player dead and/or GameEnded. 
        {
            yield return null;
        }
        GameEnded();
    } // spawning zombies loop etc.

    // calculate end score, show scorehud and menu buttons. 
    public void GameEnded()
    {
        print("Game ended");
        cinemachineCamera.Follow = null; // add zoom in effect into player last position.
        // Scoreboard.
        // calculate and show end score.
        // if highscore, set new highscore
        // show "NEW HIGH SCORE" sprite with tween animation
        // save the highscore
    }

    // Called from health script, after health 0 or less.
    public void PlayerDead()
    {
        GameIsOn = false;
    }

    public void SpawnPlayer()
    {
        playerInstance = Instantiate(playerPrefab, GetRandomSpawnPointPosition(), quaternion.identity);
        cinemachineCamera.Follow = playerInstance.transform;
    }

    public void SpawnZombie()
    {
        if (GameIsOn)
        {
            GameObject zombieInstance = Instantiate(zombiePrefab, GetRandomSpawnPointPosition(), CalculateRotationTowardsPlayer());
            zombieInstances.Add(zombieInstance); // track all zombies in the scene, easy to remove at the next round.
        }
    }

    public Vector3 GetRandomSpawnPointPosition()
    {
        if (spawnpoints.Length > 0)
        {
            return spawnpoints[UnityEngine.Random.Range(0, spawnpoints.Length)].transform.position; // return random transform position from the spawnpoint arraylist.
        }
        else
        {
            Debug.LogWarning("No spawnpoint found at the scene, returned zero -position");
            return Vector3.zero;
        }
    }

    Quaternion CalculateRotationTowardsPlayer() {
        return Quaternion.identity;
    }

}
