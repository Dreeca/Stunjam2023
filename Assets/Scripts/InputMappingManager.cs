using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputMappingManager : MonoBehaviour
{
    [SerializeField] private InputMap m_PlayerOneInputMap;
    [SerializeField] private InputMap m_PlayerTwoInputMap;
    [SerializeField] private InputMap m_PlayerThreeInputMap;
    [SerializeField] private InputMap m_PlayerFourInputMap;

    [SerializeField] private GameObject[] m_PlayerInputPanels;

    private ActionType m_CurrentActionType;
    private int m_CurrentPlayerIndex = 0;

    private void Start()
    {
        UpdatePlayerInputs();
    }

    private void UpdatePlayerInputs()
    {
        // Button are always in this order: LEFT, UP, RIGHT, DOWN, ACTION
        // Player one
        Button[] _buttons = m_PlayerInputPanels[0].GetComponentsInChildren<Button>();
        _buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString(m_PlayerOneInputMap.name + "MoveUp", m_PlayerOneInputMap.MoveUp.bindings[0].effectivePath);
        _buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString(m_PlayerOneInputMap.name + "MoveDown", m_PlayerOneInputMap.MoveDown.bindings[0].effectivePath);
        _buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString(m_PlayerOneInputMap.name + "MoveLeft", m_PlayerOneInputMap.MoveLeft.bindings[0].effectivePath);
        _buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString(m_PlayerOneInputMap.name + "MoveRight", m_PlayerOneInputMap.MoveRight.bindings[0].effectivePath);
        _buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString(m_PlayerOneInputMap.name + "Interact", m_PlayerOneInputMap.Interact.bindings[0].effectivePath);

        // Player two
         _buttons = m_PlayerInputPanels[1].GetComponentsInChildren<Button>();
        _buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString(m_PlayerTwoInputMap.name + "MoveUp", m_PlayerTwoInputMap.MoveUp.bindings[0].effectivePath);
        _buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString(m_PlayerTwoInputMap.name + "MoveDown", m_PlayerTwoInputMap.MoveDown.bindings[0].effectivePath);
        _buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString(m_PlayerTwoInputMap.name + "MoveLeft", m_PlayerTwoInputMap.MoveLeft.bindings[0].effectivePath);
        _buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString(m_PlayerTwoInputMap.name + "MoveRight", m_PlayerTwoInputMap.MoveRight.bindings[0].effectivePath);
        _buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString(m_PlayerTwoInputMap.name + "Interact", m_PlayerTwoInputMap.Interact.bindings[0].effectivePath);

        // Player three
        _buttons = m_PlayerInputPanels[2].GetComponentsInChildren<Button>();
        _buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString(m_PlayerThreeInputMap.name + "MoveUp", m_PlayerThreeInputMap.MoveUp.bindings[0].effectivePath);
        _buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString(m_PlayerThreeInputMap.name + "MoveDown", m_PlayerThreeInputMap.MoveDown.bindings[0].effectivePath);
        _buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString(m_PlayerThreeInputMap.name + "MoveLeft", m_PlayerThreeInputMap.MoveLeft.bindings[0].effectivePath);
        _buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString(m_PlayerThreeInputMap.name + "MoveRight", m_PlayerThreeInputMap.MoveRight.bindings[0].effectivePath);
        _buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString(m_PlayerThreeInputMap.name + "Interact", m_PlayerThreeInputMap.Interact.bindings[0].effectivePath);

        // Player four
        _buttons = m_PlayerInputPanels[3].GetComponentsInChildren<Button>();
        _buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString(m_PlayerFourInputMap.name + "MoveUp", m_PlayerFourInputMap.MoveUp.bindings[0].effectivePath);
        _buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString(m_PlayerFourInputMap.name + "MoveDown", m_PlayerFourInputMap.MoveDown.bindings[0].effectivePath);
        _buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString(m_PlayerFourInputMap.name + "MoveLeft", m_PlayerFourInputMap.MoveLeft.bindings[0].effectivePath);
        _buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString(m_PlayerFourInputMap.name + "MoveRight", m_PlayerFourInputMap.MoveRight.bindings[0].effectivePath);
        _buttons[4].GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString(m_PlayerFourInputMap.name + "Interact", m_PlayerFourInputMap.Interact.bindings[0].effectivePath);
    }

    private void SavePlayerInfos()
    {
        m_PlayerOneInputMap.SaveToPlayerPrefs();
        m_PlayerTwoInputMap.SaveToPlayerPrefs();
        m_PlayerThreeInputMap.SaveToPlayerPrefs();
        m_PlayerFourInputMap.SaveToPlayerPrefs();

        UpdatePlayerInputs();
    }

    public void SelectPlayer(int _playerIndex)
    {
        m_CurrentPlayerIndex = _playerIndex;
    }

    public void AssociateAction(string _actionAsString)
    {
        switch(_actionAsString)
        {
            case "ACTION_LEFT": AssociateAction(ActionType.ACTION_LEFT); break;
            case "ACTION_UP": AssociateAction(ActionType.ACTION_UP); break;
            case "ACTION_RIGHT": AssociateAction(ActionType.ACTION_RIGHT); break;
            case "ACTION_DOWN": AssociateAction(ActionType.ACTION_DOWN); break;
            case "ACTION_INTERACT": AssociateAction(ActionType.ACTION_INTERACT); break;
        }
    }

    public void AssociateAction(ActionType _type)
    {
        m_CurrentActionType = _type;
    }

    public void AddAction(Button _btn)
    {
        TextMeshProUGUI _txt = _btn.GetComponentInChildren<TextMeshProUGUI>();
        RemapButtonClicked(m_CurrentActionType, _txt);
    }

    private void RemapButtonClicked(ActionType _action, TextMeshProUGUI _text)
    {
        InputAction _actionToRebind = null;
        InputMap _currInputMap = m_CurrentPlayerIndex == 0 ? m_PlayerOneInputMap :
                                 m_CurrentPlayerIndex == 1 ? m_PlayerTwoInputMap :
                                 m_CurrentPlayerIndex == 2 ? m_PlayerThreeInputMap :
                                m_CurrentPlayerIndex == 3 ? m_PlayerFourInputMap : 
                                null;

        if(_currInputMap == null)
        {
            throw new Exception("Current input map is null. Please set the input maps variables or check the m_CurrentPlayerIndex");
        }

        switch(_action)
        {
            case ActionType.ACTION_LEFT: _actionToRebind = _currInputMap.MoveLeft; break;
            case ActionType.ACTION_UP: _actionToRebind = _currInputMap.MoveUp; break;
            case ActionType.ACTION_RIGHT: _actionToRebind = _currInputMap.MoveRight; break;
            case ActionType.ACTION_DOWN: _actionToRebind = _currInputMap.MoveDown; break;
            case ActionType.ACTION_INTERACT: _actionToRebind = _currInputMap.Interact; break;
        }

        if(_actionToRebind == null)
        {
            throw new Exception("Action to rebind is null");
        }

        var rebindOperation = _actionToRebind.PerformInteractiveRebinding()
                    .WithControlsExcluding("Mouse")
                    .OnMatchWaitForAnother(0.1f);
        rebindOperation.OnComplete(operation =>
        {
            operation.Dispose();
            _text.text = _actionToRebind.bindings[0].effectivePath;
        });
        rebindOperation.Start();

        StartCoroutine(WaitBeforeSave());
    }

    private IEnumerator WaitBeforeSave()
    {
        yield return new WaitForSeconds(1);
        SavePlayerInfos();
    }
}

[Serializable]
public enum ActionType
{
    ACTION_LEFT,
    ACTION_UP,
    ACTION_RIGHT,
    ACTION_DOWN,
    ACTION_INTERACT,
}