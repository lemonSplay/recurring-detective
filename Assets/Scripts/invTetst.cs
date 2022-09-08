using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class invTetst : MonoBehaviour
{
    //public Item itemTest;

    public GameObject fadeOut;
    public GameObject fadeIn;
    
    public List<GameObject> SpawnPoints = new List<GameObject>();
    
    private List<Item> inventorySave = new List<Item>();

    void Awake()
    {
        fadeIn.SetActive(true);
        transform.position = SpawnPoints[0].transform.position;
    }

    void Start()
    {
        LeanTween.alpha(fadeIn.GetComponent<RectTransform>(), 0f, 1f).setOnComplete(DisablePanel =>
        {
            fadeIn.SetActive(false);
        });
    }

    public void ResetScene()
    {
        //GameObject tmpObj = Instantiate(blackImage, UI.transform);

        //LeanTween.moveX(testImage, 300f, 5f);
        fadeOut.SetActive(true);
        FindObjectOfType<AudioManager>().Play("reset");

        LeanTween.alpha(fadeOut.GetComponent<RectTransform>(),1f,1f).setOnComplete(resetScene =>
        {
            //in loc de LoadScene, teleporteaza playerul la inceputul loopului si reseteaza inventarul

            //FindObjectOfType<AudioManager>().Play("reset");

            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            
            Debug.Log("Scene loaded");
        });

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            ResetScene();
        }
       
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.instance.AddItemOFType(0);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Inventory.instance.ResetInventory();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Inventory.instance.SaveInventory(inventorySave);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Inventory.instance.LoadInventory(inventorySave);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject.Find("Rain Generator").SetActive(false);
        }

    }
}
