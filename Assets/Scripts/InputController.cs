using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private CubeController cubeControllerScript;

    void Update()
    {
        if(Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0); //Get first touch, doesn't matter how many there are
            if(t.phase == TouchPhase.Moved)
            {
                if (t.deltaTime <= 1f)
                    cubeControllerScript.SetMovementAxis(t.deltaPosition.x * (2 - t.deltaTime));  //Make dependencies between distance and time of touch
            }    
        }
    }
}
