using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D m_rigidbody2D;
    private GatherInput m_gatherInput;
    [SerializeField] private Transform m_transform;
    private Animator m_animator;

    [Header("Movement Settings")]
    [SerializeField] private float speed;
    private int direction = 1;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private int extraJumps;
    [SerializeField] private int counterExtraJumps;
    [SerializeField] private bool canDoubleJump;
    private int idSpeed;

    [Header("Ground Settings")]
    [SerializeField] private Transform lFoot;
    [SerializeField] private Transform rFoot;
    RaycastHit2D rFootRay;
    RaycastHit2D lFootRay;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float rayLength;
    [SerializeField] private LayerMask groundLayer;
    private int idIsGrounded;

    [Header("Wall Settings")]
    [SerializeField] private float checkWallDistance;
    [SerializeField] private bool isWallDetected;

    void Awake()
    {
        m_gatherInput = GetComponent<GatherInput>();
        //m_transform = GetComponent<Transform>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();  
    }
    
    // Start is called before the first frame update
    void Start()
    {
        idSpeed = Animator.StringToHash("Speed");
        idIsGrounded = Animator.StringToHash("isGrounded");
        rFoot = GameObject.Find("PieD").GetComponent<Transform>();
        lFoot = GameObject.Find("PieI").GetComponent<Transform>();
        counterExtraJumps = extraJumps;
    }
    private void Update()
    {
        // Animations
        SetAnimatorValues();
    }
    private void SetAnimatorValues()
    {
        m_animator.SetFloat(idSpeed, Mathf.Abs(m_rigidbody2D.linearVelocityX));
        m_animator.SetBool(idIsGrounded, isGrounded);
    }
    void FixedUpdate()
    {
        CheckCollision();
        Move();
        Jump();
    }

    private void CheckCollision()
    {
        HandleGround();
        HandleWall();
    }

    private void HandleWall()
    {
        isWallDetected = Physics2D.Raycast(m_transform.position, Vector2.right * direction, checkWallDistance, groundLayer);
    }

    private void HandleGround()
    {
        rFootRay = Physics2D.Raycast(rFoot.position, Vector2.down, rayLength, groundLayer);
        lFootRay = Physics2D.Raycast(lFoot.position, Vector2.down, rayLength, groundLayer);
        if (rFootRay|| lFootRay)
        {
            isGrounded = true;
            counterExtraJumps = extraJumps;
            canDoubleJump = false;
        }
        else
        {
            isGrounded = false;
        }
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
    private void Jump()
    {
        if (m_gatherInput.IsJumping)
        {
            if (isGrounded)
            {
                m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.ValueX, jumpForce);
                canDoubleJump = true;
            }
            else if(counterExtraJumps>0 && canDoubleJump)
            {
                m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.ValueX, jumpForce);
                counterExtraJumps-=1;
            }
        }
        m_gatherInput.IsJumping = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(m_transform.position, new Vector2(m_transform.position.x + checkWallDistance * direction, m_transform.position.y));
        Gizmos.color = Color.red;
    }
}