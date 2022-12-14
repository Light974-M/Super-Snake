using SuperSnake.ClassicSnake;
using System.Collections;
using System.Collections.Generic;
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

    /// <summary>
    /// called when script is loaded
    /// </summary>
    private void Awake()
    {
        if(_level == null)
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
        if(_level.Level.IsPaused)
            _pauseMenuPanel.SetActive(true);
        else
            _pauseMenuPanel.SetActive(false);
    }

    public void OnUpdateTimer()
    {
        _timerUI.text = "Time : " + (Mathf.Round(_level.Timer * 1000)/1000);
    }

    public void OnScoreUpdate()
    {
        _scoreUI.text = "Score : " + _level.Level.SnakePlayer.Score;
    }

    public void OnLengthUpdate()
    {
        int length = _level.Level.SnakePlayer.Length;
        int worldSize = ((_level.Level.Width - 2) * (_level.Level.Height - 2));

        _lengthUI.text = "Length : " + length + " / " + worldSize + "      purcentage : " + (Mathf.Round(((float)length / (float)worldSize) * 100000) / 1000) + "%";
    }

    public void OnResume()
    {
        _level.Level.PauseSwitch();
    }

    public void OnRetry()
    {
        _level.Respawn();
    }

    public void OnQuit()
    {
        SceneManager.LoadScene("Menu");
    }
}
