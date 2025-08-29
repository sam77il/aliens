using UnityEngine;

public class FlashlightRaycast : MonoBehaviour
{

    //private void FixedUpdate()
    //{
    //    // do a raycast every fixed update to see if we hit something in front of us
    //    DoRaycast(10f, LayerMask.GetMask("Player", "Obstacle")); //example usage
    //}

    // do raycast as spherecast to see if we hit something in front of us
    public RaycastHit DoSpherecast(float distance, LayerMask layerMask)
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, 0.5f, transform.forward, out hitInfo, distance, layerMask))
        {
            Debug.DrawRay(transform.position, transform.forward * hitInfo.distance, Color.red);
            // we hit something
            Debug.Log("Flashlight hit: " + hitInfo.collider.name);
            return hitInfo;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * distance, Color.green);
            // we didn't hit anything
            return new RaycastHit();
        }
    }

}
