using Unity.Cinemachine;
using UnityEngine;

public class MetaTrigger : MonoBehaviour
{
    // [Header("Cinemachine Components")]
    // public CinemachineCamera CinemachineCamera;
    // public CinemachinePositionComposer PositionComposer;
    // public CinemachinePositionComposer ScreenPosition;
    // [SerializeField] private float cameraDistance;
    // [SerializeField] private float screenPositionX;
    // [SerializeField] private float screenPositionY;
    [Header("Meta Animation")]
    [SerializeField] private string metaTag = "Meta";
    [SerializeField] private GameObject meta;
    [SerializeField] private Animator animatorMeta;
    [SerializeField] private string animatorBool = "Meta";
    private int _idMeta;
    
    
    private void Start()
    {
        // PositionComposer = CinemachineCamera.GetComponent<CinemachinePositionComposer>();

        if (meta == null)
        {
            meta = GameObject.FindGameObjectWithTag(metaTag);
        }

        if (meta == null)
        {
            Debug.LogError($"MetaTrigger could not find a GameObject with tag '{metaTag}'.");
            enabled = false;
            return;
        }

        animatorMeta = meta.GetComponent<Animator>();
        if (animatorMeta == null)
        {
            Debug.LogError("MetaTrigger requires an Animator on the meta GameObject.");
            enabled = false;
            return;
        }

        _idMeta = Animator.StringToHash(animatorBool);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger Entered");
        // PositionComposer.CameraDistance = cameraDistance;
        // PositionComposer.Composition.ScreenPosition = new Vector2(screenPositionX, screenPositionY);
        
        // Activar animación de meta cuando el player entra
        if (collision.CompareTag("Player"))
        {
            animatorMeta.SetBool(_idMeta, true);
        }
    }
    public void MetaFinal()
    {
        animatorMeta.SetBool(_idMeta, true);
    }
}
