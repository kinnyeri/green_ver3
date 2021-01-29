using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public float standScore;
    List<float> rightAngleList;
    List<float> scoreList;  //1- disance
    List<float> probList;

    List<float> distanceRateList;
    List<float> velocList;
    int countRound = 0;
    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();

        state = 0;

        hole.transform.position = new Vector3(random.Next(100, 200), 3.1f, random.Next(100, 200));
        startPos.transform.position = new Vector3(random.Next(100, 200), 8f, random.Next(100, 200));

        rightAngleList = new List<float>();
        distanceRateList  = new List<float>();
        scoreList = new List<float>();
        velocList = new List<float>();
        probList = new List<float>();
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
        userVeloc = new Vector3(0, 0, Random.RandomRange(10f, 50f));
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
        Debug.Log("End State ");
        if (abm.finish)
        {
            if (abm.succeed)
            {
                //홀에 들어갔을 경우
                Debug.Log("들어감");
                abm.succeed = false;
            }
            stoppedPos = ball.transform.position;
            
            
            Debug.Log("count" + count);
            Debug.Log("Start Examine : " + stoppedPos.x + ", " + stoppedPos.z);
            // 각도 계산 
            angle = Mathf.Atan2((stoppedPos.z - startPos.transform.position.z), (stoppedPos.x - startPos.transform.position.x)) * Mathf.Rad2Deg
                - Mathf.Atan2((hole.transform.position.z - startPos.transform.position.z), (hole.transform.position.x - startPos.transform.position.x)) * Mathf.Rad2Deg;
            rightAngleList.Add(angle); // 각도 저장
            //거리 비율 저장
            distanceRateList.Add(Mathf.Sqrt(Mathf.Pow(hole.transform.position.x, 2) + Mathf.Pow(hole.transform.position.z, 2)) / Mathf.Sqrt(Mathf.Pow(stoppedPos.x,2)+ Mathf.Pow(stoppedPos.z, 2)));
            scoreList.Add(Mathf.Abs(1 - distanceRateList[count])); // |1-distanceRate|
            //속도 저장
            velocList.Add(userVeloc.z);
            
            Debug.Log("ANGLE " +angle);
            Debug.Log("distanceRateList " + count + "번째"+ distanceRateList[count]);
            Debug.Log("score " + scoreList[count]);
            abm.finish = false; // 초기화
            count++;
            if (count == 10)
            {
                Debug.Log("countRound" + countRound);
                float avgDistance = 0, avgAngle = 0, avgScore=0;
                float DeviationOfDistance = 0, DeviationOfAngle = 0, DeviationOfScore = 0 ;
                float sumOfDistance = 0, sumOfAngle = 0, sumOfScore = 0 ;

                avgDistance = distanceRateList.Average();
                avgAngle = rightAngleList.Average();
                avgScore = scoreList.Average();

                for (int i = 0; i < count; i++)
                {
                    sumOfDistance += Mathf.Pow(distanceRateList[i] - avgDistance, 2);
                    sumOfAngle += Mathf.Pow(rightAngleList[i] - avgAngle, 2);
                    sumOfScore += Mathf.Pow(scoreList[i] - avgScore, 2);
                }
                DeviationOfDistance = Mathf.Sqrt(sumOfDistance / (count));
                DeviationOfAngle = Mathf.Sqrt(sumOfAngle / (count));
                DeviationOfScore = Mathf.Sqrt(sumOfScore / (count));

                //Debug.Log("거리 평균: " + avgDistance + "표준편차:" + DeviationOfDistance);
                //Debug.Log("각도 평균: " + avgAngle + "표준편차:" + DeviationOfAngle);
                Debug.Log("점수 평균: " + avgScore + "표준편차:" + DeviationOfScore);

                probList.Add(Mathf.Abs((standScore - avgScore) / DeviationOfScore));
                Debug.Log("정규화 " + probList[countRound]);
                //초기화
                rightAngleList.Clear();
                scoreList.Clear();
                distanceRateList.Clear();
                startPos.transform.position = new Vector3(random.Next(100, 200), 8f, random.Next(100, 200));
                countRound++;
                count = 0;
            }

            nextState();
            //if (count >= 1 || abm.succeed) //count>=10이었다
            //{
            //    count = 0;
            //    //if(count==10) Debug.Log("10번 넘어감");
            //    if (abm.succeed)
            //    {
            //        //홀에 들어갔을 경우
            //        Debug.Log("들어감");
            //        abm.succeed = false;
            //    }
            //    nextState();
            //}
            //else if (!abm.succeed)
            //{
            //    abm.ChangeAngle(angle);
            //    nextState();
            //}
        }
        
        
    }
    
}
