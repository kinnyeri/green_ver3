﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Progress : MonoBehaviour
{
    public enum StateLevel {Start,Ready,Roll,End};
    public StateLevel state;
    // Start is called before the first frame update

    public void run(StateLevel sl)
    {
        switch (sl)
        {
            case StateLevel.Start :
                startState();
                break;
            case StateLevel.Ready :
                readyState();
                break;
            case StateLevel.Roll :
                Debug.Log("roll");
                rollState();
                break;
            case StateLevel.End:
                endState();
                break;
            default:
                Debug.Log("err");
                break;
        }
        
    }
    protected void nextState()
    {
        if (state == StateLevel.End)
        {
            state = 0;
            return;
        }
        state++;
    }

    public abstract void startState();
    public abstract void readyState();
    public abstract void rollState();
    public abstract void endState();
}
