using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    public UIManager UiManager;
    public NoiseManager Noise;

    public List<PlayerController> ActivePlayer;

    public float TimeToWin;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void RegisterPlayer(int PlayerNumber, PlayerController controller)
    {
        UiManager.RegisterPlayer(PlayerNumber);
        ActivePlayer.Add(controller);
    }

    public void UpdatePlayerHoldTime(int PlayerNumber, float value)
    {
        UiManager.UpdatePlayerHoldTime(PlayerNumber, (value / TimeToWin));
        if (value >= TimeToWin) PlayerWin(PlayerNumber);
    }

    public void PlayerWin(int PlayerNumber)
    {
        //Debug.Log("Player " + PlayerNumber + " win !");
    }

}
