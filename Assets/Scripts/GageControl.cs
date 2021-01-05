using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GageControl : MonoBehaviour
{
    public GetPower gp;
    public GameObject gage;
    RectTransform rectTran;
    
    // Start is called before the first frame update
    void Start()
    {
        rectTran = gage.GetComponent<RectTransform>();
        rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
        Debug.Log("SET");
    }

    // Update is called once per frame
    void Update()
    {
        rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)gp.power*3f);
    }
}
