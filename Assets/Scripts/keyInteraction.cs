using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using TMPro;

public class keyInteraction : MonoBehaviour
{
    public NPCConversation myConversation;

    [HideInInspector]
    public bool isInRange = false;

    [Header("UI Display")]
    public GameObject UIDisplay;
    public TMP_Text TextDisplay;
    public string InteractibleNameText;

    private RectTransform UiDisplayRectTransform;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInRange)
        {
            ConversationManager.Instance.StartConversation(myConversation);
        }
    }
    void Awake()
    {
        UiDisplayRectTransform = UIDisplay.GetComponent<RectTransform>();
    }

    public void DestroyMe()
    {
        this.gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            //Debug.Log("In range");
            TextDisplay.SetText(InteractibleNameText);
            isInRange = true;
            UiDisplayRectTransform.LeanAlpha(0f, 0f);
            UIDisplay.SetActive(true);
            LeanTween.alpha(UiDisplayRectTransform, 1f, .01f);
        }
        //if (col.gameObject.GetComponent<DestroyOnContact>() != null)
        //{
        //    if (ammoType == 2)
        //        SpawnProjectiles(numberOfProjectiles);
        //    Destroy(gameObject);
        //}
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            //Debug.Log("Left Range range");
            isInRange = false;

            LeanTween.alpha(UiDisplayRectTransform, 0f, 0f).setOnComplete(eventa => {

                UIDisplay.SetActive(false);

            });

        }
    }
}
