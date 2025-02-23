using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform spawnPoint;
    public AudioClip checkpointSound;
    public GameObject effectPrefab;
    private AudioSource audioSource;
    private bool activated = false;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activated)
        {
            ActivateCheckpoint(other.gameObject);
        }
    }

    private void ActivateCheckpoint(GameObject player)
    {
        activated = true;


        if (checkpointSound != null)
        {
            audioSource.PlayOneShot(checkpointSound);
        }


        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
        }


        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.SetCheckpoint(spawnPoint.position);
        }
    }
}