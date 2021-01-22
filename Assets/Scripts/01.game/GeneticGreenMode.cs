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
    private int count = 0;

    float angle;
    List<float> rightAngle;
    List<float> distanceRate;
    int countRound = 0;
    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();

        state = 0;

        hole.transform.position = new Vector3(random.Next(100, 200), 3.1f, random.Next(100, 200));
        startPos.transform.position = new Vector3(random.Next(100, 200), 8f, random.Next(100, 200));

        rightAngle = new List<float>();
        distanceRate  = new List<float>();
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
                //새 스타트 위치
                ball.transform.position = startPos.transform.position;
                //각도 랜덤 생성
                abm.ChangeAngle(Random.Range(-180f,180f));
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
        /*
        if(Vector3.Distance(hole.transform.position, startPos.transform.position) < Vector3.Distance(hole.transform.position, ball.transform.position))
        {
            isNewStart=
        }*/
    }
    public override void endState()
    {
        Debug.Log("End State");
        if (abm.finish)
        {
            
            stoppedPos = ball.transform.position;
            count++;
            
            Debug.Log("count" + count);
            Debug.Log("Start Examine : " + stoppedPos.x + ", " + stoppedPos.z);
            // 각도 계산 
            angle = Mathf.Atan2((stoppedPos.z - startPos.transform.position.z), (stoppedPos.x - startPos.transform.position.x)) * Mathf.Rad2Deg
                - Mathf.Atan2((hole.transform.position.z - startPos.transform.position.z), (hole.transform.position.x - startPos.transform.position.x)) * Mathf.Rad2Deg;
            rightAngle.Add(angle); // 각도 저장
            //거리 비율 저장
            distanceRate.Add(Mathf.Sqrt(Mathf.Pow(hole.transform.position.x, 2) + Mathf.Pow(hole.transform.position.z, 2)) / Mathf.Sqrt(Mathf.Pow(stoppedPos.x,2)+ Mathf.Pow(stoppedPos.z, 2)));
            Debug.Log("ANGLE " +angle);
            Debug.Log("distanceRate " + distanceRate[countRound]);

            countRound++;
            abm.finish = false; // 초기화
            if (count >= 1 || abm.succeed) //count>=10이었다
            {
                startPos.transform.position = new Vector3(random.Next(100, 200), 8f, random.Next(100, 200));
                count = 0;
                //if(count==10) Debug.Log("10번 넘어감");
                if (abm.succeed)
                {
                    //홀에 들어갔을 경우
                    Debug.Log("들어감");
                    abm.succeed = false;
                }
                nextState();
            }
            //else if (!abm.succeed)
            //{
            //    abm.ChangeAngle(angle);
            //    nextState();
            //}
        }
    }
    
}
