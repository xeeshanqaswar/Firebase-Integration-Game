using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using Firebase.Auth;

public class MainMenu : MonoBehaviour
{
    public GameObject regPanel;

    public TextMeshProUGUI userName;
    public TextMeshProUGUI score;

    private PlayerProfile _playerProfile;
    private GameData _userGameData;

    private void Awake()
    {
        _playerProfile = Resources.Load<PlayerProfile>("UserProfile");
        _userGameData = Resources.Load<GameData>("PlayerGameData");
    }

    private void OnEnable()
    {
        SignInEvent += SingInListner;
        SignOutEvent += SignOutListner;
        RegisterUserEvent += RegisterUserListner;
    }


    private void SingInListner(string userId)
    {
        Debug.Log("Sign In Event Call");

        _playerProfile.userId = userId;
        SaveManager.Instance.LoadProgression();

        StartCoroutine(SignInFinish(0.5f));
    }
    IEnumerator SignInFinish(float delay)
    {
        yield return new WaitForSeconds(delay);

        regPanel.SetActive(false);
        userName.text = _playerProfile.userName;
        score.text = _userGameData.score.ToString();
    }

    private void SignOutListner()
    {
        Debug.Log("Sign Out Event Call");

        regPanel.SetActive(true);
        _playerProfile.EraseProfile();
    }

    private void RegisterUserListner(FirebaseUser user , string name)
    {
        Debug.Log("Register Event Call");
        regPanel.SetActive(false);

        // Populate player Profile
        _playerProfile.userId = user.UserId;
        _playerProfile.email = user.Email;
        _playerProfile.userName = name;

        SaveManager.Instance.SaveProgression(); // Save Data 

        // Display data on screen
        userName.text = _playerProfile.userName;
        score.text = _userGameData.score.ToString();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    #region EVENTS & ACTIONS

    public static event Action<FirebaseUser,string> RegisterUserEvent; 
    public static void RegisterUserEventCall(FirebaseUser user , string name)
    {
        RegisterUserEvent.Invoke(user, name);
    }

    public static event Action<string> SignInEvent; 
    public static void SignInEventCall(string id)
    {
        SignInEvent.Invoke(id);
    }

    public static event Action SignOutEvent; 
    public static void SignOutEventCall()
    {
        SignOutEvent.Invoke();
    }

    #endregion

    private void OnDisable()
    {
        SignInEvent -= SingInListner;
        SignOutEvent -= SignOutListner;
        RegisterUserEvent -= RegisterUserListner;
    }

}
