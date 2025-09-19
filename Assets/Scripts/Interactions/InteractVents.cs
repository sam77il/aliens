using System.Collections;
using UnityEngine;

public class InteractVents : Interactables
{
    [SerializeField] private float force = 10f;
    private GameObject[] screws;

    private void Start()
    {
        //StartCoroutine(UseDelay());


        // get the parallel gameobjects that have the Script InteractScrews, fill them in the screws variable and print their names to the console
        screws = new GameObject[transform.parent.childCount - 1];
        int index = 0;
        foreach (Transform sibling in transform.parent)
        {
            if (sibling.gameObject != this.gameObject && sibling.GetComponent<InteractScrews>() != null)
            {
                screws[index] = sibling.gameObject;
                //Debug.Log("Found screw: " + sibling.gameObject.name);
                index++;
            }
        }

    }

    private IEnumerator UseDelay()
    {
        yield return new WaitForSeconds(3.5f);
        Use();
    }

    public override void Use()
    {
        if (!AllScrewsUnscrewed())
        {
            Debug.Log("Cannot interact with vent, not all screws are unscrewed.");
            return;
        }
        Debug.Log("Interacting with Vents");
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        // add impulse force to vent on it*s local z axis
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
    }

    private bool AllScrewsUnscrewed()
    {
        foreach (GameObject screw in screws)
        {
            if (screw != null)
            {
                InteractScrews interactScrews = screw.GetComponent<InteractScrews>();
                if (interactScrews != null && interactScrews.isScrewed)
                {
                    return false; // found a screw that is still screwed
                }
            }
        }
        return true; // all screws are unscrewed
    }

}
