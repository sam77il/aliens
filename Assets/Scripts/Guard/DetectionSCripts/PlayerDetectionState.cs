using UnityEngine;
using UnityEngine.UI;

public class PlayerDetectionState : MonoBehaviour
{
    private float detectionTime; // Time the player has been detected
    [SerializeField] private float requiredDetectionTime; // Time required to trigger an alert
    private bool playerInSight; // Whether the player is currently in sight
    public bool playerDetected => detectionTime >= requiredDetectionTime;

    [Header("UI References")]
    [SerializeField] private Image detectionHeartPulse; // UI element to show state

    private void Update()
    {
        //Debug.Log("detected time: " +  detectionTime);
        // change color of detectionHeartPulse based on detectionTime, between white, orange and red
        if (detectionHeartPulse != null)
        {
            if (detectionTime <= requiredDetectionTime / 2)
            {
                detectionHeartPulse.color = Color.Lerp(Color.white, new Color(1f, 0.5f, 0f), detectionTime / (requiredDetectionTime / 2)); // white to orange
            }
            else
            {
                detectionHeartPulse.color = Color.Lerp(new Color(1f, 0.5f, 0f), Color.red, (detectionTime - (requiredDetectionTime / 2)) / (requiredDetectionTime / 2)); // orange to red
            }
        }

        if (playerInSight)
        { // player is in sight, increase detection time
            detectionTime += Time.deltaTime;
            if (detectionTime >= requiredDetectionTime)
            {
                // Trigger alert state
                Debug.Log("Player detected! Triggering alert state.");
                detectionTime = requiredDetectionTime; // Clamp to max
            }
        }
        else
        { // player is not in sight, decrease detection time
            detectionTime -= Time.deltaTime*3;
            if (detectionTime < 0f)
                detectionTime = 0f;
        }

    }

    public void SetPlayerInSight(bool inSight)
    {
        playerInSight = inSight;
    }



}
