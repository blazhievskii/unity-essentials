using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 120.0f;
    public float jumpForce = 5.0f;

    public float flipThreshold = 120f;
    public float flipCheckTime = 2.0f;
    public float resetDelay = 1.5f;

    public GameObject explosionEffect;

    private Rigidbody rb;
    private Vector3 lastCheckpoint;
    private bool isResetting = false;
    private float flipStartTime = 0f;
    private bool isFlipping = false;
    private bool isGrounded = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        LoadCheckpoint();
    }

    private void Update()
    {
        if (isResetting) return;

        // Прыжок выполняется только если персонаж не перевёрнут
        if (!IsFlipped() && Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Если персонаж перевёрнут, запускаем процесс обработки переворота (взрыв и респаун)
        if (IsFlipped())
        {
            if (!isFlipping)
            {
                isFlipping = true;
                flipStartTime = Time.time;
                StartCoroutine(WaitBeforeReset());
            }
        }
        else
        {
            isFlipping = false;
        }
    }

    private void FixedUpdate()
    {
        if (isResetting) return;

        // Если персонаж перевёрнут, не обрабатываем движение и поворот
        if (IsFlipped())
            return;

        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = transform.forward * moveVertical * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        float turn = Input.GetAxis("Horizontal") * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    private void OnCollisionStay(Collision collision)
    {
        // Устанавливаем isGrounded в true, если персонаж касается любой поверхности
        if (collision.contactCount > 0)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Как только персонаж перестаёт касаться поверхности, сбрасываем isGrounded
        isGrounded = false;
    }

    private bool IsFlipped()
    {
        float angleX = Mathf.Abs(transform.eulerAngles.x);
        float angleZ = Mathf.Abs(transform.eulerAngles.z);

        return (angleX > flipThreshold && angleX < 360 - flipThreshold) ||
               (angleZ > flipThreshold && angleZ < 360 - flipThreshold);
    }

    private IEnumerator WaitBeforeReset()
    {
        while (Time.time - flipStartTime < flipCheckTime)
        {
            if (!IsFlipped())
                yield break;
            yield return null;
        }

        StartCoroutine(HandleFlip());
    }

    private IEnumerator HandleFlip()
    {
        if (isResetting) yield break;
        isResetting = true;

        Debug.Log("❌ Персонаж перевернулся! Взрыв и респаун...");

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(resetDelay);

        Respawn();
    }

    private void Respawn()
    {
        Debug.Log("🔄 Персонаж возвращён в чекпоинт.");
        transform.position = lastCheckpoint;
        transform.rotation = Quaternion.identity;
        rb.isKinematic = false;
        GetComponent<Collider>().enabled = true;
        isResetting = false;
        isFlipping = false;
    }

    public void SetCheckpoint(Vector3 checkpoint)
    {
        lastCheckpoint = checkpoint;
        PlayerPrefs.SetFloat("SpawnX", checkpoint.x);
        PlayerPrefs.SetFloat("SpawnY", checkpoint.y);
        PlayerPrefs.SetFloat("SpawnZ", checkpoint.z);
        PlayerPrefs.Save();
    }

    private void LoadCheckpoint()
    {
        if (PlayerPrefs.HasKey("SpawnX"))
        {
            lastCheckpoint = new Vector3(
                PlayerPrefs.GetFloat("SpawnX"),
                PlayerPrefs.GetFloat("SpawnY"),
                PlayerPrefs.GetFloat("SpawnZ")
            );
        }
        else
        {
            lastCheckpoint = transform.position;
        }
    }
}
