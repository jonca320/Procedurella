using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UiConfig : MonoBehaviour
{
    public TMP_Text currentPlanetText;
    Camera mainCamera;
    private Vector3 viewPortPos;
    private bool focusView = false;
    private Vector3 textPos;
    private Transform currentPlanetSphere;
    public GameObject crossHair;
    public GameObject focusedCrossHair;

    private void Start()
    {
        mainCamera = Camera.main;
        textPos = currentPlanetText.transform.position;
    }
    public void SetCurrentPlanetText(string planetText)
    {
        currentPlanetText.text = planetText;
    }

    public void SetCurrentPlanetSphere(Transform planetTransform)
    {
        currentPlanetSphere = planetTransform;
    }
    public void EnableFocusView()
    {
        if (focusView == false)
        {
            focusedCrossHair.SetActive(true);
            crossHair.SetActive(false);

            focusView = true;
        }

        viewPortPos = mainCamera.WorldToScreenPoint(currentPlanetSphere.position);
        focusedCrossHair.transform.position = new Vector3(viewPortPos.x, viewPortPos.y);
        focusedCrossHair.transform.rotation = Quaternion.identity; // or adjust as needed

    }
    public void DisableFocusView() {

        if (focusView == true)
        {
            crossHair.SetActive(true);
            focusedCrossHair.SetActive(false);
            focusView = false;
        }
    }
}
