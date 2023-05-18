using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    public Slider waterSlider;
    public GameObject NerfObject;
    public TextMeshProUGUI NerfTest;
    public Slider DashSlider;

    public void ActivateWaterSlider(bool value)
    {
        waterSlider.gameObject.SetActive(value);
    }

    public void UpdateWaterUI(float value)
    {
        waterSlider.value = value;
    }

    public void ActivateNerf(bool value)
    {
        NerfObject.SetActive(value);
    }
    public void UpdateNerfUI(int value)
    {
        NerfTest.text = value.ToString();
    }

    public void ActivateDashSlider(bool value)
    {
        DashSlider.gameObject.SetActive(value);
    }

    public void UpdateDashUI(float value)
    {
        DashSlider.value = value;
    }


}
