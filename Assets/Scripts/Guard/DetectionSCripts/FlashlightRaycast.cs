using UnityEngine;

public class FlashlightRaycast : MonoBehaviour
{
    private RaycastHit hitInfo;
    private bool hitSomething;
    private float radius;

    // do spherecast to see if we hit something in front of the flashlight
    public RaycastHit DoSpherecast(float distance, float radius)
    {
        Physics.SphereCast(transform.position, radius, transform.forward, out hitInfo, distance, LayerMask.GetMask("Player", "Obstacle"));
        Collider col = hitInfo.collider;
        if (col != null && col.CompareTag("Player")) // we hit the Player
        {  
            this.radius = radius; // set radius for drawWireSphere
            hitSomething = true; // set bool to draw wireSphere
            Debug.DrawRay(transform.position, transform.forward*distance, Color.red); // draw red ray when hitting player
            return hitInfo;
        }
        else // we did not hit the player
        {
            hitSomething = false;
            Debug.DrawRay(transform.position, transform.forward*distance, Color.green); // draw green ray when not hitting player

            return new RaycastHit();
        }

        

    }

    private void OnDrawGizmos()
    {
        if (hitSomething)
        { // draw a sphere when hitting player
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitInfo.point, radius);
        }
    }

}
