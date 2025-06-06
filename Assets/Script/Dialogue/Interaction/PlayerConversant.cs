using Thesis.Dialogue;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerConversant : MonoBehaviour
{
    [SerializeField] private string playerName;
    private Dialogue currentDialogue;
    private DialogueNode currentNode = null;
    private AIConversant currentNPC = null;
    private bool isChoosing = false;
    public event Action OnConversationUpdated;

    public void StartDialogue(AIConversant _conversant, Dialogue _dialogue)
    {
        currentNPC = _conversant;
        currentDialogue = _dialogue;
        currentNode = currentDialogue.GetRootNode();
        TriggerEnterAction();
        OnConversationUpdated();
    }

    public void Quit()
    {
        currentDialogue = null;
        TriggerExitAction();
        currentNode = null;
        isChoosing = false;
        currentNPC = null;
        OnConversationUpdated();
    }

    public bool IsActive()
    {
        return currentDialogue != null;
    }

    public bool IsChoosing()
    {
        return isChoosing;
    }

    public string GetText()
    {
        if (currentNode == null)
            return "";
        return currentNode.GetText();
    }

    public IEnumerable<DialogueNode> GetChoices()
    {
        return currentDialogue.GetPlayerChildren(currentNode);
    }

    public string GetCurrentConversantName()
    {
        if (isChoosing)
            return playerName;
        else
            return currentNPC.GetName();
    }

    public void SelectChoice(DialogueNode _chosenNode)
    {
        currentNode = _chosenNode;
        TriggerEnterAction();
        isChoosing = false;
        TriggerExitAction();
        Next();
    }

    public void Next()
    {
        int numPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
        if (numPlayerResponses > 0)
        {
            isChoosing = true;
            TriggerExitAction();
            OnConversationUpdated();
            return;
        }

        DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
        int randomIndex = UnityEngine.Random.Range(0, children.Count());
        TriggerExitAction();
        currentNode = children[randomIndex];
        TriggerEnterAction();
        OnConversationUpdated();
    }

    public bool HasNext()
    {
        return currentDialogue.GetAllChildren(currentNode).Count() > 0;
    }

    private void TriggerEnterAction()
    {
        if (currentNode != null)
            TriggerAction(currentNode.GetOnEnterAction());
    }

    private void TriggerExitAction()
    {
        if (currentNode != null)
            TriggerAction(currentNode.GetOnExitAction());
    }

    private void TriggerAction(string _action)
    {
        if (_action == "")
            return;
        foreach (DialogueTrigger trigger in currentNPC.GetComponents<DialogueTrigger>())
        {
            trigger.Trigger(_action);
        }
    }
}
