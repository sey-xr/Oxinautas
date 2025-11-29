using Unity.Cinemachine;
using UnityEngine;

public class MetaEvent : MonoBehaviour
{
    [Header("Meta Animation")]
    [SerializeField] private GameObject meta;
    [SerializeField] private Animator animatorMeta;
    private int _idMeta;
    
    private void OnEnable()
    {
        _idMeta = Animator.StringToHash("Meta");
        animatorMeta = meta.GetComponent<Animator>();
    }
    public void MetaFinal()
    {
        animatorMeta.SetBool(_idMeta, true);
    }
}
