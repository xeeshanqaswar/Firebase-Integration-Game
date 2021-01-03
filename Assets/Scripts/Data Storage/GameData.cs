using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Data", menuName = "Custom Data / Game Data")]
public class GameData : ScriptableObject
{
    public int score;
    public int level;
}
