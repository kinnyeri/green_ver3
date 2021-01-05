using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBallMove : MonoBehaviour
{
    Rigidbody rb;
    public Vector3 target; //= new Vector3(-0.615f, 2.705f, 29.275f);

    public GameObject targetPlace;
    Vector3 tarPos;
    float tan;

    public GeneticGreenMode ggm;
    public bool succeed = false;
    Vector3 newPos;
    Vector3 velocity = new Vector3(0f, 0f, 0f);

    int cnt = 0;
    float friction = 0.3f;

    public GameObject[] normVecs;

    Vector3 gravity = new Vector3(0, 9.8f, 0);
    float greenFriction = 0f;
    public float T = 0.25f;
    Vector3 FrictionA = new Vector3(0, 0, 0);
    Vector3 side1, side2, perp;
    int count = 0;


    public bool finish = false;

    public void setVelocity(Vector3 v)
    {
        velocity = v;
        velocity = velocity - perp * (Vector3.Dot(perp, velocity));
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        count++;
        if (ggm.state == Progress.StateLevel.Start)
        {
            tarPos = targetPlace.transform.position;
            tan = (tarPos.y - transform.position.y) / (tarPos.z - transform.position.z);

            side1 = normVecs[1].transform.position - normVecs[0].transform.position;
            side2 = normVecs[2].transform.position - normVecs[0].transform.position;
            perp = Vector3.Cross(side1, side2);
            perp = perp.normalized;
            Debug.Log("Start Pos: " + transform.position.x+", "+transform.position.z);
            Debug.Log("Tar Pos: " + tarPos.x+", "+ tarPos.z);
        }
        if (ggm.state == Progress.StateLevel.Roll)
        {
            if (Mathf.Abs(tarPos.x - transform.position.x) < 1.3f && Mathf.Abs(tarPos.z - transform.position.z) < 1.3f)
            {
                succeed = true;
                velocity = new Vector3(0f, 0f, 0f);
                finish = true;//들어갔다

                return;
            }

            if ((Mathf.Round(velocity.z * 1000) * 0.001f) <= 1)
            {
                finish = true; // 끝났다
                Debug.Log("End Pos: " + transform.position.x + ", " + transform.position.z);
                Debug.Log("Veloc : " + velocity.z);
                return;
            }
            cnt++;

            // 중력
            Vector3 gravityA = new Vector3(gravity.y * perp.x, gravity.y * (perp.y - 1), gravity.y * perp.z);

            //마찰력
            greenFriction = 0.3f * (Vector3.Magnitude(velocity) / T) + 0.4f * ((T - Vector3.Magnitude(velocity) / T));
            FrictionA = perp.y * gravity.y * (velocity / Vector3.Magnitude(velocity)) * greenFriction;

            velocity += (gravityA + FrictionA) * Time.deltaTime;
            //transform.position += velocity * Time.deltaTime;
            transform.Translate(velocity * Time.deltaTime); // local 좌표로 이동
            //gp.power = (Mathf.Round(velocity.z * 1000) * 0.001f);
            Debug.Log("Move " + cnt);
        }
    }
}
