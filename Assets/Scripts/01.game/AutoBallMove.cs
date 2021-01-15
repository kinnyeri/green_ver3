using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBallMove : MonoBehaviour
{
    public Terrain terrain;
    TerrainData td;
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

    Vector3[] normVecs;

    Vector3 gravity = new Vector3(0, 9.8f, 0);
    float greenFriction = 0f;
    public float T = 0.25f;
    Vector3 FrictionA = new Vector3(0, 0, 0);
    Vector3 side1, side2, perp;
    int count = 0;

    public bool isColliedGGreen = false;

    //public Camera rayCamera;

    public bool finish = false;
    //public GameObject temp;
    public void setVelocity(Vector3 v)
    {
        velocity = v;
        velocity = velocity - perp * (Vector3.Dot(perp, velocity));
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.gameObject.GetComponent<Rigidbody>();
        td = terrain.terrainData;
        normVecs = new Vector3[3];
    }

    void FixedUpdate()
    {
        count++;
        if (ggm.state == Progress.StateLevel.Start)
        {
            //norm
            tarPos = targetPlace.transform.position;
            tan = (tarPos.y - transform.position.y) / (tarPos.z - transform.position.z);
            Debug.Log("td " + td.GetHeight((int)transform.position.x, (int)transform.position.z));
            Debug.Log("t " + transform.position.x);
            normVecs[0] = new Vector3(transform.position.x, td.GetHeight((int)transform.position.x, (int)transform.position.z), transform.position.z);
            normVecs[1] = new Vector3(transform.position.x+1, td.GetHeight((int)transform.position.x+1, (int)transform.position.z), transform.position.z);
            normVecs[2] = new Vector3(transform.position.x, td.GetHeight((int)transform.position.x, (int)transform.position.z+1), transform.position.z+1);

            side1 = normVecs[1] - normVecs[0];
            side2 = normVecs[2] - normVecs[0];
            perp = Vector3.Cross(side1, side2);
            Debug.Log("노말 전 " + perp);
            perp = perp.normalized;

            Debug.Log("Start Pos: " + transform.position.x+", "+transform.position.z);
            Debug.Log("Tar Pos: " + tarPos.x+", "+ tarPos.z);
        }
        if (ggm.state == Progress.StateLevel.Roll)
        {
            if (Mathf.Abs(tarPos.x - transform.position.x) < 1.3f && Mathf.Abs(tarPos.z - transform.position.z) < 1.3f)
            {
                Debug.Log("Finish Line");
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


            //norm
            tarPos = targetPlace.transform.position;
            tan = (tarPos.y - transform.position.y) / (tarPos.z - transform.position.z);
            Debug.Log("td "+td.GetHeight((int)transform.position.x, (int)transform.position.z));
            normVecs[0] = new Vector3(transform.position.x, td.GetHeight((int)transform.position.x, (int)transform.position.z), transform.position.z);
            normVecs[1] = new Vector3(transform.position.x + 1, td.GetHeight((int)transform.position.x + 1, (int)transform.position.z), transform.position.z);
            normVecs[2] = new Vector3(transform.position.x, td.GetHeight((int)transform.position.x, (int)transform.position.z + 1), transform.position.z + 1);

            Debug.Log("NORM 0 " + normVecs[0]);
            Debug.Log("NORM 1 " + normVecs[1]);
            Debug.Log("NORM 2 " + normVecs[2]);


            side1 = normVecs[1] - normVecs[0];
            side2 = normVecs[2] - normVecs[0];
            Debug.Log("PERP " + perp);
            perp = Vector3.Cross(side1, side2);
            Debug.Log("노말 전 " + perp);

            perp = perp.normalized;


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
            Debug.Log("Velocity " + velocity);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name== "GeneratedGreen")
        {
            Debug.Log("Green collied with a ball");
            GetComponent<Rigidbody>().isKinematic = true;
            isColliedGGreen = true;
        }
    }
}
