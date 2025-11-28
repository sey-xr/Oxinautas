using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private PlayerController _playerController;
    public PlayerController PlayerControllerlayerController { get { return _playerController; } }

    [SerializeField] private int _oxygenCollected;
    public int OxygenCollected { get => _oxygenCollected; }

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }
    public void AddOxygen() => _oxygenCollected++;
}