using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    public UIManager UiManager;
    public NoiseManager Noise;
    public List<PlayerController> PossiblePlayers;

    public List<PlayerController> ActivePlayer;

    public float TimeToWin;

    public InputAction RestartAction;
    private InputAction actionInstance;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        actionInstance = RestartAction.Clone();
        actionInstance.Enable();
        actionInstance.performed += (ctx) => Restart();
    }

    public void StartWithPlayers(int n)
    {
        for (int i = 0; i < n; i++)
        {
            PossiblePlayers[i].gameObject.SetActive(true);
            RegisterPlayer(i + 1, PossiblePlayers[i]);
        }
        AudioManager.Instance.Play(Vector3.zero, SoundType.MAIN_THEME);
        UiManager.HideInputs();
    }

    public void RegisterPlayer(int PlayerNumber, PlayerController controller)
    {
        UiManager.RegisterPlayer(PlayerNumber);
        ActivePlayer.Add(controller);
        controller.StartPlayer();
    }

    public void UpdatePlayerHoldTime(int PlayerNumber, float value)
    {
        UiManager.UpdatePlayerHoldTime(PlayerNumber, (value / TimeToWin));
        if (value >= TimeToWin) PlayerWin(PlayerNumber);
    }

    public void PlayerWin(int PlayerNumber)
    {
        //Debug.Log("Player " + PlayerNumber + " win !");
        ActivePlayer.ForEach(x => { x.enabled = false; });
        Noise.enabled = false;
        Noise.Parent.enabled = false;
        UiManager.DisplayWinner(PlayerNumber);
    }

    public void Restart()
    {
        ActivePlayer.ForEach(x => { x.DestroyInputs(); });
        SceneManager.LoadScene(0);
    }

}
