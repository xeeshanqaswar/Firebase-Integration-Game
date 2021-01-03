using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using System;

public class FirebaseInit : MonoBehaviour
{
    private FirebaseApp app;

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {

            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                app = Firebase.FirebaseApp.DefaultInstance;
                Debug.Log("Firebase Initialization Successful");
            }
            else
            {
                Debug.LogError(String.Format( "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }

}
