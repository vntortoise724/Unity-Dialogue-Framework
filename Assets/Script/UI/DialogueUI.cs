using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField] TextMeshProUGUI AIText;
        [SerializeField] Button nextBtn;
        [SerializeField] GameObject AIResponse;
        [SerializeField] Transform choiceRoot;
        [SerializeField] GameObject choicePrefab;
        [SerializeField] Button quitBtn;
        [SerializeField] TextMeshProUGUI conversantName;

        // Start is called before the first frame update
        void Start()
        {
            playerConversant = GameObject.Find("Player").GetComponent<PlayerConversant>();
            playerConversant.onConversationUpdated += UpdateUI;
            nextBtn.onClick.AddListener(() => playerConversant.Next());
            quitBtn.onClick.AddListener(() => playerConversant.Quit());

            UpdateUI();
        }

        void Next()
        {
            playerConversant.Next();
        }

        void UpdateUI()
        {
            gameObject.SetActive(playerConversant.IsActive());
            if (!playerConversant.IsActive())
            {
                return;
            }
            conversantName.text = playerConversant.GetCurrentConversantName();
            AIResponse.SetActive(!playerConversant.IsChoosing());
            choiceRoot.gameObject.SetActive(playerConversant.IsChoosing());
            if (playerConversant.IsChoosing())
            {
                BuildChoiceList();
            }
            else
            {
                AIText.text = playerConversant.GetText();
                nextBtn.gameObject.SetActive(playerConversant.HasNext());
            }
        }

        private void BuildChoiceList()
        {
            foreach (Transform item in choiceRoot)
            {
                Destroy(item.gameObject);
            }
            foreach (DialogueNode choice in playerConversant.GetChoices())
            {
                GameObject choiceInstance = Instantiate(choicePrefab, choiceRoot);
                var textComp = choiceInstance.GetComponentInChildren<TextMeshProUGUI>();
                textComp.text = choice.GetText();
                Button button = choiceInstance.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => 
                {
                    playerConversant.SelectChoice(choice);
                });
            }
        }
    }
}

