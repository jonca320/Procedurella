using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;
    void Update()
    {
        //Lock to player location
        transform.position = playerTransform.position;    
    }
}
