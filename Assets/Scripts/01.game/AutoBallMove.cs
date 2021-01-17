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
    public float T = 0.5f; //0.25f;
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
            Debug.Log("td " + getHeight(transform.position.x, transform.position.z));
            Debug.Log("t " + transform.position.x);

            Debug.Log("KK 200,200 ::: "+td.GetHeight(150,150));

            //normVecs[0] = new Vector3(transform.position.x, getHeight(transform.position.x, transform.position.z), transform.position.z);
            //normVecs[1] = new Vector3(transform.position.x+ 1, getHeight(transform.position.x+ 1, transform.position.z), transform.position.z);
            //normVecs[2] = new Vector3(transform.position.x, getHeight(transform.position.x, transform.position.z+ 1), transform.position.z+ 1);

            //side1 = normVecs[1] - normVecs[0];
            //side2 = normVecs[2] - normVecs[0];
            //Debug.Log("side1 " + side1);
            //Debug.Log("side2 " + side2);
            //perp = Vector3.Cross(side2, side1);
            //Debug.Log("노말 전 " + perp);
            //perp = perp.normalized;

            //Debug.Log("Start Pos: " + transform.position.x+", "+transform.position.z);
            //Debug.Log("Tar Pos: " + tarPos.x+", "+ tarPos.z);
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
            Debug.Log("td "+getHeight(transform.position.x, transform.position.z));
            Debug.Log("td + 20 " + getHeight(transform.position.x+1, transform.position.z));
            normVecs[0] = new Vector3(transform.position.x, getHeight(transform.position.x, transform.position.z), transform.position.z);
            normVecs[1] = new Vector3(transform.position.x + 1, getHeight(transform.position.x + 1, transform.position.z), transform.position.z);
            normVecs[2] = new Vector3(transform.position.x, getHeight(transform.position.x, transform.position.z + 1), transform.position.z + 1);

            Debug.Log("NORM 0 " + normVecs[0]);
            Debug.Log("NORM 1 " + normVecs[1]);
            Debug.Log("NORM 2 " + normVecs[2]);

            side1 = normVecs[1] - normVecs[0];
            side2 = normVecs[2] - normVecs[0];

            perp = Vector3.Cross(side2, side1);
            //Debug.Log("perp y " + perp.y.ToString("N3"));
            Debug.Log("노말 전 " + perp);
            perp = perp.normalized;
            Debug.Log("PERP " + perp);

            // 중력
            float height = getHeight(transform.position.x, transform.position.z);
            Vector3 gravityA;
            if (transform.position.y <=  height + 1f )
            {
                gravityA = new Vector3(gravity.y * perp.x, gravity.y * (perp.y - 1.5f), gravity.y * perp.z);
            }
            else
            {
                gravityA = new Vector3(0, -gravity.y , 0);
            }

            //마찰력
            greenFriction = Mathf.Lerp(0.5f, 0.3f, velocity.magnitude / T); // 운동, 정지 마찰력 //  0.3f * (Vector3.Magnitude(velocity) / T) + 0.4f * ((T - Vector3.Magnitude(velocity) / T));
            FrictionA = -perp.y * gravity.y * velocity.normalized * greenFriction;

            velocity += (gravityA + FrictionA) * Time.deltaTime*5f;
            //transform.position += velocity * Time.deltaTime;
            transform.Translate(velocity * Time.deltaTime); // local 좌표로 이동
            height = getHeight(transform.position.x, transform.position.z);
            if(height + 1f > transform.position.y)
                transform.Translate(0, height +1f-transform.position.y, 0);

            //gp.power = (Mathf.Round(velocity.z * 1000) * 0.001f);
            Debug.Log("Move " + cnt);
            Debug.Log("Velocity " + velocity);
        }
    }

    float getHeight(float x, float z)
    {
        float ld, rd, lu, ru;
        ld = td.GetHeight(Mathf.FloorToInt(x), Mathf.FloorToInt(z));
        rd = td.GetHeight(Mathf.CeilToInt(x), Mathf.FloorToInt(z));
        lu = td.GetHeight(Mathf.FloorToInt(x), Mathf.CeilToInt(z));
        ru = td.GetHeight(Mathf.CeilToInt(x), Mathf.CeilToInt(z));
        float l, r;
        l = Mathf.Lerp(ld, lu, z - Mathf.Floor(z));
        r = Mathf.Lerp(rd, ru, z - Mathf.Floor(z));
        return Mathf.Lerp(l, r, x - Mathf.Floor(x));
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name== "GeneratedGreen")
        {
            Debug.Log("Green collied with a ball");
            //GetComponent<Rigidbody>().isKinematic = true;
            isColliedGGreen = true;
        }
    }
}
