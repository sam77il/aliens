using UnityEngine;

public class FlashlightRaycast : MonoBehaviour
{

    //private void FixedUpdate()
    //{
    //    // do a raycast every fixed update to see if we hit something in front of us
    //    DoRaycast(10f, LayerMask.GetMask("Player", "Obstacle")); //example usage
    //}


    private RaycastHit hitInfo;
    private bool hitSomething;
    private float radius;

    // do raycast as spherecast to see if we hit something in front of us
    public RaycastHit DoSpherecast(float distance, float radius, LayerMask layerMask)
    {
        if (Physics.SphereCast(transform.position, radius, transform.forward, out hitInfo, distance, layerMask) && hitInfo.collider.CompareTag("Player"))
        { 
            this.radius = radius;
            hitSomething = true;
            Debug.DrawLine(transform.position, hitInfo.point, Color.red);
            // we hit something
            //Debug.Log("Flashlight hit: " + hitInfo.collider.name);
            return hitInfo;
        }
        else
        {
            hitSomething = false;
            Debug.DrawRay(transform.position, hitInfo.point, Color.green);
            // we didn't hit anything
            return new RaycastHit();
        }
    }


    private void OnDrawGizmos()
    {
        if (hitSomething)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitInfo.point, radius);
        }
    }

}
