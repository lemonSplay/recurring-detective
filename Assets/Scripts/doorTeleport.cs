using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using TMPro;

public class doorTeleport : MonoBehaviour
{
    //public NPCConversation myConversation;

    [HideInInspector]
    public bool isInRange = false;

    public GameObject teleportTo;
    public GameObject player;

    [SerializeField]
    private GameObject rainGenerator;

    [Header("Scene fade in/out")]
    public GameObject fadeOut;
    public GameObject fadeIn;

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
            fadeOut.SetActive(true);
            LeanTween.alpha(fadeOut.GetComponent<RectTransform>(), 1f, 1f).setOnComplete(resetScene =>
            {
                fadeOut.GetComponent<RectTransform>().LeanAlpha(0f, 0f);
                fadeOut.SetActive(false);
                fadeIn.SetActive(true);
                fadeIn.GetComponent<RectTransform>().LeanAlpha(1f, 0f);
                rainGenerator.SetActive(!rainGenerator.activeSelf);
                player.transform.position = new Vector3(teleportTo.transform.position.x, teleportTo.transform.position.y, player.transform.position.z);
                FadeInScene();
            });
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            TextDisplay.SetText(InteractibleNameText);
            
            UiDisplayRectTransform.LeanAlpha(0f, 0f);
            UIDisplay.SetActive(true);
            LeanTween.alpha(UiDisplayRectTransform, 1f, .1f);
            
            isInRange = true;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            LeanTween.alpha(UiDisplayRectTransform, 0f, .1f).setOnComplete(eventa=>{

                UIDisplay.SetActive(false);

            });

            isInRange = false;

        }
    }

    void FadeInScene()
    {
        LeanTween.alpha(fadeIn.GetComponent<RectTransform>(), 0f, 1f).setOnComplete(DisablePanel =>
        {
            fadeIn.SetActive(false);
        });
    }
}
