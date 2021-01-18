using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticGreenMode : Progress
{
    private System.Random random;

    public AutoBallMove abm;
    
    int numOfGreen = 0;
    
    public GameObject ball;
    public GameObject cameraPos;
    public GameObject startPos;
    public GameObject startCameraPos;

    public GameObject hole;

    public testTerraiin tt;

    public Vector3 userVeloc;

    public Vector3 stoppedPos;

    float angle;

    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();

        state = 0;

        hole.transform.position = new Vector3(random.Next(100, 200), 3.1f, random.Next(100, 200));
        startPos.transform.position = new Vector3(random.Next(100, 200), 8f, random.Next(100, 200));
    }

    // Update is called once per frame
    void Update()
    {
        if (tt.finish)
        {
            //terrain 완성한 후에 state 시작
            run(state);
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
        Debug.Log("Next State : "+state);
    }

    bool isNewStart = true;

    public override void startState()
    {
        Debug.Log("Start State");
        if (!abm.finish)
        {
            if (isNewStart)
            {
                ball.transform.position = startPos.transform.position;
                isNewStart = false;
            }
            cameraPos.transform.position = startCameraPos.transform.position;
            Debug.Log("Chaged Pos : " + ball.transform.position.x + ", " + ball.transform.position.z);
            if (abm.isColliedGGreen)
            {
                isNewStart = true;
                nextState();
            }
        }
    }
    public override void readyState() {
        Debug.Log("Ready State");
        abm.setVelocity(userVeloc);
        nextState();
    }
    public override void rollState()
    {
        Debug.Log("Roll State");
        if (abm.finish)
        {
            Debug.Log("Stopped");
            nextState();
        }
    }
    public override void endState()
    {
        Debug.Log("End State");
        if (abm.finish)
        {
            abm.finish = false; // 초기화
            stoppedPos = ball.transform.position;
            Debug.Log("Start Examine : " + stoppedPos.x + ", " + stoppedPos.z);

            angle = Mathf.Atan2((stoppedPos.z - startPos.transform.position.z), (stoppedPos.x - startPos.transform.position.x)) * Mathf.Rad2Deg
                - Mathf.Atan2((hole.transform.position.z - startPos.transform.position.z), (hole.transform.position.x - startPos.transform.position.x)) * Mathf.Rad2Deg;
            Debug.Log("ANGLE " +angle); // 각도 계산이 잘 안됨
            if (!abm.succeed)
            {
                abm.ChangeAngle(angle);
                nextState();
            }
            else
            {
                Debug.Log("들어감");
            }
        }
    }
    
}
