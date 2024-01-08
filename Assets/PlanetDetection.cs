using HeneGames.Airplane;

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class PlanetDetection : MonoBehaviour
{
    public UiConfig ui;

    [SerializeField]
    private Image BlackScreen;

    private Collider selectedPlanet;
    private Collider cutScenePlanet;

    [SerializeField]
    float cutSceneBaseMoveSpeed = 300;

    [SerializeField]
    float cutSceneBaseAcceleration = 2;

    [SerializeField]
    private GameObject playerShuttle;
    
    private bool cutSceneInProgress = false;
    private Color blackScreenColor;
    private Color transperantBlackImage;
    private float blackScreenTime = 0.3f;

    // Update is called once per frame
    void Start()
    {
        blackScreenColor = BlackScreen.color;
        blackScreenColor.a = 1;
        StartCoroutine(FadeFromBlack());
    }
    void Update()
    {
        DetectPlanet();
        TravelToSelectedPlanet();
        if (cutSceneInProgress == true)
        {

            playerShuttle.transform.LookAt(cutScenePlanet.transform.position);

            Vector3 direction = cutScenePlanet.transform.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 0.05f);

            var step = cutSceneBaseMoveSpeed + cutSceneBaseAcceleration * Time.deltaTime; // calculate distance to move
            playerShuttle.transform.position = Vector3.MoveTowards(playerShuttle.transform.position, cutScenePlanet.transform.position, step);
            //blackscreen step
        }
    }
    public void DetectPlanet()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitData;
        Physics.Raycast(ray, out hitData);
        if (hitData.collider != null)
        {
            if (hitData.collider.tag == "Planet")
            {
                selectedPlanet = hitData.collider;
                ui.SetCurrentPlanetText(hitData.collider.name);
                ui.SetCurrentPlanetSphere(hitData.transform);
                ui.EnableFocusView();
            }
        } else
        {
            selectedPlanet = null;
            ui.SetCurrentPlanetText("");
            ui.SetCurrentPlanetSphere(null);
            ui.DisableFocusView();
        }
        
    }
    public void TravelToSelectedPlanet()
    {
        if (selectedPlanet != null)
        {
            //Travel to planet on button down

            if (Input.GetKeyDown("space"))
            {
                Debug.Log("Travel to " + selectedPlanet.name);
                cutScenePlanet = selectedPlanet;
                GetComponentInParent<SimpleAirPlaneController>().enabled = false;
                FindAnyObjectByType<SolarSystem>().enabled = false;
                //FindAnyObjectByType<Canvas>().enabled = false;
                cutSceneInProgress = true;
                StartCoroutine(CutScene());


            }

        }
    }


    IEnumerator CutScene()
    {
        yield return FadeToBlack();
        Debug.Log(cutScenePlanet.name);
        SceneManager.LoadScene(cutScenePlanet.name);
    }
    IEnumerator FadeFromBlack()
    {
        while (blackScreenColor.a >0 )
        {

            blackScreenColor.a -= blackScreenTime * Time.deltaTime;
            BlackScreen.color = blackScreenColor;

            yield return null;
        }

    }
    IEnumerator FadeToBlack()
    {
        while (blackScreenColor.a < 1)
        {

            blackScreenColor.a += blackScreenTime * Time.deltaTime;
            BlackScreen.color = blackScreenColor;

            yield return null;
        }

    }

}
