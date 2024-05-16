using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTime : GameUtil
{
    
    public Text TestTime;
    public float TimeScale;// �ð� �帧 ����
    // Start is called before the first frame update
    void Start()
    {
        time.Gametime = 6.0f;
    }
    
    // Update is called once per frame
    void Update()
    {
        //time.Gametime += TimeScale * Time.deltaTime;
        TestTime.text = Time.time.ToString("F2");
        if(time.Gametime >= 24.0f)
        {
            time.day++;
            time.Gametime = 0.0f;
        }
    }
    
}
