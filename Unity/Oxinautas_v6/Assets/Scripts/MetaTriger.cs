using Unity.Cinemachine;
using UnityEngine;

public class CustomCamera : MonoBehaviour
{
    public CinemachineCamera CinemachineCamera;
    public CinemachinePositionComposer PositionComposer;
    public CinemachinePositionComposer ScreenPosition;
    [SerializeField] private float cameraDistance;
    // [SerializeField] private float screenPositionX;
    // [SerializeField] private float screenPositionY;
    [SerializeField] private AnimationClip metaAnimation;
    private Animation animationComponent;
    
    private void Start()
    {
        PositionComposer = CinemachineCamera.GetComponent<CinemachinePositionComposer>();
        
        // Obtener o agregar el componente Animation
        animationComponent = GetComponent<Animation>();
        if (animationComponent == null)
        {
            animationComponent = gameObject.AddComponent<Animation>();
        }
        
        // Agregar el clip de animación
        if (metaAnimation != null)
        {
            animationComponent.AddClip(metaAnimation, "Meta");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger Entered");
        PositionComposer.CameraDistance = cameraDistance;
        // PositionComposer.Composition.ScreenPosition = new Vector2(screenPositionX, screenPositionY);
        
        // Reproducir la animación
        if (metaAnimation != null && animationComponent != null)
        {
            animationComponent.Play("Meta");
        }
    }
}
