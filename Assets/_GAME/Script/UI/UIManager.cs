using SuperSnake.ClassicSnake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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


    /// <summary>
    /// Level used for UI rendering
    /// </summary>
    private LevelRenderer _level;

    private void Awake()
    {
        if(_level == null)
            _level = FindObjectOfType<LevelRenderer>();
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
}
