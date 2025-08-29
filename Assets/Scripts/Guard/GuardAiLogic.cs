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
    NavMeshAgent m_Agent;
    //RaycastHit m_HitInfo = new RaycastHit();
    private Animator m_Animator;

    [SerializeField] private float delayBetweenPatrolPoints;
    [SerializeField] private GameObject patrolPointsParent;
    private Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        // get animator from child component
        m_Animator = GetComponentInChildren<Animator>();

        // get all patrol points from parent object
        patrolPoints = patrolPointsParent.GetComponentsInChildren<Transform>();
        patrolPoints = patrolPoints[1..]; // remove first element which is the parent itself

        StartCoroutine(GoToPatrollingPoint());
        StartCoroutine(PrintAgentData());
    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
        //{
        //    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo))
        //        m_Agent.destination = m_HitInfo.point;
        //}
        //Debug.Log("Agent velocity: " + m_Agent.velocity.magnitude);
        if (m_Animator.GetBool("isWalking") && m_Agent.velocity.magnitude > 1.5f)
            m_Animator.speed = m_Agent.velocity.magnitude / m_Agent.speed; // adjust animation speed based on agent speed
        else
            m_Animator.speed = 1f; // reset animation speed when not walking
    }

    private IEnumerator PrintAgentData()
    {
        while (false)
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
        m_Animator.SetBool("isWalking", false);
        // wait until agent stop moving
        while (m_Agent.velocity.magnitude > 0.1f)
        {
            yield return null;
        }
        StartCoroutine(GoToPatrollingPoint());
    }

    //private void OnAnimatorMove()
    //{
    //    if (m_Animator.GetBool("isWalking"))
    //    {
    //        m_Agent.speed = (m_Animator.deltaPosition / Time.deltaTime).magnitude;
    //        Debug.Log("Agent speed: " + m_Agent.speed);
    //    }
    //}

}
