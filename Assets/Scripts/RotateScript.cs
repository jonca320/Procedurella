using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class RotateScript : MonoBehaviour
    
{
    [Header("Rotation Amount per second")]
    public float rotateXAmount;
    public float rotateYAmount;
    public float rotateZAmount; 


    private void Update()
    {
        RotationMovement();
    }
    public void RotationMovement()
    {

        transform.Rotate(rotateXAmount*Time.deltaTime, rotateXAmount * Time.deltaTime, rotateXAmount * Time.deltaTime);

    }
}
