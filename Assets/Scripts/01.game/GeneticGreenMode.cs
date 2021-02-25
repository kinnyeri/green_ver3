using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Windows;
using System;

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
    Vector3 findAnsVeloc; 
    public Vector3 stoppedPos;
    private int count = 0;
    private int AnsCount = 0;
    private int standScore=10;

    float angle;

    List<float> rightAngleList;

    public List<float> probList; //확률 저장

    List<float> angleList;
    List<float> velocList;
    List<float> distanceList;

    int countRound = 0;

    bool foundAns, isFirstStartForAns;
    public makeLevel ml;

    public bool finish = false;



    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();

        state = 0;

        //정답 찾기
        foundAns = false;
        isFirstStartForAns = true;

        hole.transform.position = new Vector3(random.Next(100, 200), 3.1f, random.Next(100, 200));
        startPos.transform.position = new Vector3(random.Next(100, 200), 8f, random.Next(100, 200));
        findAnsVeloc = new Vector3(0, 0, 20);

        rightAngleList = new List<float>();
        angleList = new List<float>();
        distanceList = new List<float>();
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
            Debug.Log("update State : " + state);

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

    bool isNewStart = true;

    public override void startState()
    {
        Debug.Log("Start State");
        if (!foundAns)
        {
            Debug.Log("정답 각도 찾기 "+countRound);
        }
        if (isNewStart)
        {
            //새 스타트 위치
            ball.transform.position = startPos.transform.position;
            //각도 랜덤 생성
            isNewStart = false;
        }

        cameraPos.transform.position = startCameraPos.transform.position;
        if (abm.isColliedGGreen)
        {
            isNewStart = true;
            Debug.Log("first veloc " + findAnsVeloc.z);
            nextState();
        }

    }
    public override void readyState() {
        Debug.Log("Ready State");
        if (!foundAns)
        {
            Debug.Log("못찾음");

            abm.setVelocity(findAnsVeloc);
            nextState();
        }
        else
        {
            abm.setVelocity(userVeloc);
            nextState();
        }
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
        Debug.Log("End State ");
        stoppedPos = ball.transform.position;
        // 각도 계산 
        angle = Mathf.Atan2((stoppedPos.z - startPos.transform.position.z), (stoppedPos.x - startPos.transform.position.x)) * Mathf.Rad2Deg
            - Mathf.Atan2((hole.transform.position.z - startPos.transform.position.z), (hole.transform.position.x - startPos.transform.position.x)) * Mathf.Rad2Deg;
        
      if (!foundAns)
        {
            if (abm.succeed)
            {
                //홀에 들어갔을 경우
                Debug.Log("들어감");
                foundAns = true;
                rightAngleList.Add(abm.transform.rotation.z); // 정답 각도 저장 //1회 -0까지만 있음
            }
            else
            {
                Debug.Log("못 들어감");
                AnsCount++;
                abm.ChangeAngle(angle);
                if(AnsCount == 5)
                {
                    startPos.transform.position = new Vector3(random.Next(100, 200), 8f, random.Next(100, 200));
                    AnsCount = 0;
                    angle = 0;
                }
            
            }
        }
        else
        {
            Debug.Log("count" + count);
            Debug.Log("Start Examine : " + stoppedPos.x + ", " + stoppedPos.z);

            //거리 비율 저장
            distanceList.Add(Mathf.Sqrt(Mathf.Pow(hole.transform.position.x - stoppedPos.x, 2) + Mathf.Pow(hole.transform.position.z - stoppedPos.z, 2)));

            Debug.Log("ANGLE " + angle);
            Debug.Log("DISTAN " + distanceList[count]);
            float temp = UnityEngine.Random.Range(-5f, 5f);
            userVeloc = new Vector3(0, 0, findAnsVeloc.z + temp);
            temp = UnityEngine.Random.Range(-25f, 25f);
            angle = rightAngleList[countRound] + temp;

            velocList.Add(userVeloc.z);
            angleList.Add(angle);

            Debug.Log("userVeloc  " +count+" "+ userVeloc.z);
            Debug.Log("try angle " +count+" "+ angle);

            count++;
            if (count == 10)
            {
                Debug.Log("countRound" + countRound);
                float avgDistance = 0;
                float DeviationOfDistance = 0;
                float sumOfDistance = 0;

                avgDistance = distanceList.Average();

                for (int i = 0; i < count; i++)
                {
                   sumOfDistance += Mathf.Pow(distanceList[i] - avgDistance, 2);
                }
                DeviationOfDistance = Mathf.Sqrt(sumOfDistance / (count));

                Debug.Log("거리 평균: " + avgDistance + "표준편차:" + DeviationOfDistance);

                probList.Add(Mathf.Abs((standScore - avgDistance) / DeviationOfDistance));
                Debug.Log("정규화 " + probList[countRound]+"성공확률" + Phi(countRound)); //probList[countRound], 10

                //초기화
                //rightAngleList.Clear();
                distanceList.Clear();
                angleList.Clear();
                startPos.transform.position = new Vector3(random.Next(100, 200), 8f, random.Next(100, 200));
                foundAns = false;

                countRound++;
                if (countRound == ml.populationSize)
                {
                    countRound = 0;
                }
                
                count = 0;
                abm.succeed = false;
                this.enabled = false;
                tt.finish = false;
                finish = true;
                
                //정답 부터 다시 찾기
            }
        }
        abm.turnOffKinematic();
        abm.isColliedGGreen = false;
        abm.finish = false; // 초기화
        nextState();
    }
    double Phi(int index)
    {
        float x = probList[index];
        int n = 10;

        float sum = 0;
        float x2 = x * x;
        float nom = x;
        float denom = 1;
        float c = 1;
        for (int i = 0; i < n; i++)
        {
            sum += nom / denom;
            c += 2;
            nom *= x2;
            denom *= c;
        }
        return 0.5 + sum * Mathf.Exp(-x2 * 0.5f) / Mathf.Sqrt(2 * Mathf.PI);
    }
}
