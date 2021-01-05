using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;

public class BollMove : MonoBehaviour
{
    public GetPower gp;
    Rigidbody rb;
    Vector3 target = new Vector3(-0.615f, 2.705f, 29.275f);
    public GameObject selectButtons;

    public GameObject targetPlace;
    Vector3 tarPos;
    float tan;

    public PracticeMode pm;
    public bool succeed = false;
    Vector3 newPos;
    Vector3 velocity = new Vector3(0f, 0f, 0f);

    int cnt = 0;
    float friction = 0.3f;

    public GameObject[]normVecs;

    Vector3 gravity = new Vector3(0, 9.8f, 0);
    float greenFriction = 0f;
    public float T = 0.25f;
    Vector3 FrictionA=new Vector3(0, 0, 0);
    Vector3 side1, side2, perp;
    
    public void setVelocity(Vector3 v)
    {
        velocity = v;
        velocity = velocity - perp * (Vector3.Dot(perp, velocity));
    }
    int count = 0;
    // Start is called before the first frame update
    void Start()
    { 
        rb = transform.gameObject.GetComponent<Rigidbody>();
        
    }
    private void Update()
    {

    }
    void FixedUpdate()
    {
        count++;
        if(pm.state == Progress.StateLevel.Start && selectButtons.activeSelf)
        {
            tarPos = targetPlace.transform.position;
            tan =(tarPos.y - transform.position.y) / (tarPos.z - transform.position.z);

            side1 = normVecs[1].transform.position - normVecs[0].transform.position;
            side2 = normVecs[2].transform.position - normVecs[0].transform.position;
            perp = Vector3.Cross(side1, side2);
            perp = perp.normalized;
        }
        if (pm.state == Progress.StateLevel.Roll)
        {
            if (Mathf.Abs(tarPos.x - transform.position.x) < 1.3f && Mathf.Abs(tarPos.z - transform.position.z) < 1.3f)
            {
                succeed = true;
                gp.power = 0;
                gp.getInput = 3; //들어갔다

                return;
            }
            
            if ((Mathf.Round(velocity.z*1000)*0.001f) <= 1)
            {
                gp.getInput = 3; // 끝났다

                return;
            }
            cnt++;
            
            // 중력
            Vector3 gravityA = new Vector3(gravity.y * perp.x, gravity.y*(perp.y-1),gravity.y* perp.z);

            //마찰력
            greenFriction = 0.3f * (Vector3.Magnitude(velocity) / T) + 0.4f * ((T - Vector3.Magnitude(velocity) / T));
            FrictionA = perp.y * gravity.y* (velocity / Vector3.Magnitude(velocity))*greenFriction;

            velocity += (gravityA+FrictionA) * Time.deltaTime;
            //transform.position += velocity * Time.deltaTime;
            transform.Translate(velocity * Time.deltaTime); // local 좌표로 이동
            gp.power = (Mathf.Round(velocity.z * 1000) * 0.001f);

            //transform.gameObject.GetComponentInChildren<Transform>().Rotate(Vector3.right * gp.power);
            
            //transform.Rotate(Vector3.right * gp.power);
        }
    }
}
