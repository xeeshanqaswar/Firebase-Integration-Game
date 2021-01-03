using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;

public class SaveManager : MonoBehaviour
{

    public static SaveManager Instance;

    private PlayerProfile _userProfile;
    private GameData _userGameData;
    private PlayerProgression _userProgression;

    private DatabaseReference _dbReference;
    private const string PLAYER_DATA = "PlayerData";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        _userProfile = Resources.Load<PlayerProfile>("UserProfile");
        _userGameData = Resources.Load<GameData>("PlayerGameData");
    }

    public void LoadProgression()
    {
        _userProgression = new PlayerProgression();

        _dbReference.Child(PLAYER_DATA).Child(_userProfile.userId).GetValueAsync().ContinueWithOnMainThread(task => {

            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Value != null)
                {
                    Debug.Log("SnapShot Exist");

                    // Data against user exist
                    _userProgression = JsonUtility.FromJson<PlayerProgression>(snapshot.GetRawJsonValue());
                    _userGameData.level = _userProgression.level;
                    _userGameData.score = _userProgression.score;
                    _userProfile.userName = _userProgression.playerName;
                }
                else
                {
                    Debug.Log("SnapShot not exist");
                    // Data doesn't exist
                }
            }
        });
    }

    public void SaveProgression()
    {
        _userProgression = new PlayerProgression();
        _userProgression.playerName = _userProfile.userName;
        _userProgression.level = _userGameData.level;
        _userProgression.score = _userGameData.score;

        string progToJson = JsonUtility.ToJson(_userProgression);
        _dbReference.Child(PLAYER_DATA).Child(_userProfile.userId).SetRawJsonValueAsync(progToJson);
        Debug.Log("Data Saving Happened");
    }

}

public class PlayerProgression
{
    public string playerName;
    public int score;
    public int level;
}
