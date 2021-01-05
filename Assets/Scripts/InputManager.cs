using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InputManager : MonoBehaviour
{
    public GameObject golfBall;
    public GameObject camera;
    public GameObject green;
    public GameObject targetPlace;
    public Slider LRSlope;
    public Slider BFSlope;
    public GameObject ballPlace;
    public GameObject camPlace;
    float staticDistance = (-29.49f) + (80.0f);
    public float curDistance = 0f;
    public float LR;
    public float BF;
    float angleLR;
    
    public PushedButton leftPB;
    public PushedButton rightPB;
    Vector3 ballDir;
    void Start()
    {
        //높이, 밑변
        angleLR = Mathf.Atan2(1000, 30);
        Debug.Log("tatatat " + angleLR);
    }
    void Update()
    {
        //Debug.Log(ballPlace.transform.position);
    }
    public void scrolling(Slider slider)
    {
        if (slider.name == "WideLength")
        {
            golfBall.transform.position = new Vector3(golfBall.transform.position.x, golfBall.transform.position.y, (-80.0f)+slider.value * staticDistance);
            camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, (-90.0f) + slider.value * staticDistance);
            curDistance =1-slider.value;
        }
        if(slider.name == "LRSlope")
        {
            camera.transform.position = camPlace.transform.position;
            golfBall.transform.position = ballPlace.transform.position;
            green.transform.rotation = Quaternion.Euler(new Vector3((BFSlope.value - 0.5f) * 5f, 0f, (slider.value-0.5f)*10f) );
            LR = slider.value;
        }
        if (slider.name == "BFSlope")
        {
            camera.transform.position = camPlace.transform.position;
            golfBall.transform.position = ballPlace.transform.position;
            green.transform.rotation = Quaternion.Euler(new Vector3((slider.value - 0.5f) * 5f, 0f, (LRSlope.value - 0.5f) * 10f));
            BF = slider.value;
        }
    }
    public void changeDir()
    {
        ballDir = new Vector3(golfBall.transform.rotation.x + 0f, golfBall.transform.rotation.y + leftPB.length + rightPB.length, golfBall.transform.rotation.z + 0f);
        golfBall.transform.rotation = Quaternion.Euler(ballDir);
    }
    
}
