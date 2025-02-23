using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float boostSpeed = 10.0f; // Ускоренная скорость
    public float boostDuration = 3.0f; // Длительность ускорения
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
    private bool isBoosted = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        LoadCheckpoint();
    }

    private void Update()
    {
        if (isResetting) return;

        if (!IsFlipped() && Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Проверка на переворот
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
        if (IsFlipped()) return;

        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = transform.forward * moveVertical * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        float turn = Input.GetAxis("Horizontal") * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.contactCount > 0)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Если вошли в триггер ускорения
        if (other.CompareTag("SpeedBoost"))
        {
            StartCoroutine(SpeedBoost());
        }
        // Если вошли в "опасную зону"
        else if (other.CompareTag("ExplosionZone")) // <-- Укажите нужный вам тэг
        {
            // Вызовем ту же логику, что и при перевороте (взрыв + респаун),
            // но без ожидания flipCheckTime
            StartCoroutine(ExplodeAndRespawn());
        }
    }

    private IEnumerator SpeedBoost()
    {
        if (isBoosted) yield break;
        isBoosted = true;

        float originalSpeed = speed;
        speed = boostSpeed;
        yield return new WaitForSeconds(boostDuration);

        speed = originalSpeed;
        isBoosted = false;
    }

    // Проверяем, перевернут ли игрок
    private bool IsFlipped()
    {
        float angleX = Mathf.Abs(transform.eulerAngles.x);
        float angleZ = Mathf.Abs(transform.eulerAngles.z);

        return (angleX > flipThreshold && angleX < 360 - flipThreshold) ||
               (angleZ > flipThreshold && angleZ < 360 - flipThreshold);
    }

    // Корутин, который ждёт немного времени, чтобы убедиться, что персонаж всё ещё перевёрнут
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

    // Тот же механизм, но вызывается сразу при попадании в опасную зону (без ожидания flipCheckTime)
    private IEnumerator ExplodeAndRespawn()
    {
        if (isResetting) yield break;
        isResetting = true;

        Debug.Log("❌ Персонаж подорвался в опасной зоне! Взрыв и респаун...");

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
