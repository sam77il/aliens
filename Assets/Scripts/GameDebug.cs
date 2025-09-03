using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameDebug : MonoBehaviour
{
    public static GameDebug Instance { get; private set; }
    private List<TMP_Text> debugTexts;

    private void Awake()
    {
        Instance = this;
    }

  private void Start()
    {
        TMP_Text[] allText = gameObject.GetComponentsInChildren<TMP_Text>();
        debugTexts = new List<TMP_Text>(allText);
    }

    public void UpdateDebugText(string debugType, object value)
    {
        switch (debugType)
        {
            case "jumping":
                debugTexts[0].text = "IsJumping?: " + value;
                break;
            case "walking":
                debugTexts[1].text = "IsWalking?: " + value;
                break;
            case "sprinting":
                debugTexts[2].text = "IsSprinting?: " + value;
                break;
            case "againstwall":
                debugTexts[3].text = "IsAgainstWall?: " + value;
                break;
            case "grounded":
                debugTexts[4].text = "IsGrounded?: " + value;
                break;
            case "crouching":
                debugTexts[5].text = "IsCrouching?: " + value;
                break;
        }
    }
}
