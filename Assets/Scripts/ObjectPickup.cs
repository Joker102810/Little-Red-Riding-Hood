using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip pickup;
    public AudioClip putdown;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Pickup()
    {
        audioSource.PlayOneShot(pickup);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 1)
        {
            audioSource.PlayOneShot(putdown);
        }
    }
}
