using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public GameObject player1UI;
    private Slider slider1;
    public GameObject player2UI;
    private Slider slider2;
    public GameObject player3UI;
    private Slider slider3;
    public GameObject player4UI;
    private Slider slider4;


    public void Start()
    {
        player1UI.SetActive(false);
        slider1 = player1UI.transform.Find("SliderHold").GetComponent<Slider>();
        player2UI.SetActive(false);
        slider2 = player2UI.transform.Find("SliderHold").GetComponent<Slider>();
        player3UI.SetActive(false);
        slider3 = player3UI.transform.Find("SliderHold").GetComponent<Slider>();
        player4UI.SetActive(false);
        slider4 = player4UI.transform.Find("SliderHold").GetComponent<Slider>();

    }

    public void RegisterPlayer(int playerNumber)
    {
        switch (playerNumber)
        {
            case 1:
                player1UI.SetActive(true);
                break;
            case 2:
                player2UI.SetActive(true);
                break;
            case 3:
                player3UI.SetActive(true);
                break;
            case 4:
                player4UI.SetActive(true);
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

}
