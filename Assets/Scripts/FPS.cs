using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour
{
    bool boolFPSIncrease;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (boolFPSIncrease)
            {
                Application.targetFrameRate = 1;
            }
            else
            {
                Application.targetFrameRate = 120;
            }
            boolFPSIncrease = !boolFPSIncrease;
        }
    }
}
