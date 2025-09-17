using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class InteractScrews : Interactables
{

    public bool isScrewed = true;

    // Uncomment the following Start method to test the Use method automatically after a delay
    private void Start()
    {
        StartCoroutine(DelayUse());
    }

    private IEnumerator DelayUse()
    {
        yield return new WaitForSeconds(1.5f);
        Use();
    }

    public override void Use()
    {
        //Debug.Log("unScrewing " + gameObject.name);

        StartCoroutine(UseHelper());
    }

    private IEnumerator UseHelper()
    {
        float sec = 0f;
        while ( sec <= 0.2f )
        {
            // move toward z axis and rotate around z axis in local space slowly over time
            transform.Translate(Vector3.forward * Time.deltaTime * 0.5f, Space.Self);
            transform.Rotate(Vector3.forward * Time.deltaTime * -360f, Space.Self);
            sec += Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.25f);
        isScrewed = false;
        GetComponent<Rigidbody>().isKinematic = false;
    }

}
