using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.InputSystem;
using UnityEditor.XR;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 4.5f;
    [SerializeField] private float jumpForse = 6.6f;
    [SerializeField] private float moveDirection = -1f;
    [SerializeField] private float wallSlideSpeed = 0.5f;
    [SerializeField] private float maxJumpTime = 0.5f;

    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite firstJumpSprite;
    [SerializeField] private Sprite secondJumpSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform visual;

    private Rigidbody2D rb;


    private float wallCheckDistance = 0.1f;
    private bool isStuckToWall;
    private bool canGrabWall;
    private bool canControlJump;
    private float jumpHoldTime;
    private int maxJumpCharges = 2;
    private int jumpCharges;
    private bool isDoingFlip;
    private float flipTimer;
    

    void Start()
    {
        canGrabWall = true;
        rb = GetComponent<Rigidbody2D>();
        jumpCharges = maxJumpCharges;
    }

    void Update()
    {
        if (canControlJump && Keyboard.current.spaceKey.isPressed)
        {
            jumpHoldTime += Time.deltaTime;
            
            if (jumpHoldTime < maxJumpTime)
            {
                rb.linearVelocity = new Vector2(horizontalSpeed * moveDirection, jumpForse);
            } 
        }

        if (!Keyboard.current.spaceKey.isPressed || jumpHoldTime >= maxJumpTime)
        {
            canControlJump = false;
        }

        if (isDoingFlip)
        {
            flipTimer += Time.deltaTime;

            if (flipTimer >= 0.1f)
            {
                flipTimer = 0f;
                visual.Rotate(0f, 0f, -45f);
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
        CheckWall();
    }

    private void Move()
    {

        if (isStuckToWall)
        {
            rb.linearVelocity = new Vector2(0f, -wallSlideSpeed);
            return;
        }

        //rb.linearVelocity = new Vector2(horizontalSpeed * moveDirection, rb.linearVelocity.y);
    }

    private void CheckWall()
    {
        Vector2 leftDirection = Vector2.left;
        Vector2 rightDirection = Vector2.right;

        RaycastHit2D leftHit = Physics2D.Raycast(transform.position + Vector3.left * 0.46f, leftDirection, wallCheckDistance);
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position + Vector3.right * 0.46f, rightDirection, wallCheckDistance);

        if (!canGrabWall)
        {
            if (leftHit.collider == null && rightHit.collider == null)
            {
                canGrabWall = true;
            }
        }
        
        if (canGrabWall && leftHit.collider != null && leftHit.collider.CompareTag("Wall"))
        {
            isStuckToWall = true;
            isDoingFlip = false;
            visual.localRotation = Quaternion.identity;
            spriteRenderer.sprite = idleSprite;
            jumpCharges = maxJumpCharges;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        if (canGrabWall && rightHit.collider != null && rightHit.collider.CompareTag("Wall"))
        {
            isStuckToWall = true;
            isDoingFlip = false;
            visual.localRotation = Quaternion.identity;
            spriteRenderer.sprite = idleSprite;
            jumpCharges = maxJumpCharges;
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            ContactPoint2D contact = collision.GetContact(0);
            bool hitFromSide = Mathf.Abs(contact.normal.x) > Mathf.Abs(contact.normal.y);

            if (hitFromSide)
            {
                isStuckToWall = true;
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("CollisionExit: отцепился");
            isStuckToWall = false;
        }
    }

    private void Jump()
    {
        if (jumpCharges <= 0)
        {
            return;
        }

        if (jumpCharges == 2)
        {
            spriteRenderer.sprite = firstJumpSprite;
        }

        else if (jumpCharges == 1)
        {
            spriteRenderer.sprite = secondJumpSprite;
            isDoingFlip = true;
        }

        jumpHoldTime = 0f;
        canControlJump = true;

        isStuckToWall = false;
        canGrabWall = false;

        rb.linearVelocity = new Vector2(horizontalSpeed * moveDirection, jumpForse);

        moveDirection *= -1f;
        transform.localScale = new Vector3(moveDirection, 1f, 1f);

        jumpCharges--;
    }
    
    private void OnJump()
    {
        Jump();
    }
}
