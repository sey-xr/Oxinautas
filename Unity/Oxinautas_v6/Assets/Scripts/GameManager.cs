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

    public PlayerController PlayerControllerlayerController => playerController;

    [SerializeField] private int _oxygenCollected;
    public int OxygenCollected { get => _oxygenCollected; }

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
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