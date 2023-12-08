using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.EventSystems;


public class MarineMovement : MonoBehaviour
{
    Rigidbody rb;
    Camera mainCamera;
    public float moveSpeed = 200f;
    public float rotateSpeed = 10f;
    public float heightIncreaseAmount = 0.5f;

    private float verticalInput;
    private float horizontalInput;
    private float altitudeInput;


    private float heightMovement;

    private Vector3 moveDirection;
    public MapGenerator mapGenerator;
    float inputAmount;
    private Vector3 cruiserVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;

    }

    void Update()
    {
        
        cruiserVelocity = (moveDirection * moveSpeed * inputAmount);
        Controls();
        Debug.Log(cruiserVelocity);
        //mapGenerator.GenerateMap();
        rb.velocity = cruiserVelocity*Time.deltaTime;
        //rb.AddForce(cruiserVelocity);
        //mapGenerator.offset.x += cruiserVelocity.x*Time.deltaTime;
        //mapGenerator.offset.y -= cruiserVelocity.z*Time.deltaTime;
    }

    void Controls()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");



        Vector3 correctedVertical = verticalInput * mainCamera.transform.forward;
        Vector3 correctedHorizontal = horizontalInput * mainCamera.transform.right;
        Vector3 combinedInput = correctedHorizontal + correctedVertical;

        moveDirection = new Vector3((combinedInput).normalized.x, 0, (combinedInput).normalized.z);
  
        //float inputMagnitude = Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput);
        float inputMagnitude = Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput);

        inputAmount = Mathf.Clamp01(inputMagnitude);


    }
}
