using System;
using UnityEngine;
using UnityEngine.Serialization;

public class DeadArea : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    // Start is called once before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) player.Die();
    }
}
