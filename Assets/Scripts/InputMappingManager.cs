using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputMappingManager : MonoBehaviour
{
    public InputMap player1;
    public InputMap player2;
    public InputMap player3;
    public InputMap player4;

    [SerializeField] private List<GameObject> PlayerInputPanels;

    private void Start()
    {

        player1.LoadFromPlayerPrefs();
        UpdatePlayer(player1, 0);
        player2.LoadFromPlayerPrefs();
        UpdatePlayer(player2, 1);
        player3.LoadFromPlayerPrefs();
        UpdatePlayer(player3, 2);
        player4.LoadFromPlayerPrefs();
        UpdatePlayer(player4, 3);
        SetOnClicks(player1, 0);
        SetOnClicks(player2, 1);
        SetOnClicks(player3, 2);
        SetOnClicks(player4, 3);
    }


    private void UpdatePlayer(InputMap player, int playerNumber)
    {
        Button[] buttons = PlayerInputPanels[playerNumber].GetComponentsInChildren<Button>();
        buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = player.MoveUp.bindings[0].effectivePath;
        buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = player.MoveLeft.bindings[0].effectivePath;
        buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = player.MoveDown.bindings[0].effectivePath;
        buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = player.MoveRight.bindings[0].effectivePath;
        buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = player.Interact.bindings[0].effectivePath;
    }

    public void SetOnClicks(InputMap player, int number)
    {
        Button[] buttons = PlayerInputPanels[number].GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(() => RemapButtonClicked(player.MoveUp, buttons[0].GetComponentInChildren<TextMeshProUGUI>(), player));
        buttons[1].onClick.AddListener(() => RemapButtonClicked(player.MoveLeft, buttons[1].GetComponentInChildren<TextMeshProUGUI>(), player));
        buttons[2].onClick.AddListener(() => RemapButtonClicked(player.MoveDown, buttons[2].GetComponentInChildren<TextMeshProUGUI>(), player));
        buttons[3].onClick.AddListener(() => RemapButtonClicked(player.MoveRight, buttons[3].GetComponentInChildren<TextMeshProUGUI>(), player));
        buttons[4].onClick.AddListener(() => RemapButtonClicked(player.Interact, buttons[4].GetComponentInChildren<TextMeshProUGUI>(), player));
    }

    private void RemapButtonClicked(InputAction actionToRebind, TextMeshProUGUI text, InputMap map)
    {
        InputActionRebindingExtensions.RebindingOperation rebindOperation = actionToRebind.PerformInteractiveRebinding()
                    .WithControlsExcluding("Mouse")
                    .OnMatchWaitForAnother(0.1f);
        rebindOperation.OnComplete(operation =>
        {
            operation.Dispose();
            text.text = actionToRebind.bindings[0].effectivePath;
            map.SaveToPlayerPrefs();
        });
        rebindOperation.Start();
    }

}

