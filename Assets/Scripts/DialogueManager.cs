using UnityEngine;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public GameObject move;

    [HideInInspector]
    public bool isCutscene;
    [HideInInspector]
    public bool isMoving;

    [HideInInspector]
    public bool isMotherMoving;
    [HideInInspector]
    public bool isWolfMoving;
    [HideInInspector]
    public bool isLumberjackMoving;
    [HideInInspector]
    public bool isGrandmotherMoving;

    private bool motherAtWaypoint;
    private bool wolfAtWaypoint;
    private bool lumberjackAtWaypoint;
    private bool grandmotherAtWaypoint;

    private int motherWaypointIndex;
    private int wolfWaypointIndex;
    private int lumberjackWaypointIndex;
    private int grandmotherWaypointIndex;

    [Header("Mother")]
    public Transform mother;
    public Animator motherAnim;
    public List<Transform> motherWaypoints = new List<Transform>();

    [Header("Wolf")]
    public Transform wolf;
    public Animator wolfAnim;
    public List<Transform> wolfWaypoints = new List<Transform>();

    [Header("Lumberjack")]
    public Transform lumberjack;
    public Animator lumberjackAnim;
    public List<Transform> lumberjackWaypoints = new List<Transform>();

    [Header("Grandmother")]
    public Transform grandmother;
    public Animator grandmotherAnim;
    public List<Transform> grandmotherWaypoints = new List<Transform>();

    void Start()
    {
        motherAnim = GetComponentInChildren<Animator>();
        wolfAnim = GetComponentInChildren<Animator>();
        lumberjackAnim = GetComponentInChildren<Animator>();
        grandmotherAnim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        isMoving = (isMotherMoving || isWolfMoving || isLumberjackMoving || isGrandmotherMoving);

        if (isMotherMoving)
        {
            isMotherMoving = MoveTowardsWaypoint(mother, motherWaypoints, ref motherWaypointIndex, ref motherAtWaypoint, ref isMotherMoving, 1);
        }

        if (isWolfMoving)
        {
            isWolfMoving = MoveTowardsWaypoint(wolf, wolfWaypoints, ref wolfWaypointIndex, ref wolfAtWaypoint, ref isWolfMoving, 1);
        }

        if (isLumberjackMoving)
        {
            isLumberjackMoving = MoveTowardsWaypoint(lumberjack, lumberjackWaypoints, ref lumberjackWaypointIndex, ref lumberjackAtWaypoint, ref isLumberjackMoving, 1);
        }

        if (isGrandmotherMoving)
        {
            isGrandmotherMoving = MoveTowardsWaypoint(grandmother, grandmotherWaypoints, ref grandmotherWaypointIndex, ref grandmotherAtWaypoint, ref isGrandmotherMoving, 1);
        }

        wolfAnim.SetBool("wolfWalk", isWolfMoving);
        grandmotherAnim.SetBool("grandmotherWalk", isGrandmotherMoving);

        if (isCutscene == true || isMoving)
        {
            move.SetActive(false);
        }
        else
        {
            move.SetActive(true);
        }
    }

    public void StartMotherMoving()
    {
        if (motherWaypointIndex < motherWaypoints.Count)
        {
            motherAtWaypoint = false;
            StartMoving(ref isMotherMoving);
        }
    }

    public void StartWolfMoving()
    {
        if (wolfWaypointIndex < wolfWaypoints.Count)
        {
            wolfAtWaypoint = false;
            StartMoving(ref isWolfMoving);
        }
    }

    public void StartLumberjackMoving()
    {
        if (lumberjackWaypointIndex < lumberjackWaypoints.Count)
        {
            lumberjackAtWaypoint = false;
            StartMoving(ref isLumberjackMoving);
        }
    }
    public void StartGrandmotherMoving()
    {
        if (grandmotherWaypointIndex < grandmotherWaypoints.Count)
        {
            grandmotherAtWaypoint = false;
            StartMoving(ref isGrandmotherMoving);
        }
    }

    private void StartMoving(ref bool isMovingFlag)
    {
        isMovingFlag = true;
    }

    private bool MoveTowardsWaypoint(Transform character, List<Transform> waypoints, ref int waypointIndex, ref bool atWaypoint, ref bool isMovingFlag, float moveSpeed)
    {
        if (waypoints == null || waypoints.Count == 0 || atWaypoint)
            return false;

        moveSpeed *= Time.deltaTime;
        character.position = Vector3.MoveTowards(character.position, waypoints[waypointIndex].position, moveSpeed);

        Vector3 direction = waypoints[waypointIndex].position - character.position;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            character.rotation = Quaternion.Lerp(character.rotation, targetRotation, Time.deltaTime * 10f);
        }

        if (Vector3.Distance(character.position, waypoints[waypointIndex].position) <= 0.05f)
        {
            atWaypoint = true;
            isMovingFlag = false;

            waypointIndex++;
            if (waypointIndex >= waypoints.Count)
            {
                waypointIndex = waypoints.Count - 1;
            }
        }

        return isMovingFlag;
    }
}