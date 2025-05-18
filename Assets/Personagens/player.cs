using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class player : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 25f;
    private bool isFacingRight = true;
    private bool controlsInverted = false;
    private Coroutine invertCoroutine;

    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Rigidbody2D rb;
    private Transform groundCheck;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector3 groundCheckOffset = new Vector3(0f, -1.5f, 0f);

    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (groundCheck == null)
        {
            GameObject gc = new GameObject("GroundCheck");
            gc.transform.parent = transform;
            gc.transform.localPosition = groundCheckOffset;
            groundCheck = gc.transform;
        }

        gameOverUI.SetActive(false);
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        if (controlsInverted) horizontal *= -1;

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        Flip();

        if (Input.GetKeyDown(KeyCode.B) && gameOverUI.activeSelf)
        {
            RestartGame();
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if ((isFacingRight && horizontal < 0f) || (!isFacingRight && horizontal > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + groundCheckOffset, 0.2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Voide"))
        {
            TriggerGameOver();
        }

        if (collision.CompareTag("item"))
        {
            InvertControls(Mathf.Infinity); // inversão infinita
            Destroy(collision.gameObject);
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    private void TriggerGameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("Game Over!");
    }

    // === Inversão de Controles ===
    public void InvertControls(float duration)
    {
        if (invertCoroutine != null)
            StopCoroutine(invertCoroutine);

        controlsInverted = true;

        if (!float.IsInfinity(duration))
        {
            invertCoroutine = StartCoroutine(InvertControlsCoroutine(duration));
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowMessage("Controles invertidos!", duration);
        }
    }

    private IEnumerator InvertControlsCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        controlsInverted = false;
    }
}
