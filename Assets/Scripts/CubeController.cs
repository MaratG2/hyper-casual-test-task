using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    [Tooltip("Speed of cube rotation")] [SerializeField] [Range(0.05f, 1f)] private float rotationSpeed = 0.25f;
    [Tooltip("Max rotation speed")] [SerializeField] [Range(1000, 3000)] private int maxRotation = 1500;

    [HideInInspector] public float movementAxis { get; set; } //To use it as total swipe move per tick
    [HideInInspector] public float deltaRotationY = 0f; //Used for counting rotations (/360)
    [HideInInspector] public bool win;
    [HideInInspector] public float rotationY = 0f; //Charging rotation for smoothness
    
    private float oldRotationY = 0f;
   
    void Start()
    {
        StartCoroutine(RotationSmooth());
    }
    void Update()
    {
        oldRotationY = UnityEngine.Mathf.Abs(transform.localEulerAngles.y);
        transform.Rotate(new Vector3(0f, rotationY * rotationSpeed * Time.deltaTime * -1, 0f)); //invert for better feeling of player (swipe dir is the same with cube dir)
        deltaRotationY = UnityEngine.Mathf.Abs(UnityEngine.Mathf.Abs(transform.localEulerAngles.y) - oldRotationY);

        if (deltaRotationY > 300) //Exception 
            deltaRotationY = 2f; //2 is approximate medium number per frame to add
    }

    public void SetMovementAxis(float axisValue)
    {
        if (!win)
        {
            movementAxis = axisValue;
            rotationY = rotationY + movementAxis; //Charge 
        }

        if (rotationY > maxRotation)
            rotationY = maxRotation;
        else if (rotationY < -maxRotation)
            rotationY = -maxRotation;
    }

    private IEnumerator RotationSmooth() //Smooth charge loop
    {
        if (rotationY > 30f || rotationY < -30f) //Borders, out of which rotation needs to be stopped
            rotationY *= 0.95f;
        else
            rotationY = 0f; 
        
        yield return new WaitForSecondsRealtime(0.032f); //2 frames, just for full control of process and ease of change just in case
        StartCoroutine(RotationSmooth());
    }
}
