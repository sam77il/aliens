using System.Collections.Generic;
using UnityEngine;

public class SignsManager : MonoBehaviour
{
    [SerializeField] private GameObject exits;
    private List<GameObject> exitPoints = new List<GameObject>();
    private int selectedExitIndex = -1;

    private void Start()
    {
        foreach (Transform child in exits.transform)
        {
            exitPoints.Add(child.gameObject);
        }

        foreach (GameObject exit in exitPoints)
        {
            exit.SetActive(false);
        }

        RandomizeExit();
    }

    private void RandomizeExit()
    {
        if (exitPoints.Count == 0) return;

        int randomIndex = Random.Range(0, exitPoints.Count);
        GameObject selectedExit = exitPoints[randomIndex];

        selectedExit.SetActive(true);
        selectedExitIndex = int.Parse(selectedExit.name.Split('_')[1]);
        Debug.Log("Activated Exit: " + selectedExitIndex);
    }
}
