using Unity.Cinemachine;
using UnityEngine;

public class CustomCamera : MonoBehaviour
{
    public CinemachineCamera CinemachineCamera;
    public CinemachinePositionComposer PositionComposer;
    private void Start()
    {
        PositionComposer = CinemachineCamera.GetComponent<CinemachinePositionComposer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger Entered");
        PositionComposer.CameraDistance = 4f;
    }
}
