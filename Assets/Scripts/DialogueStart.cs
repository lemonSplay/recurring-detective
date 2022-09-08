using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using TMPro;

public class DialogueStart : MonoBehaviour
{
    public NPCConversation myConversation;

    [HideInInspector]
    public bool isInRange = false;
    

    public GameObject player;

    

    [Header("UI Display")]
    public GameObject UIDisplay;
    public TMP_Text TextDisplay;
    public string InteractibleNameText;

    private RectTransform UiDisplayRectTransform;

    void Awake()
    {
        UiDisplayRectTransform = UIDisplay.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInRange)
        {
            ConversationManager.Instance.StartConversation(myConversation);
            player.GetComponent<playerMovement>().speed = 0;
            UIDisplay.SetActive(false);

        }
    }


    public void SetPlayerSpeed(float _speed)
    {
        player.GetComponent<playerMovement>().speed = _speed;
    }

    public void DestroyMe()
    {
        this.gameObject.SetActive(false);
    }

    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col.gameObject.tag == "Player")
    //    {
    //        Debug.Log("In range");
    //        isInRange = true;
    //    }
    //}
    //void OnTriggerExit2D(Collider2D col)
    //{
    //    if (col.gameObject.tag == "Player")
    //    {
    //        Debug.Log("Left Range range");
    //        isInRange = false;

    //    }
    //}


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            //Debug.Log("In range of door");
            TextDisplay.SetText(InteractibleNameText);


            UiDisplayRectTransform.LeanAlpha(0f, 0f);
            UIDisplay.SetActive(true);
            LeanTween.alpha(UiDisplayRectTransform, 1f, .01f);
            
            isInRange = true;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            //Debug.Log("Left Range of door");
            
            LeanTween.alpha(UiDisplayRectTransform, 0f, 0f).setOnComplete(eventa => {

                UIDisplay.SetActive(false);

            });

            isInRange = false;

        }
    }
}
