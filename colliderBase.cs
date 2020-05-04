using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colliderBase : MonoBehaviour
{
	//public Image imgLife1;
	//public Text text1;
	
    // Start is called before the first frame update
	public static bool resultUploaded = false;
	
	AudioSource source;

	
    void Start()
    {
		source = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
	


	void OnCollisionEnter(Collision col)
	{
        // Game over  DISABLED

        //SpawnPointScript.gameOver = true;
		
		
		if (!resultUploaded){
			int randNum = Random.Range(1,999);
			string playerName = "Player" + randNum;
			
			//HighscoreTable hst = FindObjectOfType<HighscoreTable>();
			//StartCoroutine(hst.UploadHighscore(playerName,MenuScript.scoreValue ));
			//StartCoroutine(hst.DownloadHighscore());

			resultUploaded = true;
		}
		
		if(MenuScript.SoundOn == true){
			source.Play();
		}
		
		
	}
	

	
	
}
