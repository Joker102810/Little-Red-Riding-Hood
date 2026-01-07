using System.Collections;
using UnityEngine;

public class Cutscenes : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public AudioSource audioSource;
    private bool firstPickup;

    [Header("Mother")]
    public AudioClip mother1;
    public AudioClip mother2;
    public AudioClip mother3;

    [Header("Little Red Riding Hood")]
    public AudioClip littleRedRidingHood1;
    public AudioClip littleRedRidingHood2;
    public AudioClip littleRedRidingHood3;
    public AudioClip littleRedRidingHood4;
    public AudioClip littleRedRidingHood5;
    public AudioClip littleRedRidingHood6;
    public AudioClip littleRedRidingHood7;
    public AudioClip littleRedRidingHood8;

    [Header("Wolf")]
    public AudioClip wolf1;
    public AudioClip wolf2;
    public AudioClip wolf3;
    public AudioClip wolf4;
    public AudioClip wolf5;
    public AudioClip wolf6;
    public AudioClip wolf7;

    [Header("Lumberjack")]
    public AudioClip lumberjack1;
    public AudioClip lumberjack2;

    [Header("Grandmother")]
    public AudioClip grandmother1;
    public AudioClip grandmother2;
    public AudioClip grandmother3;

    public void StartGame()
    {
        StartCoroutine(Dialogue(mother1));
        StartCoroutine(Dialogue(mother2));
    }

    IEnumerator Mother()
    {
        dialogueManager.StartMotherMoving();
        yield return new WaitUntil(() => !dialogueManager.isMotherMoving);
    }

    IEnumerator Wolf()
    {
        dialogueManager.StartWolfMoving();
        yield return new WaitUntil(() => !dialogueManager.isWolfMoving);
    }

    IEnumerator Lumberjack()
    {
        dialogueManager.StartLumberjackMoving();
        yield return new WaitUntil(() => !dialogueManager.isLumberjackMoving);
    }

    IEnumerator Grandmother()
    {
        dialogueManager.StartGrandmotherMoving();
        yield return new WaitUntil(() => !dialogueManager.isGrandmotherMoving);
    }

    IEnumerator Dialogue(AudioClip clip)
    {
        dialogueManager.isCutscene = true;
        audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        dialogueManager.isCutscene = false;
    }

    public void BasketInteraction()
    {
        StartCoroutine(Dialogue(mother3));
    }

    public void FirstPickup()
    {
        if (firstPickup == false)
        {
            audioSource.PlayOneShot(littleRedRidingHood1);
            firstPickup = true;
        }
        else
        {
            return;
        }
    }
}