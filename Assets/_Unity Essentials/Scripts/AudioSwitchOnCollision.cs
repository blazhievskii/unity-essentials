using UnityEngine;

public class AudioSwitchOnCollision : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip newClip; 

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (audioSource != null && newClip != null)
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }
}