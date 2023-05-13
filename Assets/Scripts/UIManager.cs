using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{

    private VisualElement Player1Interface;
    private ProgressBar Player1HoldTime;
    private VisualElement Player2Interface;
    private ProgressBar Player2HoldTime;
    private VisualElement Player3Interface;
    private ProgressBar Player3HoldTime;
    private VisualElement Player4Interface;
    private ProgressBar Player4HoldTime;

    private UIDocument uiDocument;

    public void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        Player1Interface = uiDocument.rootVisualElement.Q<VisualElement>("Player1Interface");
        Player1HoldTime = Player1Interface.Q<ProgressBar>("HoldTime");
        Player1Interface.visible = false;
        Player2Interface = uiDocument.rootVisualElement.Q<VisualElement>("Player2Interface");
        Player2HoldTime = Player2Interface.Q<ProgressBar>("HoldTime");
        Player2Interface.visible = false;
        Player3Interface = uiDocument.rootVisualElement.Q<VisualElement>("Player3Interface");
        Player3HoldTime = Player3Interface.Q<ProgressBar>("HoldTime");
        Player3Interface.visible = false;
        Player4Interface = uiDocument.rootVisualElement.Q<VisualElement>("Player4Interface");
        Player4HoldTime = Player4Interface.Q<ProgressBar>("HoldTime");
        Player4Interface.visible = false;
    }

    public void RegisterPlayer(int playerNumber)
    {
        switch (playerNumber)
        {
            case 1:
                Player1Interface.visible = true;
                Player1HoldTime.value = 0;
                break;
            case 2:
                Player2Interface.visible = true;
                Player2HoldTime.value = 0;
                break;
            case 3:
                Player3Interface.visible = true;
                Player3HoldTime.value = 0;
                break;
            case 4:
                Player4Interface.visible = true;
                Player4HoldTime.value = 0;
                break;
        }
    }

    public void UpdatePlayerHoldTime(int PlayerNumber, float value)
    {
        switch (PlayerNumber)
        {
            case 1:
                Player1HoldTime.value = value;
                break;
            case 2:
                Player2HoldTime.value = value;
                break;
            case 3:
                Player3HoldTime.value = value;
                break;
            case 4:
                Player4HoldTime.value = value;
                break;
        }
    }

}
