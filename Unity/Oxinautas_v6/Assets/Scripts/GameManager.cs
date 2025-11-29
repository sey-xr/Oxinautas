using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Player Settings")]
    [SerializeField] private PlayerController playerPrefab;
    [SerializeField] private Transform playerRespawnPoint;
    [SerializeField] private float respawnPlayerDelay;
    [SerializeField] private PlayerController playerController;
    
    [Header("Oxygen Settings")]
    [SerializeField] private int _oxygenCollected;
    [SerializeField] private int totalOxygen;
    public int OxygenCollected { get => _oxygenCollected; }

    public PlayerController PlayerControllerlayerController => playerController;



    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void Start()
    {
        totalOxygenInLevel();
    }

    private void totalOxygenInLevel()
    {
        GameObject[] oxygens = GameObject.FindGameObjectsWithTag("Oxygen");
        totalOxygen = oxygens.Length;
    }

    public void RespawnPlayer() => StartCoroutine(RespawnPlayerCoroutine());

    IEnumerator RespawnPlayerCoroutine()
    {
        yield return new WaitForSeconds(respawnPlayerDelay);
        GameObject newPlayer = Instantiate(playerPrefab.gameObject, playerRespawnPoint.position, Quaternion.identity);
        newPlayer.name = "Player";
        playerController = newPlayer.GetComponent<PlayerController>();
    }
    public void AddOxygen() => _oxygenCollected++;
}