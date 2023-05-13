using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    public UIManager UiManager;

    public float TimeToWin;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void RegisterPlayer(int PlayerNumber)
    {
        UiManager.RegisterPlayer(PlayerNumber);
    }

    public void UpdatePlayerHoldTime(int PlayerNumber, float value)
    {
        UiManager.UpdatePlayerHoldTime(PlayerNumber, (value / TimeToWin) * 100);
        if (value >= TimeToWin) PlayerWin(PlayerNumber);
    }

    public void PlayerWin(int PlayerNumber)
    {
        Debug.Log("Player " + PlayerNumber + " win !");
    }

}
