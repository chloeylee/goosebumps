using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float inputSmoothTime = 0.05f;

    [Header("Dash")]
    public float dashSpeed = 30f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public bool isInvincibleDuringDash = true;

    private Vector2 moveInputRaw;
    private Vector2 smoothedInput;
    private Vector2 currentVelocity;

    private bool isDashing = false;
    private float dashTimeLeft;
    private float lastDashTime;
    private bool isInvincible = false;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Update()
    {
        // raw input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInputRaw = new Vector2(moveX, moveY).normalized;

        // smooth the input
        smoothedInput = Vector2.SmoothDamp(smoothedInput, moveInputRaw, ref currentVelocity, inputSmoothTime);

        // check for dash input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isDashing && Time.time >= lastDashTime + dashCooldown && moveInputRaw != Vector2.zero)
            {
                StartDash();
            }
        }

        // apply movement
        if (isDashing)
        {
            transform.Translate(smoothedInput * dashSpeed * Time.deltaTime);
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0f)
            {
                EndDash();
            }
        }
        else
        {
            transform.Translate(smoothedInput * moveSpeed * Time.deltaTime);
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        lastDashTime = Time.time;

        if (isInvincibleDuringDash)
        {
            SetInvincibility(true);
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.green;
        }
    }

    void EndDash()
    {
        isDashing = false;

        if (isInvincibleDuringDash)
        {
            SetInvincibility(false);
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }

    void SetInvincibility(bool value)
    {
        isInvincible = value;
        Debug.Log("Invincibility: " + value);
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }
}