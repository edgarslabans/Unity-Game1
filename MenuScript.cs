// Class containing the contols of the user interface and static variables for the game state

//Improvements 

// Calculate Accurate only work with fixed coke position at the end 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.Monetization;

public class MenuScript : MonoBehaviour
{

	public static bool SoundOn = true;
	public static bool PausePressed = false;
    public static bool gameMode2 = false;

    public static int scoreValue0 = 0;
    public static int scoreValue = 0;
    public static int scoreValue1 = 0;

    private bool nextLevelOpen = false;     // access to the next game level is enabled if true

    public static int currentLevel = 0;

    public static float levelProgress = 0;    // Progress inside the level (value 0-1)

    public  int scoreLimit0 = 1;
    public  int scoreLimit = 2;
    public  int scoreLimit1 = 1;



    public GameObject MenuUI;       // ingame menu
	public GameObject mainMenu;     // main menu
	public GameObject HSmenu;          // -- not used
	
	
	public GameObject resBut;	
	public Button pauseButtoon;
    public GameObject nextButton;
    public GameObject resetmenu;
    public GameObject resetButton;

    public GameObject dropZone1;            // drop zones to calculate accuracy of the 
    public GameObject dropZone2;
    public GameObject dropZone3;

    public static float accuracy1 = 0;        // accuracy for each drop zone 
    public static float accuracy2 = 0;
    public static float accuracy3 = 0;



    public Button soundButton;
	public Sprite SoundOnSprite;
	public Sprite SoundOffSprite;
	public Text score = null;
    public Text levelScore = null;
    public Text info = null;
    public Text menuTitle = null;

    public GameObject box;
    public GameObject glass;
    public GameObject fluidLevel;


    //private string store_id = "3490938";    //   <<<For Test purpose >>>
    private int numRestarts = 0;

	
    void Start()
    {
		//Monetization.Initialize(store_id, true);          //   <<<For Test purpose >>>
		// set the sound icon according the boolean variable
		if (SoundOn){
			soundButton.image.sprite = SoundOnSprite;
		}else{
			soundButton.image.sprite = SoundOffSprite;
		}
		mainMenu.SetActive(true);
		Time.timeScale = 0f;

    }


    void Update()
    {
		
		if(PausePressed){
			Pause();
		}
		// menu at the game over
		if (SpawnPointScript.gameOver){
			GameOverMode();
		}
		
		// updating the score
		score.text = "Score: "  + scoreValue.ToString();
        info.text = scoreValue0.ToString() + "/1     " + scoreValue.ToString() + "/"+ scoreLimit.ToString()+ "     " +scoreValue1.ToString() + "/1     ";
        if (scoreValue0 == scoreLimit0 & scoreValue == scoreLimit & scoreValue1 == scoreLimit1 & !SpawnPointScript.gameOver)
        {
            CalculateAccuracy();
            SpawnPointScript.gameOver = true;
        }


        UpdateLevelProgress();


    }

    void CalculateAccuracy()
    {
        // accuracy around pos 1
        GameObject[] objs;
        objs = GameObject.FindGameObjectsWithTag("GroundedSlice");

        foreach (GameObject slice in objs)
        {
            // Calculate accuracy in zone 
            if (slice.GetComponent<Rigidbody>().transform.position.x < 0 & accuracy1 == 0)
            {
                float locDist = Vector2.Distance(new Vector2(dropZone1.transform.position.x, dropZone1.transform.position.z), new Vector2(slice.transform.position.x, slice.transform.position.z));
                accuracy1 += locDist;
            }

            if (slice.GetComponent<Rigidbody>().transform.position.x > 0 & slice.GetComponent<Rigidbody>().transform.position.x <2 & accuracy2 == 0)
            {
                float locDist = Vector2.Distance(new Vector2(dropZone2.transform.position.x, dropZone2.transform.position.z), new Vector2(slice.transform.position.x, slice.transform.position.z));
                accuracy2 += locDist;
            }

            if (slice.GetComponent<Rigidbody>().transform.position.x > 2 & accuracy3 ==0)
            {
                float locDist = Vector2.Distance(new Vector2(dropZone3.transform.position.x, dropZone1.transform.position.z), new Vector2(slice.transform.position.x, slice.transform.position.z));
                accuracy3 += locDist;
            }
        }

        // normalizing the values

        accuracy1 = NormalizeAccuracy(accuracy1);
        accuracy2 = NormalizeAccuracy(accuracy2);
        //accuracy3 = NormalizeAccuracy(accuracy3);  // -- DISABLED -- need workaround to make flexibility


        Debug.Log("Accuracy1: " + accuracy1 + " Accuracy2: " + accuracy2 + " Accuracy3: " + accuracy3);

        levelScore.text = "Accuracy:  \n" + Mathf.FloorToInt(accuracy1).ToString()+" % / "+ Mathf.FloorToInt(accuracy2).ToString() + " % / " + Mathf.FloorToInt(accuracy3).ToString() + " %";
        if (accuracy1 > 70f & accuracy2 > 70f & accuracy3 > 70f)
        {
            nextLevelOpen= true;
            menuTitle.text = "Task finished!";

        } else
        {
            menuTitle.text = "Not good - try again!";
        }

    }


