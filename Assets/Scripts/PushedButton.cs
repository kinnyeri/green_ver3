using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PushedButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PracticeMode pm;
    bool pressed = false;
    public float length;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pressed && transform.name =="Left")
        {
            length -= 0.5f;
        }
        if (pressed && transform.name == "Right")
        {
            length += 0.5f;
        }
        if (!(pm.state==Progress.StateLevel.Ready))
        {
            //length = 0;
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (pm.state == Progress.StateLevel.Ready)
        {
            pressed = true;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (pm.state == Progress.StateLevel.Ready)
        {
            pressed = false;
        }
    }
}
