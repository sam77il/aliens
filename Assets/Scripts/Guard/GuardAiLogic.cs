using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// copy paste "click to move" script from unity standard assets and adjusted it to work as we need it
// tutorial watched: https://www.youtube.com/watch?v=SMWxCpLvrcc; https://www.youtube.com/watch?v=vU6fCMC_IXA
/// <summary>
/// Walking between different pre define patroling points
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class GuardAiLogic : MonoBehaviour
{
    // Variables
    NavMeshAgent m_Agent;
    private Animator m_Animator;

    [Header("Patrolling Settings")]
    [SerializeField] private float delayBetweenPatrolPoints;
    [SerializeField] private GameObject patrolPointsParent;
    private Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    private bool patrollingIsEnabled;

    void Start()
    {
        // set the variables
        patrollingIsEnabled = true;
        // get NavMeshAgent
        m_Agent = GetComponent<NavMeshAgent>();
        // get animator from child component (the 3d model)
        m_Animator = GetComponentInChildren<Animator>();

        // get all patrol points from parent object
        patrolPoints = patrolPointsParent.GetComponentsInChildren<Transform>();
        patrolPoints = patrolPoints[1..]; // remove first element which is the parent itself

        // start patrolling
        StartCoroutine(GoToPatrollingPoint());
        //StartCoroutine(PrintAgentData());
    }

    void Update()
    {
        if (m_Animator.GetBool("isWalking") && m_Agent.velocity.magnitude > 1.5f)
            m_Animator.speed = m_Agent.velocity.magnitude / m_Agent.speed; // adjust animation speed based on agent speed
        else
            m_Animator.speed = 1f; // reset animation speed when not walking
    }

    public void PausePatrollingForSeconds(float duration)
    {
        if (patrollingIsEnabled)
            StartCoroutine(PausePatrollingForSecondsHelper(duration));
    }

    private IEnumerator PausePatrollingForSecondsHelper(float duration)
    {
        // pause patrolling for duration seconds
        patrollingIsEnabled = false;
        m_Agent.isStopped = true;
        m_Animator.SetBool("isWalking", false); // set animation

        yield return new WaitForSeconds(duration); // wait for duration seconds

        // resume patrolling
        patrollingIsEnabled = true;
        m_Animator.SetBool("isWalking", true); // set animation
        m_Agent.isStopped = false;
    }

    // helper function
    private IEnumerator PrintAgentData()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("Agent velocity: " + m_Agent.velocity.magnitude);
            Debug.Log("Agent !hasPath: " + !m_Agent.hasPath);
            Debug.Log("animator state: " + m_Animator.GetBool("isWalking") + "\ncurrentAnim: " + m_Animator.GetCurrentAnimatorStateInfo(0).shortNameHash );
        }
    }

    private IEnumerator GoToPatrollingPoint()
    {

        yield return new WaitForSeconds(delayBetweenPatrolPoints);

        m_Animator.SetBool("isWalking", true);
        m_Agent.destination = patrolPoints[currentPatrolIndex++ % patrolPoints.Length].position;

        StartCoroutine(WaitUntilAgentReachDestination());
    }

    private IEnumerator WaitUntilAgentReachDestination()
    {
        // wait until agent reach destination
        while (m_Agent.pathPending || m_Agent.remainingDistance > m_Agent.stoppingDistance)
        {
            yield return null;
        }

        m_Animator.SetBool("isWalking", false); // set animation
        
        // wait until agent stop moving
        while (m_Agent.velocity.magnitude > 0.1f)
        {
            yield return null;
        }

        // go to next patrolling point
        StartCoroutine(GoToPatrollingPoint());
    }

}
