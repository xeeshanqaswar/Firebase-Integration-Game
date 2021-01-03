using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UiManager : MonoBehaviour
{

    public TextMeshProUGUI scoreText;

    private PlayerProfile _playerProfile;
    private GameData _userGameData;

    private void Awake()
    {
        _playerProfile = Resources.Load<PlayerProfile>("UserProfile");
        _userGameData = Resources.Load<GameData>("PlayerGameData");

        scoreText.text = _userGameData.score.ToString();
    }

    private void OnEnable()
    {
        UiManager.UpdateScoreEvent += IncrementScore;
    }


    private void IncrementScore()
    {
        _userGameData.score++;
        scoreText.text = _userGameData.score.ToString();
    }

    public void SaveData()
    {
        SaveManager.Instance.SaveProgression();
    }

    private void OnDisable()
    {
        UiManager.UpdateScoreEvent -= IncrementScore;
    }

    public static Action UpdateScoreEvent;
    public static void UpdateScoreEventCall()
    {
        UpdateScoreEvent.Invoke();
    }

}
