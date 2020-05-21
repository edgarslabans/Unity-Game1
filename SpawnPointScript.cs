//This scrpt is contains the methods for instantiating new slices and adding gravity to them by a screen click
// the first slice is instantiated by Restart() command in MenuScript

//Issues: 1) the clicks will be only registered if score >0 (thus, the first auto falling slice is important)
// The spawn positions should not be rigid but determined by FoodSequenceArray

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SpawnPointScript : MonoBehaviour
{
	
	public GameObject mainCam;
	
	public GameObject[ ] slices = new GameObject[11] ;
	

	public int flySpeed = 70;            // movement speed for the slice 
    public float yRotAmpl = 30f;        // amplitude of possible initial rotatioin around Y (GameMode2)
    public float zShiftAmpl = 0.3f;     // amplitude for the possible shift in Z direction (GameMode2)



    public static bool gameOver;
    public static bool bottleOpened;                      // when true bottle is open and streaming particle stream
    public static int numPotatoesCollided = 0;           //counter for the slices of the fries on the ground 
    public static bool sliceOntheFly = false;            // check if the slice is falling, if true, the touch input will be disabled


    public bool check;

	private bool cooldown = false;		// cooldown for the button pressed
	private bool timeToRiseCamera = false;

    private int randNum;                   // random number to select slice

    private int lastSliceNum;
	private Vector3 newCameraPosition;
	private Vector3 initialCameraPosition;
	private Vector3 initialSpawnPosition;
	
	public static List<GameObject> slicesInPile = new List<GameObject>();

    // boundaries for click boxes
    private Rect bounds1;
    private Rect bounds2;
    private Rect bounds3;

    // sequence of the slices for 
    public static int[] sandwichSeq = new int[] {8, 0, 9};
    //public static int[,] FoodSequenceFull = new int[,] { { 8, 0, 9 }, { 8,0,2,0,9}, { 8, 0, 3,2,4, 0, 9 }, { 7, 8 } };

    public static int[][] FoodSequenceFull =
    {
    new int[] {  9, 0, 8 },
    new int[] {  8,0,2,0,9 },
    new int[] {  8, 0, 3,2,4, 0, 9}
    };  

    public static int currentSlice = 0;




    void Start(){
		initialCameraPosition = mainCam.transform.position;
		initialSpawnPosition = this.transform.position;

        Debug.Log("SpawnPointScript.FoodSequenceFull.Length "+ SpawnPointScript.FoodSequenceFull.Length);
        //bottleOpened = false;
        check = bottleOpened;

        bounds1 = new Rect(0, 0, Screen.width, Screen.height * 0.9f);
        bounds2 = new Rect(Screen.width / 3, 0, Screen.width * 2 / 3, Screen.height * 0.9f);
        bounds3 = new Rect(Screen.width * 2 / 3, 0, Screen.width, Screen.height * 0.9f);

    }

    // The update method checks if the click Touch happens inside the given boundaries
    // The camera position is also updated to follow the top slice
    void Update()
    {

        if ((Input.touchCount > 0 || Input.GetMouseButtonDown(0)) & !gameOver & !cooldown  & bounds1.Contains(Input.mousePosition) & !MenuScript.PausePressed & !sliceOntheFly & Time.timeScale==1f)
        {         
            MakeSlicesFall(0);
            sliceOntheFly = true;
        }

        if (timeToRiseCamera){
            // the last number - speed of rising camera
			mainCam.transform.position = Vector3.Lerp (mainCam.transform.position, newCameraPosition, 0.005f);
		}
	}
	
    // cooldown for screen touch
	void ResetCooldown()
	{
    cooldown = false;
	}

	// making slices fall by applying gravity 
	public void MakeSlicesFall(int pos){

        

        GameObject[] objs ;
		objs = GameObject.FindGameObjectsWithTag("ingredient");
		foreach(GameObject slice in objs) {

        slice.GetComponent<Rigidbody>().useGravity = true;
        slice.GetComponent<Rigidbody>().velocity = Vector3.zero;
     
        }

        if (GameObject.FindGameObjectsWithTag("Soda").Length != 0)
        {
            // open cola bottle
            bottleOpened = true;

        }



        // also looking for soda bottle, and applying the stream function


    }


    // method for spawning new slice. Arguments: coordinate (where to spawn) and forward moving force (if true)
    // the method is activated when the previous slice collides with the objects on the ground of reach the boundary on the right

    public void SpawnSlice(Vector3 origin1, bool force){
        // generate random number to select slice for array and storing the last number to prevent repeated slices

        //randNum = sandwichSeq[currentSlice];

        randNum = FoodSequenceFull[MenuScript.currentLevel][currentSlice];
        currentSlice++;




        // cooldown for the button for number of seconds
        Invoke("ResetCooldown",0.5f);
		cooldown = true;
		

        // Instantiate slice. In the case of GameMode2 the random rotation an position shif is added

        float yRot = 0;
        float zShift = 0;
        if (MenuScript.gameMode2)
        {
            yRot = Random.Range(-yRotAmpl, yRotAmpl);
            zShift = Random.Range(-zShiftAmpl, zShiftAmpl);
        }


        origin1 = this.transform.position;


        //  different spawn angles for sandwich pieces
        if (randNum < 6)
        {


            GameObject bulletInstance = Instantiate(slices[randNum], new Vector3(origin1.x+1.7f, origin1.y, origin1.z + zShift), Quaternion.Euler(0f, 0, 0f)) as GameObject;

            //bulletInstance.transform.rotation = new Quaternion.Euler(0f, yRot + 45f, 90f);
            bulletInstance.transform.Rotate(0f, yRot + 0f, 90f);       // , Space.Self


            Quaternion rotation = Quaternion.Euler(0f, 90f, 0f);
            Vector3 direction = rotation * Vector3.right;
            bulletInstance.GetComponent<Rigidbody>().AddForce(direction * flySpeed);
            bulletInstance.GetComponent<Rigidbody>().useGravity = false;
        }


        // spawning additional intems for potatoes
        if (randNum  == 8)
        {
            Debug.Log("Spawning fries");
            GameObject bulletInstance1 = Instantiate(slices[randNum], new Vector3(origin1.x, origin1.y, origin1.z + zShift), Quaternion.Euler(0f, 0, 0f)) as GameObject;
            GameObject bulletInstance2 = Instantiate(slices[randNum], new Vector3(origin1.x+0.4f, origin1.y, origin1.z + zShift), Quaternion.Euler(0f, 0, 0f)) as GameObject;
            GameObject bulletInstance3 = Instantiate(slices[randNum], new Vector3(origin1.x+0.4f, origin1.y, origin1.z + zShift+0.4f), Quaternion.Euler(0f, 0, 0f)) as GameObject;
            GameObject bulletInstance4 = Instantiate(slices[randNum], new Vector3(origin1.x, origin1.y, origin1.z + zShift+0.4f), Quaternion.Euler(0f, 0, 0f)) as GameObject;
            GameObject bulletInstance5 = Instantiate(slices[randNum], new Vector3(origin1.x, origin1.y, origin1.z + zShift+0.8f), Quaternion.Euler(0f, 0, 0f)) as GameObject;
            GameObject bulletInstance6 = Instantiate(slices[randNum], new Vector3(origin1.x+0.4f, origin1.y, origin1.z + zShift+0.8f), Quaternion.Euler(0f, 0, 0f)) as GameObject;
            bulletInstance1.GetComponent<Rigidbody>().useGravity = false;
            bulletInstance2.GetComponent<Rigidbody>().useGravity = false;
            bulletInstance3.GetComponent<Rigidbody>().useGravity = false;
            bulletInstance4.GetComponent<Rigidbody>().useGravity = false;
            bulletInstance5.GetComponent<Rigidbody>().useGravity = false;
            bulletInstance6.GetComponent<Rigidbody>().useGravity = false;

            Quaternion rotation = Quaternion.Euler(0f, 90f, 0f);
            Vector3 direction = rotation * Vector3.right;
            bulletInstance1.GetComponent<Rigidbody>().AddForce(direction * flySpeed);
            bulletInstance2.GetComponent<Rigidbody>().AddForce(direction * flySpeed);
            bulletInstance3.GetComponent<Rigidbody>().AddForce(direction * flySpeed);
            bulletInstance4.GetComponent<Rigidbody>().AddForce(direction * flySpeed);
            bulletInstance5.GetComponent<Rigidbody>().AddForce(direction * flySpeed);
            bulletInstance6.GetComponent<Rigidbody>().AddForce(direction * flySpeed);
        }

        if (randNum == 9)
        {

            GameObject bulletInstance = Instantiate(slices[randNum], new Vector3(origin1.x+3.2f, origin1.y, origin1.z + zShift), Quaternion.Euler(0f, 0, 0f)) as GameObject;
            //bulletInstance.transform.rotation = new Quaternion.Euler(0f, yRot + 45f, 90f);
            bulletInstance.transform.Rotate(0f, yRot + 0f, 0f);       // , Space.Self


            Quaternion rotation = Quaternion.Euler(0f, 90f, 0f);
            Vector3 direction = rotation * Vector3.right;
            bulletInstance.GetComponent<Rigidbody>().AddForce(direction * flySpeed);
            bulletInstance.GetComponent<Rigidbody>().useGravity = false;
        }


        // updating of the camera position after every spawned slice
        UpdateCameraPos();
        //return bulletInstance;
	}

    // the method returns random number based on requested slice spawn location
    private int ProvideRandnum(Vector3 loc)
    {
        if (loc.x < 0)
        {
            randNum = 8;

        }
        else if(loc.x >0 & loc.x < 1.5)
        {
            randNum = Random.Range(0, 5);
            // protection agains repeated slices
            if (randNum == lastSliceNum)
            {
                randNum = Random.Range(0, slices.Length);
            }

            // making first slice always bread
            if (MenuScript.scoreValue == 0)
            {
                randNum = 0;
            }
            lastSliceNum = randNum;
        }
        else if(loc.x > 2)
        {
            randNum = 9;
        }

        return randNum;
    }







	public void ResetCameraAndSpawnPos()
	{
		this.transform.position = initialSpawnPosition;
		mainCam.transform.position = initialCameraPosition;
		timeToRiseCamera = false;
	}

    
    public void UpdateCameraPos()
    {
        // updating spawn point coordinate after pile height increase
        if (MenuScript.scoreValue > 3 & !gameOver)
        {

            Vector3 topOfPile = FindThePileTop(slicesInPile);
            // setting new camera position, 
            // camera x position changed only if the top of the slice significantly shifted
            if (Mathf.Abs(topOfPile.x - mainCam.transform.position.x) >= 0.6f)
            {
                newCameraPosition = new Vector3(topOfPile.x, topOfPile.y + 4.5f, mainCam.transform.position.z);
            }
            else
            {
                newCameraPosition = new Vector3(mainCam.transform.position.x, topOfPile.y + 4.5f, mainCam.transform.position.z);
            }
            timeToRiseCamera = true;
            //increasing the spawn point height 
            this.transform.position = new Vector3(transform.position.x, topOfPile.y + 3.0f, transform.position.z);
        }
    }


    // method to find the highest slice and return its coordinate
    // it is used to update spawnPoint and Camera heights
	
	Vector3 FindThePileTop (List<GameObject> pile){
		Vector3 currentH = new Vector3 (0,-10,0);
		foreach(GameObject slice in pile) {
			Vector3 newHigh = slice.GetComponent<Rigidbody>().transform.position;
			if (newHigh.y > currentH.y){
				currentH = newHigh;
			}
		}
		return currentH;
	}





}
