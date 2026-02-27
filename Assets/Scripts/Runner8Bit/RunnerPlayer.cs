using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Runner8Bit
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class RunnerPlayer : MonoBehaviour
    {
        [SerializeField] private float jumpForce = 11f;
        [SerializeField] private float visualLerp = 10f;

        private Rigidbody2D rb;
        private bool grounded;
        private Vector3 baseScale;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            baseScale = transform.localScale;
        }

        private void Update()
        {
            var manager = RunnerGameManager.Instance;
            if (manager == null || !manager.IsPlaying) return;

            if (IsJumpPressed() && grounded)
            {
                rb.linearVelocity = new Vector2(0f, jumpForce);
            }

            if (transform.position.y < -8f)
            {
                manager.Lose("You fell off the stage");
            }

            var targetScale = baseScale;
            if (!grounded)
            {
                targetScale = rb.linearVelocity.y > 0.1f
                    ? new Vector3(baseScale.x * 0.88f, baseScale.y * 1.12f, 1f)
                    : new Vector3(baseScale.x * 1.16f, baseScale.y * 0.86f, 1f);
            }

            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, visualLerp * Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.GetComponent<RunnerGround>() != null)
            {
                grounded = true;
            }

            if (collision.collider.GetComponent<RunnerObstacle>() != null)
            {
                var manager = RunnerGameManager.Instance;
                if (manager != null)
                {
                    manager.Lose("You hit an obstacle");
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.GetComponent<RunnerGround>() != null)
            {
                grounded = false;
            }
        }

        private static bool IsJumpPressed()
        {
#if ENABLE_INPUT_SYSTEM
            var keyboard = Keyboard.current;
            if (keyboard == null) return false;
            return keyboard.spaceKey.wasPressedThisFrame || keyboard.upArrowKey.wasPressedThisFrame;
#else
            return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);
#endif
        }
    }
}
