using UnityEngine;

public class SearchPlayer : MonoBehaviour
{
    // reference to the flashlight raycast script and guard ai logic script and player detection state script
    private FlashlightRaycast flashlightRaycast;
    private GuardAiLogic guardAiLogic;
    private PlayerDetectionState playerDetectionState;

    // Empty at eye level of the guard.
    [SerializeField] private Transform EyesPosition;
    [SerializeField] private float FOV;


    void Start()
    {
        // get flashlightRaycast component from a child object (the Guard's hand)
        flashlightRaycast = GetComponentInChildren<FlashlightRaycast>();

        // get GuardAiLogic component from this object
        guardAiLogic = GetComponent<GuardAiLogic>();

        playerDetectionState = null; // we will get this component later from the player when we see him

        // debug check if we found it
        if (flashlightRaycast == null)
            Debug.LogError("FlashlightRaycast component not found in children of " + gameObject.name);
    }

    void FixedUpdate()
    {
        // get player with tag "Player" if it is in the trigger collider of this object
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f, LayerMask.GetMask("Player"));

        // check if hitColliders array is empty. if yes: return;
        if (hitColliders.Length == 0)
            return;

        Collider hitCollider = hitColliders[0]; // save collided player in a variable for easier acces

        Vector3 directionToPlayer = (hitCollider.transform.position - transform.position).normalized; // get direction vector to player
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer); // get angle of player relative to the guard
        if (angleToPlayer < (FOV/2)) // calculate if player is in view of *FOV* cone in front of guard
        {
            // do a sphereCast to see if player is in the light area of the flashlight
            RaycastHit hitInfo;
            hitInfo = flashlightRaycast.DoSpherecast(10f, 2f);

            if (hitInfo.collider != null) // if whe hit something ( the player or an obstacle )
            {
                // Raycast from the eyes of the guard, to check if there is direct line of sight
                RaycastHit hit;
                Vector3 positionOfEyes = EyesPosition.position;
                directionToPlayer = (hitCollider.transform.position - positionOfEyes).normalized;
                Physics.Raycast(positionOfEyes, directionToPlayer, out hit, 11f); // raycast from the eyes of the guard
                if (hit.collider != null && hit.collider.CompareTag("Player")) // when player in direct sight
                {
                    if (playerDetectionState == null) // get PlayerDetectionState component from the player if we don't have it yet
                        playerDetectionState = hitCollider.GetComponentInParent<PlayerDetectionState>();
                    playerDetectionState.SetPlayerInSight(true);
                    Debug.DrawLine(positionOfEyes, hit.point, Color.magenta); // draw magenta line when player in sight
                    guardAiLogic.PausePatrollingForSeconds(2.64f); // pause patrolling when player in sight
                }
                else
                {
                    if (playerDetectionState != null)
                        playerDetectionState.SetPlayerInSight(false);
                    Debug.DrawLine(positionOfEyes, hit.point, Color.cyan); // draw cyna line when player not in sight
                }

            } else
            {
                if (playerDetectionState != null)
                    playerDetectionState.SetPlayerInSight(false);
            }
        } 
    }

    private void OnDrawGizmos()
    {
        // visualize the overlap sphere in the editor
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 10f);

    }
}
