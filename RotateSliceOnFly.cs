// the script responsibe for rotation of the slice while flying
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSliceOnFly : MonoBehaviour
{
    private Vector3 _startPosition;
    private Vector3 _startRotation;
    private bool stepChange = false;
    private bool stepChangeZ = false;
    private bool rotStepChange = false;

    private bool MoveZAxixStepChange = false;


    private float torqForce;
    private float rotaionPeriod;    

    private float verticalForce = 0.1f;
    private double verticalAmplitude = 0.1;

    private float AmplitudeZ = 4f;


    private int rotationAxis;
    private GameObject spawn;

    // Start is called before the first frame update
    void Start(){
         
        spawn = GameObject.Find("Spawn_point");
        _startPosition = spawn.transform.position;

        // start Z direction change coriutine
        InvokeRepeating("SwithMoveZdirection", 3f, 3f);

        if (MenuScript.gameMode2)
        {
            
            // add initial vertical movement of the slice
            this.GetComponent<Rigidbody>().AddForce(Vector3.down * -verticalForce, ForceMode.VelocityChange);

            // defining random rotation parameters 
            rotationAxis = Random.Range(0, 2);                  // between 0 aand 2
            torqForce = Random.Range(2, 5);
            rotaionPeriod = Random.Range(0.5f, 4.0f);

            //Debug.Log("Rotating axis: " + rotationAxis);

            // give initial rotation to maintain balansed swing of the slice around axis
            Invoke("SwithRotationDirectionOnce", rotaionPeriod * 0.5f);
            // starting repeating rotation coroutine (invoke delay, time intervals between calls)
            InvokeRepeating("SwithRotationDirection", 0.5f, rotaionPeriod);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (MenuScript.gameMode2)
        {
            // if vertical displacement exceeds verticalAmplitude value the slice is being redirectd in opposite way
            if (transform.position.y > _startPosition.y + verticalAmplitude & !stepChange)
            {
                float yVelocity = this.GetComponent<Rigidbody>().velocity.y;

                this.GetComponent<Rigidbody>().AddForce(Vector3.down * yVelocity * 2, ForceMode.VelocityChange);
                stepChange = true;
            }

            if (transform.position.y < _startPosition.z - verticalAmplitude & stepChange)
            {
                float yVelocity = this.GetComponent<Rigidbody>().velocity.y;
                //Debug.Log(" changing direction 2, y_vel: : " + yVelocity);
                this.GetComponent<Rigidbody>().AddForce(Vector3.down * yVelocity * 2, ForceMode.VelocityChange);
                stepChange = false;
            }
        }
        
        // return slice when it reaches the limit z position (Except group of fries)
        if (transform.position.z < _startPosition.z - AmplitudeZ & !stepChangeZ & gameObject.name != "Free(Clone)")
        {
            float zVelocity = this.GetComponent<Rigidbody>().velocity.z;
            this.GetComponent<Rigidbody>().AddForce(Vector3.back * zVelocity * 2, ForceMode.VelocityChange);
            stepChangeZ = true;
        }

        if (transform.position.z > _startPosition.z & stepChangeZ & gameObject.name != "Free(Clone)")
        {
            float zVelocity = this.GetComponent<Rigidbody>().velocity.z;
           this.GetComponent<Rigidbody>().AddForce(Vector3.back * zVelocity * 2, ForceMode.VelocityChange);
            stepChangeZ = false;
        }
        
    }



    void FindAndReturnAllFries()
    {
        GameObject[] objs;
        objs = GameObject.FindGameObjectsWithTag("Free");
        foreach (GameObject slice in objs)
        {
            float vel = slice.GetComponent<Rigidbody>().velocity.z;
            slice.GetComponent<Rigidbody>().AddForce(Vector3.back * vel * 2, ForceMode.VelocityChange);
            Debug.Log("Returning fries " );
        }


    }



    // the method zeroes angular velocity and adds torq forcre every time step 
    void SwithRotationDirection()
    {

        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        switch (rotationAxis)
        {
            case 0:
                
                if (!rotStepChange)
                {
                    //Debug.Log("Rotating 1, Angle(x): " + transform.rotation.eulerAngles.x);
                    this.GetComponent<Rigidbody>().AddRelativeTorque(0, torqForce, 0);
                    rotStepChange = true;
                }
                else
                {
                    //Debug.Log("Rotating 2, Angle:(x) " + transform.rotation.eulerAngles.x);
                    this.GetComponent<Rigidbody>().AddRelativeTorque(0, -torqForce, 0);
                    rotStepChange = false;
                }
                break;
            case 1:
                
                if (!rotStepChange)
                {
                    //Debug.Lo g("Rotating 1, Angle(z): " + transform.rotation.eulerAngles.z);
                    this.GetComponent<Rigidbody>().AddRelativeTorque(0, 0, torqForce);
                    rotStepChange = true;
                }
                else
                {
                    //Debug.Log("Rotating 2, Angle(z): " + transform.rotation.eulerAngles.z);
                    this.GetComponent<Rigidbody>().AddRelativeTorque(0, 0, -torqForce);
                    rotStepChange = false;
                }
                break;


            default:
                Debug.Log("Default case"); ;
                break;

        }
    

    }

    void SwithRotationDirectionOnce()
    {
        
        switch (rotationAxis)
        {
            case 0:

               this.GetComponent<Rigidbody>().AddRelativeTorque(0, -torqForce, 0);
                break;
            case 1:
                this.GetComponent<Rigidbody>().AddRelativeTorque(0, 0, -torqForce);
                break;

            default:
                Debug.Log("Default case"); ;
                break;
        }


    }



    // the method zeroes angular velocity and adds torq forcre every time step 
    void SwithMoveZdirection()
    {

        float zVelocity = this.GetComponent<Rigidbody>().velocity.z;
        //Debug.Log("SwithMoveZdirection triggered");


        if (!MoveZAxixStepChange & gameObject.name == "Free(Clone)")
            {
            //Debug.Log("Rotating 1, Angle(x): " + transform.rotation.eulerAngles.x);
            this.GetComponent<Rigidbody>().AddForce(Vector3.back * zVelocity * 2, ForceMode.VelocityChange);
            MoveZAxixStepChange = true;
            }
        else if (gameObject.name == "Free(Clone)")
            {
            //Debug.Log("Rotating 2, Angle:(x) " + transform.rotation.eulerAngles.x);
            this.GetComponent<Rigidbody>().AddForce(Vector3.back * zVelocity * 2, ForceMode.VelocityChange);
            MoveZAxixStepChange = false;
            }


    }

}



