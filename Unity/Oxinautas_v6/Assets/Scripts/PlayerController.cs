using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D m_rigidbody2D;
    private GatherInput m_gatherInput;
    private Transform m_transform;
    private Animator m_animator;
    [SerializeField] private float speed;
    private int direction = 1;
    void Start()
    {
        m_gatherInput = GetComponent<GatherInput>();
        m_transform = GetComponent<Transform>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
    }
    private void Update()
    {
        // Animations
        SetAnimatorValues();
    }
    private void SetAnimatorValues()
    {        
        m_animator.SetFloat("Speed", Mathf.Abs(m_rigidbody2D.linearVelocityX));
    }
    void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        // Método para comprobar si está girado el player
        Flip();
        // Linear movement 30fps
        m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.ValueX, m_rigidbody2D.linearVelocityY);
    }
    private void Flip()
    {
        if (m_gatherInput.ValueX * direction < 0)
        {
            m_transform.localScale = new Vector3(-m_transform.localScale.x, 1, 1);
            direction *= -1;
        }
        else if (m_gatherInput.ValueX * direction > 0)
        {
            m_transform.localScale = new Vector3(m_transform.localScale.x, 1, 1);
            direction *= 1;
        }
    }
}
