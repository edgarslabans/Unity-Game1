using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ProgressBar : MonoBehaviour
{

    public Image barImage;
    public float FillSpeed = 0.00000005f;       // fill speed does not work
    private float progressLevel;

    public static float fillRatio;


    private void Awake()
    {
        barImage.GetComponent<Image>().fillAmount = 0.01f;

    }



    private void Update()
    {

        if (fillRatio <= MenuScript.levelProgress)
        {
            fillRatio += FillSpeed * Time.deltaTime;
            barImage.GetComponent<Image>().fillAmount = fillRatio;
        }

        if (MenuScript.levelProgress == 0)
        {
            fillRatio =0;
            barImage.GetComponent<Image>().fillAmount = fillRatio;
        }


    }
    

    /*
    // Start is called before the first frame update
    void Start()
    {
        IncrementProgress(0.75f);
    }

    // Update is called once per frame
    void Update()
    {
        if (slider.value < targetProgress)
            slider.value += FillSpeed * Time.deltaTime;
    }

    public void IncrementProgress(float newProgress)
    {
        targetProgress =  slider.value + newProgress;
    }

    */
}
