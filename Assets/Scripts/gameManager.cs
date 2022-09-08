using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class gameManager : MonoBehaviour
{
    public GameObject player;
    public ParticleSystem rainGenerator;
    public Light2D global2DLight;

    public GameObject villain;
    public Sprite villainSideView;

    //public check bools:
    public bool hasDied = false;
    public bool hasWon = false;

    public bool levelChanged = false;
    public bool firstLevelPassed = false;
    public bool secondLevelPassed = false;
    public bool thirdLevelPassed = false;

    [HideInInspector]
    public bool hasLostGame = false;
    public bool hasInteractedWithDrinkOnCounter = false;
    public bool hasFlippedDeadBody = false;
    public bool hasRefusedIce = false;
    public bool hasBoughtDrinkForRegular = false;
    public bool hasCheckedDeadBody=false;

    public bool stateHasChanged = true;

    public bool isPlayerOutside=true;

    //general variables:
    public int nrOfResets = 0;


    //fadeIn/out
    [Header("Scene fade in/out")]
    public GameObject fadeOut;//transparent->black
    public GameObject fadeIn; //black->transparent

    //spawnpoints
    [Header("SpawnPoints")]
    public List<GameObject> SpawnPoints = new List<GameObject>();

    //inventory save
    private List<Item> inventorySave = new List<Item>();

    private void Awake()
    {
        fadeIn.SetActive(true);
        player.transform.position = SpawnPoints[0].transform.position;
    }

    void FadeInScene()
    {
        LeanTween.alpha(fadeIn.GetComponent<RectTransform>(), 0f, 1f).setOnComplete(DisablePanel =>
        {
            fadeIn.SetActive(false);
        });
    }

    public void TurnVillainToRight ()
    {
        villain.GetComponent<SpriteRenderer>().sprite = villainSideView;
    }

    public void ResetScene()
    {
        //GameObject tmpObj = Instantiate(blackImage, UI.transform);

        //LeanTween.moveX(testImage, 300f, 5f);
        fadeOut.SetActive(true);
        FindObjectOfType<AudioManager>().Play("reset");

        LeanTween.alpha(fadeOut.GetComponent<RectTransform>(), 1f, 1f).setOnComplete(resetScene =>
        {
            //in loc de LoadScene, teleporteaza playerul la inceputul loopului si reseteaza inventarul

            //FindObjectOfType<AudioManager>().Play("reset");

            UnityEngine.SceneManagement.SceneManager.LoadScene(1);

            //Debug.Log("Scene loaded");
        });

    }

    private void Start()
    {
        FadeInScene();

        var emission = rainGenerator.GetComponent<ParticleSystem>().emission;
        emission.rateOverTime = 50f;

        global2DLight.intensity = 0.75f;
    }

    private void Update()
    {
        //#region Scene and inventory tests
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    ResetScene();
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    Inventory.instance.AddItemOFType(0);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    Inventory.instance.ResetInventory();
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    Inventory.instance.SaveInventory(inventorySave);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    Inventory.instance.LoadInventory(inventorySave);
        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    GameObject.Find("Rain Generator").SetActive(false);
        //}
        //#endregion

        if (stateHasChanged)
        {
            if (hasDied)
                RestartGame();

            if (hasWon)
                WinGame();

            if (levelChanged)
            {
                if (firstLevelPassed)
                {
                    var emission = rainGenerator.GetComponent<ParticleSystem>().emission;
                    emission.rateOverTime = 100f;

                    global2DLight.intensity = 0.5f;
                }
                else if (secondLevelPassed)
                {
                    var emission = rainGenerator.GetComponent<ParticleSystem>().emission;
                    emission.rateOverTime = 150f;

                    global2DLight.intensity = 0.3f;
                }
                else if (thirdLevelPassed)
                {
                    var emission = rainGenerator.GetComponent<ParticleSystem>().emission;
                    emission.rateOverTime = 200f;
                }

                levelChanged = false;
            }

            stateHasChanged = false;
        }
    }

    void RestartGame ()
    {
        //Debug.Log("game has restarted :(");

        nrOfResets++;
        IncreaseInsanityLevel();
        hasDied = false;
    }

    void IncreaseInsanityLevel ()
    {
        Debug.Log("Insanity has been increased");
    }

    public void WinGame ()
    {
        //Debug.Log("Player has won!");
        fadeOut.SetActive(true);
        LeanTween.alpha(fadeOut.GetComponent<RectTransform>(), 1f, 1f).setOnComplete(resetScene =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
            
        });
       

    }

    public void HasLost()
    {
        hasLostGame = true;
    }

    public void HasIteractedWithDrinkOnCounter()
    {
        hasInteractedWithDrinkOnCounter = true;
    }

    public void HasFlippedDeadBody()
    {
        hasFlippedDeadBody = true;
    }

    public void HasRefusedIce()
    {
        hasRefusedIce = true;
    }

    public void HasBoughtDrinkForRegular()
    {
        hasBoughtDrinkForRegular = true;
    }

    public void HasCheckedDeadBody()
    {
        hasCheckedDeadBody = true;
    }



}
