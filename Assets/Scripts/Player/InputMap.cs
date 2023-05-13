using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputMap", menuName = "Player/InputMap", order = 1)]
public class InputMap : ScriptableObject
{
    public InputAction MoveUp;
    public InputAction MoveDown;
    public InputAction MoveLeft;
    public InputAction MoveRight;
    public InputAction Interact;


}
