// slice class is responsibe for destroying the slice when it reaches right side
// it also contains onCollision method which spawns new slice when current one collides with something
// after collision the collision detecion method is simplified

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceScript : MonoBehaviour
{
	AudioSource source;

    
	
	
	// after defined score lower slices will be freezed by one at every score increase
	public static int stabilityAssist = 50;
	
	
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
		//stabilityAssist = 5;
    }

    // Update is called once per frame
    void Update()
    {

        // removes all constrains from grounded slices after game is over
            if ( SpawnPointScript.gameOver){
			    this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		    }


    }
	

	
	void OnCollisionEnter(Collision col)
	{
        
        if (this.transform.gameObject.tag == "ingredient" & col.gameObject.tag != "ingredient"  &  !SpawnPointScript.gameOver){
			
		    // play sound if it is enabled
		    if(MenuScript.SoundOn == true){
			    source.Play();
		    }

		    this.transform.gameObject.tag = "GroundedSlice";
		


		    // adding gameobject to the list of grounded slices to calculate highest point of the sanwich
		    SpawnPointScript.slicesInPile.Add(this.transform.gameObject);


             // disable on the fly boolean. User input is available again
             SpawnPointScript.sliceOntheFly = false;


                // spawning new slice on after collision with the base

             SpawnPointScript spw = FindObjectOfType<SpawnPointScript>();
		     GameObject spawnP = GameObject.Find("Spawn_point");


             // Using the x coordinate of the slice to spawn next one
             Vector3 SpawnCoord = new Vector3(this.transform.position.x, spawnP.transform.position.y, spawnP.transform.position.z);

            if (gameObject.name != "Free(Clone)")
            {
                spw.SpawnSlice(SpawnCoord, true);
                MenuScript.scoreValue += 1;
            }
            else
            {
                // seperate spawn technique for the bunch of free
                SpawnPointScript.numPotatoesCollided++;
                if (SpawnPointScript.numPotatoesCollided == 6)
                {
                    Debug.Log("Request to spawn fries sent - onCollision");
                    spw.SpawnSlice(SpawnCoord, true);
                    SpawnPointScript.numPotatoesCollided = 0;     //--- Free spawn DISABLED
                    MenuScript.scoreValue0 += 1;
                }

            }





         // stability assist feature	
		 FreezeLowerSlices();
         SimplifiedCollisionDetector();

      
        }

	}









    public IEnumerator FreezeMovementAfterTime(){
		yield return new WaitForSeconds(10.0f);
		this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
	}

    // this method simplifies the CollisionDetectionMode for the slices alrady on the ground
    void SimplifiedCollisionDetector()
    {
        if (MenuScript.scoreValue > 2)
        {
            GameObject ob = SpawnPointScript.slicesInPile[MenuScript.scoreValue-1]; // last number -score
            ob.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
        }
    }


    // freeze slices on the ground beyond certain score
    void FreezeLowerSlices(){
		if(MenuScript.scoreValue > stabilityAssist){
			GameObject o = SpawnPointScript.slicesInPile[MenuScript.scoreValue-1-stabilityAssist];
			o.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;	
		}
	}

    public void CheckJust()
    {
        Debug.Log("TEST");
    }


}
