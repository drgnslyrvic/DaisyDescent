using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int daisyCount = 0;
    private int winCount; //daisies required to win round
    public TextMeshProUGUI countText;
    public TextMeshProUGUI islandText;
    public Slider timerObj;

    private float levelTimer;
    private float levelTimerMax;

    public GameObject player;
    public GameObject mower;
    private bool inProgress;
    public GameObject daisiesParent;
    public GameObject log;

    public GameObject loseScreen;
    public TextMeshProUGUI youScored;
    public int levelNumber;

    public Animator cameraAnim;
    private int cameraAnimNum = 0;
    public AudioClip pickup;

    private void Start()
    {
        LevelStart();
        levelNumber = 1;
    }

    private void Update()
    {
        if (inProgress  == true)
        {
            levelTimer -= Time.deltaTime;
            timerObj.value = (levelTimer / levelTimerMax);
        }
        countText.text = daisyCount.ToString() + "/8";
        islandText.text = levelNumber.ToString();

        if(daisyCount >= winCount)
        {
            cameraAnimNum = 0;
            cameraAnim.SetTrigger("reset");
            inProgress = false;
            mower.GetComponent<BoxCollider>().enabled = false;
            mower.GetComponent<Animator>().speed = 0;
            player.GetComponent<CharacterController>().canMove = false;
            
            //Cutscene plays here
            StartCoroutine("NextLevelCutscene");
            
        }
        if (levelTimer <= 0.1)
        {
            cameraAnimNum = 0;
            cameraAnim.SetTrigger("reset");
            mower.GetComponent<Animator>().speed = 0;
            player.GetComponent<CharacterController>().canMove = false;
            inProgress = false;
            mower.GetComponent<BoxCollider>().enabled = false;
            youScored.text = ("You Reached Island " + levelNumber + "!");
            loseScreen.SetActive(true);
        }

        if(levelTimer <= (levelTimerMax / 2) && cameraAnimNum == 0 && levelTimer >= 1)
        {
            cameraAnim.SetTrigger("shakeLight");
            cameraAnimNum = 1;
        }
        if (levelTimer <= (levelTimerMax / 6) && cameraAnimNum == 1)
        {
            cameraAnim.SetTrigger("shakeHeavy");
            cameraAnimNum = 2;
        }

    }

    void SpawnDaisies()
    {
            List<int> indexes = new List<int>();
            List<Transform> items = new List<Transform>();
            for (int i = 0; i < daisiesParent.transform.childCount; ++i)
            {
                indexes.Add(i);
                items.Add(daisiesParent.transform.GetChild(i));
            }

            foreach (var item in items)
            {
                item.SetSiblingIndex(indexes[Random.Range(0, indexes.Count)]);
            }
        if(levelNumber <= 5)
        {
            for (int i = 0; i < 15; i++)
            {
                daisiesParent.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        if (levelNumber > 5 && levelNumber <= 7)
        {
            for (int i = 0; i < 14; i++)
            {
                daisiesParent.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        if (levelNumber > 7)
        {
            for (int i = 0; i < 12; i++)
            {
                daisiesParent.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        if (levelNumber > 10 && levelNumber < 15)
        {
            levelTimer = 10;
            levelTimerMax = levelTimer;
        }
        if (levelNumber >= 15 && levelNumber < 20)
        {
            levelTimer = 8;
            levelTimerMax = levelTimer;
        }
        if (levelNumber >= 20)
        {
            levelTimer = 6;
            levelTimerMax = levelTimer;
        }

    }
    void SpawnMower()
    {
        mower.SetActive(false);
        if(levelNumber >= 4)
        {
            mower.SetActive(true);
            mower.transform.position = new Vector3(-8.43f, 0, -0.93f);
            mower.GetComponent<Animator>().speed = 1;
            int x = Random.Range(1, 3);
            if (x == 1)
            {
                mower.GetComponent<Animator>().SetTrigger("anim1");
            }
            if (x == 2)
            {
                mower.GetComponent<Animator>().SetTrigger("anim2");
            }
            mower.GetComponent<BoxCollider>().enabled = true;
            
        }
        
    }

    void SpawnLog()
    {
        log.SetActive(false);
        if ((levelNumber >= 2 && levelNumber < 4) || levelNumber >= 6)
        {
            
            int y = Random.Range(1, 4);
            if (y == 1)
            {
                log.transform.position = new Vector3(1.72f, 0, 1.9f);
                log.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (y == 2)
            {
                log.transform.position = new Vector3(2.79f, 0, -6.97f);
                log.transform.rotation = Quaternion.Euler(0, 48.68f, 0);
            }
            if (y == 3)
            {
                log.transform.position = new Vector3(-1.5f, 0, -7.03f);
                log.transform.rotation = Quaternion.Euler(0, 69.64f, 0);
            }
            log.SetActive(true);
        }
        
    }

    public void LevelStart()
    {
        cameraAnimNum = 0;
        cameraAnim.SetTrigger("reset");
        loseScreen.SetActive(false);
        levelNumber = 1;
        levelTimer = 15;
        levelTimerMax = levelTimer;
        SpawnDaisies();
        player.transform.position = new Vector3(0, 0.81f, 0);
        SpawnMower();
        SpawnLog();
        daisyCount = 0;
        winCount = 8;
        
        inProgress = true;
        player.GetComponent<CharacterController>().canMove = true;
    }

    private void NextLevel()
    {
        cameraAnimNum = 0;
        cameraAnim.SetTrigger("reset");
        StopCoroutine("NextLevelCutscene");
        daisyCount = 0;
        levelNumber++;
        levelTimer = 15;
        levelTimerMax = levelTimer;
        SpawnDaisies();
        player.transform.position = new Vector3(0, 0.81f, 0);
        SpawnMower();
        SpawnLog();
        winCount = 8;
        
        inProgress = true;
        player.GetComponent<CharacterController>().canMove = true;
    }

    IEnumerator NextLevelCutscene()
    {
        yield return new WaitForSeconds(2);
        NextLevel();
    }

    public void PickupSound()
    {
        GetComponent<AudioSource>().PlayOneShot(pickup);
    }
}