    float NormalizeAccuracy(float accur)
    {
        if (accur !=0) { }
        accur = (1 - (accur - 0.5f) / 1) * 100;
        if (accur > 100)
        {
            accur = 100;
        } else if (accur < 0)
        {
            accur = 0;

        }
        return accur;
    }



    void UpdateLevelProgress()
    {
        if (scoreValue0 == 0)
        {
            levelProgress = 0.0f;
        }

        if (scoreValue0 == 1)
        {
            levelProgress = 0.33f;
        }

        if (scoreValue >= scoreLimit)
        {
            levelProgress = 0.66f;
        }

        if (scoreValue1 >= scoreLimit1)
        {
            levelProgress = 0.99f;
        }
    }




	public void Resume()
	{
		MenuUI.SetActive(false);
		Time.timeScale = 1f;
		PausePressed = false;
		Debug.Log ("Resuming the game");
	}
	
	void Pause()
	{
		menuTitle.text = "Pause";
		if(HSmenu.activeInHierarchy)
        {
			MenuUI.SetActive(false);
		} else {
			MenuUI.SetActive(true);
		}
		Time.timeScale = 0f;

	}
	
	public void LoadMenu()
	{	
		PausePressed = false;
		MenuUI.SetActive(false);
		mainMenu.SetActive(true);
		SpawnPointScript.gameOver = false;
		Time.timeScale = 0f;

    }
	
	public void Restart()
	{
        nextButton.SetActive(false);

        //unhiding back the resume button
        resBut.GetComponent<Image>().color = new Color(255, 255, 255, 255f);
		resBut.GetComponentInChildren<Text>().text = "Resume";

        //  zeroing the values 
        SpawnPointScript.currentSlice = 0;


        // resetting the values in scripts


        DestroyAllObjects();
		scoreValue0 = 0;
        scoreValue = 0;
        scoreValue1 = 0;
        Time.timeScale = 1f;

        levelProgress = 0;
        ProgressBar.fillRatio = 0.05f; // reseting the infill ofthe progress bar

        accuracy1 = 0;        // resetting the accuracy
        accuracy2 = 0;
        accuracy3 = 0;

        nextLevelOpen = false;




    // getting the new values for in-level score value
    scoreLimit = SpawnPointScript.FoodSequenceFull[currentLevel].Length - 2;


        // removing all menus and  switches 
        mainMenu.SetActive(false);
		MenuUI.SetActive(false);
		SpawnPointScript.gameOver = false;
		PausePressed = false;
		colliderBase.resultUploaded = false;


        // instantiate box and  glass

        SpawnItemsOnPlate();


        //GameObject obj1 =  Instantiate(box, new Vector3(-0.68f, -7.9f,-1.04f), Quaternion.Euler(0f, 0, 0f)) as GameObject;
        //GameObject obj2 = Instantiate(glass, new Vector3(2.22f, -8f, -1.29f), Quaternion.Euler(0f, 0, 0f)) as GameObject;
        //GameObject obj3 = Instantiate(fluidLevel, new Vector3(2.22f, -8.06f, -1.32f), Quaternion.Euler(0f, 0, 0f)) as GameObject;

        //spawning the first slice  at all spawn points
        SpawnPointScript spw = FindObjectOfType<SpawnPointScript>();
        spw.SpawnSlice(new Vector3(spw.transform.position.x + 0f, spw.transform.position.y, spw.transform.position.z + 0.5f), true);



        // resetting the camera and spawn position to initial  -- DISABLED ---

        spw.ResetCameraAndSpawnPos();
		
		// clearing the lists of gameobjects used for height calculations
		SpawnPointScript.slicesInPile.Clear();
		numRestarts++;
		 
        /*
		if(Monetization.IsReady("video") & numRestarts%3 ==0)
		{
			ShowAdPlacementContent ad = null;
			ad = Monetization.GetPlacementContent("video") as ShowAdPlacementContent;
		
			if(ad !=null){
				ad.Show();
			} else {
				Debug.Log("ad =null"); 
			}
		}
        */
		 
	}


