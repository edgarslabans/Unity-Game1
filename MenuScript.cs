// Class containing the contols of the user interface and static variables for the game state
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

    public static int currentLevel = 0;

    public static float levelProgress = 0;    // Progress inside the level (value 0-1)

    public  int scoreLimit0 = 1;
    public  int scoreLimit = 2;
    public  int scoreLimit1 = 1;



    public GameObject MenuUI;
	public GameObject mainMenu;
	public GameObject HSmenu;
	
	
	public GameObject resBut;	
	public Button pauseButtoon;
    public GameObject nextButton;
    public GameObject resetmenu;
    public GameObject resetButton;




    public Button soundButton;
	public Sprite SoundOnSprite;
	public Sprite SoundOffSprite;
	public Text score = null;
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
        if (scoreValue0 == scoreLimit0 & scoreValue == scoreLimit & scoreValue1 == scoreLimit1)
        {
            SpawnPointScript.gameOver = true;
        }


        UpdateLevelProgress();


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




        // getting the new values for in-level score value
        scoreLimit = SpawnPointScript.FoodSequenceFull[currentLevel].Length - 2;


        // removing all menus and  switches 
        mainMenu.SetActive(false);
		MenuUI.SetActive(false);
		SpawnPointScript.gameOver = false;
		PausePressed = false;
		colliderBase.resultUploaded = false;
		

        // instantiate box and  glass

        GameObject obj1 =  Instantiate(box, new Vector3(-0.68f, -7.9f,-1.32f), Quaternion.Euler(0f, 0, 0f)) as GameObject;
        GameObject obj2 = Instantiate(glass, new Vector3(2.22f, -8f, -1.29f), Quaternion.Euler(0f, 0, 0f)) as GameObject;
        GameObject obj3 = Instantiate(fluidLevel, new Vector3(2.22f, -8.06f, -1.32f), Quaternion.Euler(0f, 0, 0f)) as GameObject;

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
			MenuUI.SetActive(true);
            nextButton.SetActive(true);

            if (SpawnPointScript.FoodSequenceFull.Length == currentLevel+1)
            {
                resetmenu.SetActive(true); 
                resetButton.SetActive(true);
                MenuUI.SetActive(false);
                nextButton.SetActive(false);


            }

        }

		menuTitle.text = "Task finished!";
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


}
