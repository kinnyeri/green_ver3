using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PracticeMode : Progress
{
    public GameObject selectButtons;

    Slider[] selectSliders = new Slider[3];
    public GameObject startButtons;
    public GameObject readyButtons;
    public GetPower gp;
    
    public BollMove bm;

    public GameObject endButtons;
    public GameObject againUI;

    public int hitCount = 0;
    int practiceCount;

    public Text hitCntObj;
    public Text practiceCntObj;

    public GameObject ball;
    public GameObject startPos;
    public GameObject cameraPos;
    public GameObject startCameraPos;


    // Start is called before the first frame update
    void Start()
    {
        state = 0;
        practiceCount = 0;
        for(int i = 0; i < selectSliders.Length; i++)
        {
            selectSliders[i] = selectButtons.transform.GetChild(i).gameObject.GetComponent<Slider>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        hitCntObj.text = "" + hitCount;
        practiceCntObj.text = "" + (practiceCount + 1);

        run(state);
    }

    public override void startState()
    {
        
        if (!startButtons.activeSelf)
        {
            startButtons.SetActive(true);
        }
        if (againUI.activeSelf)
        {
            StartCoroutine(WaitForNotice());
        }
        if (endButtons.activeSelf)
        {
            endButtons.SetActive(false);
        }
        ball.transform.GetComponent<Rigidbody>().isKinematic = true;
    }
    public void gotoReady()
    {
        if (selectButtons.activeSelf)
        {
            selectButtons.SetActive(false);
        }
        nextState();
    }
   
    IEnumerator WaitForNotice()
    {
        yield return new WaitForSeconds(1.0f);
        againUI.SetActive(false);
    }
    public override void readyState()
    {
        if (!readyButtons.activeSelf)
        {
            Debug.Log("Hi Start");
            readyButtons.SetActive(true);
        }
        if (startButtons.activeSelf)
        {
            startButtons.SetActive(false);
        }
        
        if (gp.getInput == 2)
        {
            Debug.Log(gp.power);
            
            nextState();
        }
    }
    public override void rollState()
    {
        //Debug.Log(gp.power);
        if (gp.getInput == 3)
        {
            gp.getInput = 0;
            
            if (readyButtons.activeSelf)
            {
                readyButtons.SetActive(false);
            }
            //if (bm.succeed)
            //{
            //    Debug.Log("Hi bm.succeed");
            //    nextState();
            //}
            nextState();
            Debug.Log("BYE ROLL");
        }
    }
    public override void endState()
    {
        Debug.Log("END STATE");
        if (bm.succeed)
        {
            
            Debug.Log("GOAL");
            ball.transform.GetComponent<Rigidbody>().isKinematic = false;

            endButtons.SetActive(true);
        }
        else
        {
            if (!endButtons.activeSelf)
            {
                againUI.SetActive(true);
            }
            nextState();
        }
    }

    public void oneMore()
    {
        practiceCount++;
        hitCount = 0;
        nextState();
        if (!selectButtons.activeSelf)
        {
            selectButtons.SetActive(true);
            selectSliders[0].value = 0f;
            selectSliders[1].value = 0.5f;
            selectSliders[2].value = 0.5f;
        }
        ball.transform.position = startPos.transform.position;
        ball.transform.rotation = startPos.transform.rotation;
        //cameraPos.transform.position = startCameraPos.transform.position;
        bm.succeed = false;
    }
}
