using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBlock : MonoBehaviour, IInteractable
{
    public void Interaction()
    {
        UiManager.UpdateScoreEventCall();
        gameObject.SetActive(false);
    }


}
