using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GetPower : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{    /// button 눌리는 시간
    bool powerCheck;
    public float power;
    public int getInput = 0; // 0 : 게이지 올리기 전, 1: 게이지 올리는 중,2 : 다 올림, 3: stop
    public PracticeMode pm;
    public BollMove bm;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pm.state == Progress.StateLevel.Ready)
        {
            //Debug.Log("Ready Checked");
            swingPower();
        }
        
    }

    public void swingPower()
    {
        if (powerCheck)
        {
            power += 0.4f;
            //Debug.Log("power on");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (pm.state == Progress.StateLevel.Ready)
        {
            pm.hitCount++;
            powerCheck = true;
            if (getInput == 0) getInput = 1;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (pm.state == Progress.StateLevel.Ready)
        {
            powerCheck = false;
            if (getInput == 1) getInput = 2;
            bm.setVelocity(new Vector3(0, 0, power));
        }
    }
}
