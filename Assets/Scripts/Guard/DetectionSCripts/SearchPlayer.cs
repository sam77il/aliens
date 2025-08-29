using UnityEngine;

public class SearchPlayer : MonoBehaviour
{
    private FlashlightRaycast flashlightRaycast;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get flashlightRaycast component from a child object
        flashlightRaycast = GetComponentInChildren<FlashlightRaycast>();
        //debug check if we found it
        if (flashlightRaycast == null)
            Debug.LogError("FlashlightRaycast component not found in children of " + gameObject.name);
    }

    void FixedUpdate()
    {
        // if player with tag "Player" is in the trigger collider of this object, calculate if player is in view of 75° cone in front of this object
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f, LayerMask.GetMask("Player"));
        // visualize the sphere in the editor
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.blue);



        foreach (var hitCollider in hitColliders)
        {
            Vector3 directionToPlayer = (hitCollider.transform.position - transform.position).normalized;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer < 37.5f) // half of 75°
            {
                Debug.Log("Player in view: " + hitCollider.name);
                // do a raycast to see if there is a direct line of sight to the player
                RaycastHit hitInfo;
                hitInfo = flashlightRaycast.DoSpherecast(10f, LayerMask.GetMask("Player", "Obstacle"));
            }
        }
    }
}
