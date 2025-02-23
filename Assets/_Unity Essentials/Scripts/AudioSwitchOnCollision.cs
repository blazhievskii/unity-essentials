using UnityEngine;

public class AudioSwitchOnCollision : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip newClip;
    private bool hasSwitched = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (audioSource != null && newClip != null && !hasSwitched)
        {
            audioSource.clip = newClip;
            audioSource.Play();
            hasSwitched = true;
        }
    }
}