using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class VRPlayerAnimation : MonoBehaviour
{
    [Header("Animation & Input")]
    public Animator playerAnim;
    public InputActionProperty moveAction;
    public DialogueManager dialogueManager;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip walkingIndoors;
    public AudioClip walkingOutdoors;

    [Header("Forest Ambience")]
    public AudioSource forestAmbienceSource;
    public AudioClip forestAmbience;

    private bool isOutdoors;
    private bool wasWalking;
    private bool lastOutdoorsState;
    private Coroutine fadeRoutine;
    private Coroutine ambienceFadeRoutine;

    void Start()
    {
        if (!audioSource)
            audioSource = GetComponent<AudioSource>();

        if (!playerAnim)
            playerAnim = GetComponent<Animator>();

        audioSource.loop = true;
        audioSource.volume = 1f;

        if (forestAmbienceSource != null && forestAmbience != null)
        {
            forestAmbienceSource.clip = forestAmbience;
            forestAmbienceSource.loop = true;
            forestAmbienceSource.volume = 0f;
        }
    }

    void OnEnable()
    {
        if (moveAction != null)
            moveAction.action.Enable();
    }

    void OnDisable()
    {
        if (moveAction != null)
            moveAction.action.Disable();
    }

    void LateUpdate()
    {
        UpdateGroundType();
        UpdateAmbience();

        if (dialogueManager != null && (dialogueManager.isCutscene || dialogueManager.isMoving))
        {
            StopWalking();
            return;
        }

        Vector2 input = moveAction.action.ReadValue<Vector2>();
        bool isWalking = input.magnitude >= 0.1f;

        playerAnim.SetBool("walk", isWalking);

        if (isWalking && !wasWalking)
            StartWalking();
        else if (!isWalking && wasWalking)
            StopWalking();

        if (isWalking && isOutdoors != lastOutdoorsState)
            StartWalking();

        lastOutdoorsState = isOutdoors;
        wasWalking = isWalking;
    }

    void UpdateGroundType()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f))
        {
            int hitLayer = hit.collider.gameObject.layer;

            if (hitLayer == LayerMask.NameToLayer("Outdoors"))
                isOutdoors = true;
            else if (hitLayer == LayerMask.NameToLayer("Indoors"))
                isOutdoors = false;
        }
    }

    void UpdateAmbience()
    {
        if (forestAmbienceSource == null) return;

        if (isOutdoors)
        {
            if (!forestAmbienceSource.isPlaying)
            {
                forestAmbienceSource.Play();
                if (ambienceFadeRoutine != null)
                    StopCoroutine(ambienceFadeRoutine);
                ambienceFadeRoutine = StartCoroutine(FadeIn(forestAmbienceSource));
            }
        }
        else
        {
            if (forestAmbienceSource.isPlaying)
            {
                if (ambienceFadeRoutine != null)
                    StopCoroutine(ambienceFadeRoutine);
                ambienceFadeRoutine = StartCoroutine(FadeOut(forestAmbienceSource));
            }
        }
    }

    void StartWalking()
    {
        AudioClip targetClip = isOutdoors ? walkingOutdoors : walkingIndoors;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        if (audioSource.clip != targetClip)
        {
            fadeRoutine = StartCoroutine(CrossFade(targetClip));
        }
        else if (!audioSource.isPlaying)
        {
            fadeRoutine = StartCoroutine(FadeIn(audioSource));
        }
    }

    void StopWalking()
    {
        playerAnim.SetBool("walk", false);

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeOut(audioSource));
        wasWalking = false;
    }

    IEnumerator FadeIn(AudioSource source)
    {
        source.volume = 0f;
        source.Play();

        while (source.volume < 1f)
        {
            source.volume += Time.deltaTime / 0.25f;
            yield return null;
        }

        source.volume = 1f;
    }

    IEnumerator FadeOut(AudioSource source)
    {
        float startVolume = source.volume;

        while (source.volume > 0f)
        {
            source.volume -= Time.deltaTime / 0.25f;
            yield return null;
        }

        source.Stop();
        source.volume = startVolume;
    }

    IEnumerator CrossFade(AudioClip newClip)
    {
        while (audioSource.volume > 0f)
        {
            audioSource.volume -= Time.deltaTime / 0.25f;
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.Play();

        while (audioSource.volume < 1f)
        {
            audioSource.volume += Time.deltaTime / 0.25f;
            yield return null;
        }

        audioSource.volume = 1f;
    }
}