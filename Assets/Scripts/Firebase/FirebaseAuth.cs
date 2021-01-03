using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Extensions;
using Firebase.Auth;

public class FirebaseAuth : MonoBehaviour
{
    [Header("Ref")]
    public GameObject success;
    public GameObject failed;

    [Header("Login Inputs")]
    public TMP_InputField l_email;
    public TMP_InputField l_password;

    [Header("Register Inputs")]
    public TMP_InputField r_userName;
    public TMP_InputField r_email;
    public TMP_InputField r_password;
    public TMP_InputField r_confirmPassword;

    private Firebase.Auth.FirebaseAuth auth;
    private FirebaseUser user;
    private PlayerProfile playerProfile;

    private void Awake()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        playerProfile = Resources.Load<PlayerProfile>("UserProfile");
    }

    private void OnEnable()
    {
        auth.StateChanged += AuthStateChanged;
    }

    private void Start()
    {
        AuthStateChanged(this, null);
    }

    public void OnLoginBtnPress()
    {
        auth.SignInWithEmailAndPasswordAsync(l_email.text, l_password.text).ContinueWithOnMainThread(task => {

            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                failed.SetActive(true);
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            user = task.Result;

            //Debug.LogFormat("User signed in successfully: {0} ({1})", user.UserId);
            MainMenu.SignInEventCall(user.UserId);
        });
    }

    public void OnRegisterBtnPress()
    {
        if (!r_password.text.Equals(r_confirmPassword.text))
        {
            failed.SetActive(true);
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(r_email.text, r_password.text).ContinueWithOnMainThread(task => {

            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                failed.SetActive(true);
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            user = task.Result;
            //Debug.LogFormat("Firebase user created successfully: {0} ({1})", user.UserId);
            MainMenu.RegisterUserEventCall(user, r_userName.text);
        });
    }

    public void OnSignoutPress()
    {
        auth.SignOut();
        MainMenu.SignOutEventCall();
    }

    /// <summary>
    /// Call Back received to tell wheter 
    /// </summary>
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                MainMenu.SignOutEventCall();
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                success.SetActive(true);
                MainMenu.SignInEventCall(user.UserId);
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    private void OnDisable()
    {
        auth.StateChanged -= AuthStateChanged;
    }

}
