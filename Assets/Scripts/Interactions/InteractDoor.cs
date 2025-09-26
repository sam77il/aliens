using System.Collections;
using UnityEngine;

public class InteractDoor : Interactables
{
    private InteractDoor[] doors;
    [SerializeField] private bool leftDoor = false;

    void Start()
    {
        // get the parallel gameobjects, including self, that have the Script InteractDoor and assign it to the doors variable
        doors = new InteractDoor[transform.parent.childCount - 1];
        int index = 0;
        foreach (Transform sibling in transform.parent)
        {
            if (sibling.GetComponent<InteractDoor>() != null)
            {
                doors[index] = sibling.GetComponent<InteractDoor>();
                index++;
            }
        }
        //Use();
    }

    public override void Use()
    {
        StartCoroutine(UseHelper());
    }

    private IEnumerator UseHelper()
    {
        // move door that has the Tag "DoorLeft" postitive in local x axis over 1 second and the door that has the Tag "DoorRight" negative in local x axis over 1 second
        float sec = 0f;
        while (sec <= 0.75f)
        {
            if (leftDoor)
            {
                transform.Translate(Vector3.right * Time.deltaTime * 1f, Space.Self);
            }
            else if (!leftDoor)
            {
                transform.Translate(Vector3.left * Time.deltaTime * 1f, Space.Self);
            }
            sec += Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }

        // disable this scripts for both doors
        foreach (InteractDoor door in doors)
        {
            if (door != null)
            {
                door.enabled = false;
            }
        }
    }

}
