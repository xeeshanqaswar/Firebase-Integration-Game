using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="User Profile" , menuName ="Custom Data / User Profile")]
public class PlayerProfile : ScriptableObject
{
    public string userId;
    public string userName;
    public string email;

    public void EraseProfile()
    {
        userId = "";
        userName = "";
        email = "";
    }
}
