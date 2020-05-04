// issues glassIsFull variable doesn`t work

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleScript : MonoBehaviour
{
    GameObject level;

    private Vector3 startPosLevel;
    public float waitTimeAfterFilling = 1.0f;
    public float fillSpeed = 0.2f;

    private float minDist = 5f;  // minimal distance where cola will be filled

    private bool fillingSoda = false;


    GameObject streamS;
    ParticleSystem ps;

    // Start is called before the first frame update
    void Start() 
    {
        level = GameObject.Find("FluidLevel(Clone)");
        startPosLevel = level.transform.position;

        ps = gameObject.GetComponentInChildren<ParticleSystem>();
        var emission = ps.emission;
        emission.rateOverTime = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) & !fillingSoda & level != null )
        {
            var emission = ps.emission;
            emission.rateOverTime = 50f;

            // calculate distance from bottle to glase
            float dist = Vector3.Distance(this.transform.position, startPosLevel);

            // move the level of liquid 
            if (level.transform.position.y < -7.35 & dist < minDist)
            {
                level.transform.Translate(0f, fillSpeed * Time.deltaTime, 0f);
            }
            // freeze the bottle
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        if (level == null)
        {
            level = GameObject.Find("FluidLevel(Clone)");
            startPosLevel = level.transform.position;
        }


        if (Input.GetMouseButtonUp(0))
        {
            //fillingSoda = true;
            StartCoroutine(RemoveBottle());
            SpawnPointScript.sliceOntheFly = false;

        }


    }

    

    // Coroutine for dalay
    IEnumerator RemoveBottle()
    {
        // stopping the stream
        var emission = ps.emission;
        emission.rateOverTime = 0f;


        yield return new WaitForSeconds(waitTimeAfterFilling);

        // removeing constraints 
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        //Kicking the botel  away
        Quaternion rotation = Quaternion.Euler(0f, 80f, 0f);
        Vector3 direction = rotation * Vector3.forward;
        this.GetComponent<Rigidbody>().AddForce(direction * 200);
        this.GetComponent<Rigidbody>().AddRelativeTorque(0, 0, -500f);

        MenuScript.scoreValue1 += 1;

    }

    
 

    public void ResetTheBottleScript()
    {

        if (MenuScript.scoreValue1 != 0) { 
            level.transform.position = startPosLevel;
            fillingSoda = false;
        }
    }

    
}
