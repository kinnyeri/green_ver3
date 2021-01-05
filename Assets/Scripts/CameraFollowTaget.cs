using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTaget : MonoBehaviour
{
    public Transform target;
    public PracticeMode pm;
    public GetPower gp;
    float offsetX = 0f, offsetY = 2.75f, offsetZ = -10;

    public Transform defaultPos;

    Vector3 cameraPos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        //if (pm.state == Progress.StateLevel.Roll)
        //{
        //    cameraPos.x = target.position.x + offsetX;
        //    cameraPos.y = target.position.y + offsetY;
        //    cameraPos.z = target.position.z + offsetZ;
        //    transform.position = cameraPos;
        //}
        if(pm.state == Progress.StateLevel.Ready)
        {
            transform.position = defaultPos.position;
            transform.rotation = defaultPos.rotation;
        }
    }
}
