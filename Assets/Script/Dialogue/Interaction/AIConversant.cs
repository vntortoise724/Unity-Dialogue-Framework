using Thesis.Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIConversant : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private string npcName;

    public void IntiatiateDialogue(Transform player)
    {
        if (dialogue == null)
            return;

        player.GetComponent<PlayerConversant>().StartDialogue(this, dialogue);
    }

    public string GetName()
    {
        return npcName;
    }
}
