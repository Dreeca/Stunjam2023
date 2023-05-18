using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public PlayerUI player1UI;
    private Slider slider1;
    public PlayerUI player2UI;
    private Slider slider2;
    public PlayerUI player3UI;
    private Slider slider3;
    public PlayerUI player4UI;
    private Slider slider4;
    public Slider Noise;

    public GameObject EndGameUI;
    public TextMeshProUGUI WinnerTest;

    public GameObject InputMaps;

    public void Awake()
    {
        player1UI.gameObject.SetActive(false);
        slider1 = player1UI.transform.Find("SliderHold").GetComponent<Slider>();
        player2UI.gameObject.SetActive(false);
        slider2 = player2UI.transform.Find("SliderHold").GetComponent<Slider>();
        player3UI.gameObject.SetActive(false);
        slider3 = player3UI.transform.Find("SliderHold").GetComponent<Slider>();
        player4UI.gameObject.SetActive(false);
        slider4 = player4UI.transform.Find("SliderHold").GetComponent<Slider>();

    }

    public void RegisterPlayer(int playerNumber)
    {
        switch (playerNumber)
        {
            case 1:
                player1UI.gameObject.SetActive(true);
                break;
            case 2:
                player2UI.gameObject.SetActive(true);
                break;
            case 3:
                player3UI.gameObject.SetActive(true);
                break;
            case 4:
                player4UI.gameObject.SetActive(true);
                break;
        }
    }

    public void UpdatePlayerHoldTime(int PlayerNumber, float value)
    {
        switch (PlayerNumber)
        {
            case 1:
                slider1.value = value;
                break;
            case 2:
                slider2.value = value;
                break;
            case 3:
                slider3.value = value;
                break;
            case 4:
                slider4.value = value;
                break;
        }
    }

    public PlayerUI GetPlayerUI(int playerNumber)
    {
        switch (playerNumber)
        {
            case 1:
                return player1UI;
            case 2:
                return player2UI;
            case 3:
                return player3UI;
            case 4:
                return player4UI;
        }
        return null;
    }

    public void HideInputs()
    {
        InputMaps.SetActive(false);
    }

    public void DisplayWinner(int n)
    {
        EndGameUI.SetActive(true);
        WinnerTest.text = "PLAYER " + n + " WIN !";
    }

}
