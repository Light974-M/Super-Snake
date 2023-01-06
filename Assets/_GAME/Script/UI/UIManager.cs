using SuperSnake.ClassicSnake;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI REFERENCES")]
    [SerializeField, Tooltip("text to display timer")]
    private Text _timerUI;

    [SerializeField, Tooltip("text to display score")]
    private Text _scoreUI;

    [SerializeField, Tooltip("text to display length of snake")]
    private Text _lengthUI;

    [SerializeField, Tooltip("panel of pause menu")]
    private GameObject _pauseMenuPanel;


    /// <summary>
    /// Level used for UI rendering
    /// </summary>
    private LevelRenderer _level;

    private List<SnakeRenderer> _playerList = new List<SnakeRenderer>();

    /// <summary>
    /// called when script is loaded
    /// </summary>
    private void Awake()
    {
        if (_level == null)
            _level = FindObjectOfType<LevelRenderer>();
    }

    /// <summary>
    /// update is called every frame
    /// </summary>
    private void Update()
    {
        PauseUIManager();
    }

    private void PauseUIManager()
    {
        if (_level.Level.IsPaused)
            _pauseMenuPanel.SetActive(true);
        else
            _pauseMenuPanel.SetActive(false);
    }

    private void PlayerListSetup()
    {
        _playerList = new List<SnakeRenderer>();

        for (int i = 0; i < _level.SnakeParentObject.transform.childCount; i++)
            _playerList.Add(_level.SnakeParentObject.transform.GetChild(i).gameObject.GetComponent<SnakeRenderer>());
    }

    public void EventSetup()
    {
        _level.SnakeScoreUIUpdate += OnScoreUpdate;
    }

    public void OnUpdateTimer()
    {
        _timerUI.text = "Time : " + (Mathf.Round(_level.Timer * 1000) / 1000);
    }

    public void OnScoreUpdate(int playerIndex)
    {
        PlayerListSetup();

        _scoreUI.text = "Score : " + _playerList[playerIndex].LinkedSnake.Score;
    }

    public void OnLengthUpdate()
    {
        if (_level.LevelParameters.PlayerNumber == 1)
        {
            int length = _playerList[0].LinkedSnake.Length;
            int worldSize = ((_level.Level.Width - 2) * (_level.Level.Height - 2));

            _lengthUI.text = "Length : " + length + " / " + worldSize + "      purcentage : " + (Mathf.Round(((float)length / (float)worldSize) * 100000) / 1000) + "%";
        }
    }

    public void OnResume()
    {
        _level.Level.PauseSwitch();
    }

    public void OnRetry()
    {
        _level.Retry();
    }

    public void OnQuit()
    {
        SceneManager.LoadScene("Menu");
    }
}
