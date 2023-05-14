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

    public void SaveToPlayerPrefs()
    {
        PlayerPrefs.SetString(name + "MoveUp", MoveUp.bindings[0].effectivePath);
        PlayerPrefs.SetString(name + "MoveDown", MoveDown.bindings[0].effectivePath);
        PlayerPrefs.SetString(name + "MoveLeft", MoveLeft.bindings[0].effectivePath);
        PlayerPrefs.SetString(name + "MoveRight", MoveRight.bindings[0].effectivePath);
        PlayerPrefs.SetString(name + "Interact", Interact.bindings[0].effectivePath);
        PlayerPrefs.Save();
    }

    public void LoadFromPlayerPrefs()
    {
        MoveUp.ChangeBinding(0).WithPath(PlayerPrefs.GetString(name + "MoveUp", MoveUp.bindings[0].effectivePath));
        MoveDown.ChangeBinding(0).WithPath(PlayerPrefs.GetString(name + "MoveDown", MoveDown.bindings[0].effectivePath));
        MoveLeft.ChangeBinding(0).WithPath(PlayerPrefs.GetString(name + "MoveLeft", MoveLeft.bindings[0].effectivePath));
        MoveRight.ChangeBinding(0).WithPath(PlayerPrefs.GetString(name + "MoveRight", MoveRight.bindings[0].effectivePath));
        Interact.ChangeBinding(0).WithPath(PlayerPrefs.GetString(name + "Interact", Interact.bindings[0].effectivePath));
    }
}