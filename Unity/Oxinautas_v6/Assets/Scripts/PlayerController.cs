using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Animator Hash IDs
    private int idIsGrounded;
    private int idSpeed;
    private int idIsWallDetected;
    private int idKnockback;
    private int idDoubleJump;

    [Header("Components")]
    [SerializeField] private Transform m_transform;
    private Rigidbody2D m_rigidbody2D;
    private GatherInput m_gatherInput;
    private Animator m_animator;

    [Header("Movement Settings")]
    [SerializeField] private float speed;
    private int direction = 1;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private int extraJumps;
    [SerializeField] private int counterExtraJumps;
    [SerializeField] private bool canDoubleJump;

    [Header("Ground Settings")]
    [SerializeField] private Transform lFoot;
    [SerializeField] private Transform rFoot;
    RaycastHit2D rFootRay;
    RaycastHit2D lFootRay;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float rayLength;
    [SerializeField] private LayerMask groundLayer;

    [Header("Wall Settings")]
    [SerializeField] private float checkWallDistance;
    [SerializeField] private bool isWallDetected;
    // [SerializeField] private bool canWallSlide;
    // [SerializeField] private float slideSpeed;
    // [SerializeField] private Vector2 wallJumpForce;
    // [SerializeField] private bool isWallJumping;
    // [SerializeField] private float wallJumpDuration;


    [Header("Knock Settings")]
    [SerializeField] private bool isKnocked;
    [SerializeField] private bool canBeKnocked;
    [SerializeField] private Vector2 knockPower;
    [SerializeField] private float knockedDuration;

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
        idIsWallDetected = Animator.StringToHash("isWallDetected");
        idKnockback = Animator.StringToHash("knockback");
        idDoubleJump = Animator.StringToHash("canDoubleJump");
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
        if(isKnocked) return;
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
        // else if (m_gatherInput.ValueX * direction > 0)
        // {
        //     m_transform.localScale = new Vector3(m_transform.localScale.x, 1, 1);
        //     direction *= 1;
        // }
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
    private void DoubleJump()
    {
        m_animator.SetTrigger(idDoubleJump);
        // TODO: Double Jump Logic
        //m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.ValueX, jumpForce);
    }
    public void Knockback()
    {
        StartCoroutine(KnockbackRoutine());
        m_rigidbody2D.linearVelocity = new Vector2(knockPower.x * -direction, knockPower.y);
        m_animator.SetTrigger(idKnockback);
    }

    private IEnumerator KnockbackRoutine()
    {
        isKnocked = true;
        canBeKnocked = false;
        yield return new WaitForSeconds(knockedDuration);
        isKnocked = false;
        canBeKnocked = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(m_transform.position, new Vector2(m_transform.position.x + checkWallDistance * direction, m_transform.position.y));
    }
}