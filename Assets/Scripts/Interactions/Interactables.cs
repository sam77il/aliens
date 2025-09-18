using UnityEngine;

public class Interactables : MonoBehaviour
{
    public virtual void Use()
    {
        Debug.Log("Using " + gameObject.name);
    }
}
