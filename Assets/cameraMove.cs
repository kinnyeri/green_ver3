using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove: MonoBehaviour
{
    public Transform target;

    float offsetX = 0f, offsetY = 2.75f, offsetZ = -10;


    Vector3 cameraPos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {

            cameraPos.x = target.position.x + offsetX;
            cameraPos.y = target.position.y + offsetY;
            cameraPos.z = target.position.z + offsetZ;
            transform.position = cameraPos;
    }
}