    public void startNextLevel()
    {
        currentLevel++;
        Debug.Log("currentLevel: "+ currentLevel);
        Restart();
    }

    public void resetGame()
    {
        currentLevel = 0;
        Restart();
        resetmenu.SetActive(false);
        resetButton.SetActive(false);
    }




    public void startGameMode2()
    {
        gameMode2 = true;
        Restart();
    }


    public void startGameMode1()
    {
        gameMode2 = false;
        Restart();
    }

    public void LoadHighScore()
	{	
		//PausePressed = false;
		MenuUI.SetActive(false);
		HSmenu.SetActive(true);
		//Time.timeScale = 0f;
	}
	
	public void ReturnToMenu()
	{	
		//PausePressed = false;
		MenuUI.SetActive(true);
		HSmenu.SetActive(false);
		//Time.timeScale = 0f;
	}
	
	
	public void pressPause(){

		if(PausePressed)
		{
		    Resume();
		}
		else 
		{
		    PausePressed = true;
            levelScore.text = " ";
        }
	}
	
	public void pressSoundButton()
	{
		if (SoundOn) {
			soundButton.image.sprite = SoundOffSprite;
			SoundOn = false;
		}	else {
			soundButton.image.sprite = SoundOnSprite;
			SoundOn = true;
		}
	}
	
	public void GameOverMode(){
		if(HSmenu.activeInHierarchy)
        {
			MenuUI.SetActive(false);
            nextButton.SetActive(false);
        } else {

            // end of the level screen
			MenuUI.SetActive(true);
           
            if (nextLevelOpen)
                nextButton.SetActive(true);
                

            // End of the game screen
            if (SpawnPointScript.FoodSequenceFull.Length == currentLevel+1)
            {
                resetmenu.SetActive(true); 
                resetButton.SetActive(true);
                MenuUI.SetActive(false);
                nextButton.SetActive(false);
            }

        }

		
		//making resume button transparent
		//resBut = GameObject.Find("ResumeButton");
		resBut.GetComponentInChildren<Text>().text = "";
		resBut.GetComponent<Image>().color = new Color(0, 0, 0, 0f);
	}
	
	void DestroyAllObjects(){
		// remove all slices after restart or new game
		GameObject[] gameObjects;
		gameObjects = GameObject.FindGameObjectsWithTag ("GroundedSlice");
    
		for(var i = 0 ; i < gameObjects.Length ; i ++)
		 {
			Destroy(gameObjects[i]);
		 }
		 
		gameObjects = GameObject.FindGameObjectsWithTag ("ingredient");
    
		for(var i = 0 ; i < gameObjects.Length ; i ++)
		{
			Destroy(gameObjects[i]);
		}

        gameObjects = GameObject.FindGameObjectsWithTag("NonFood");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }

        gameObjects = GameObject.FindGameObjectsWithTag("Soda");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }





    }


    // based on the order of items in the level the script determines which tray elements 
    // (box, or glass) shoul be spawned at each level

    void SpawnItemsOnPlate()
    {
        int[] levelContent = SpawnPointScript.FoodSequenceFull[currentLevel];


        if (levelContent[0] == 8)
        {
            GameObject obj1 = Instantiate(box, new Vector3(-0.68f, -7.9f, -1.04f), Quaternion.Euler(0f, 0, 0f)) as GameObject;
        }

        if (levelContent[levelContent.Length-1] == 9)
        {
            GameObject obj2 = Instantiate(glass, new Vector3(2.22f, -8f, -1.29f), Quaternion.Euler(0f, 0, 0f)) as GameObject;
            GameObject obj3 = Instantiate(fluidLevel, new Vector3(2.22f, -8.06f, -1.32f), Quaternion.Euler(0f, 0, 0f)) as GameObject;
        }



        if (levelContent[0] == 9)
        {
            GameObject obj2 = Instantiate(glass, new Vector3(-0.68f, -8f, -1.04f), Quaternion.Euler(0f, 0, 0f)) as GameObject;
            GameObject obj3 = Instantiate(fluidLevel, new Vector3(-0.68f, -8.06f, -1.04f), Quaternion.Euler(0f, 0, 0f)) as GameObject;
        }

        if (levelContent[levelContent.Length - 1] == 8)
        {
            GameObject obj1 = Instantiate(box, new Vector3(2.22f, -8f, -1.29f), Quaternion.Euler(0f, 0, 0f)) as GameObject;

        }








    }



}
