using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PlanetDetection : MonoBehaviour
{
    public UiConfig ui;


    // Update is called once per frame
    void Update()
    {
        DetectPlanet();
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
                Debug.Log(hitData.collider.name);
                ui.SetCurrentPlanetText(hitData.collider.name);
                ui.SetCurrentPlanetSphere(hitData.transform);
                ui.EnableFocusView();
            }
        } else
        {
            ui.SetCurrentPlanetText("");
            ui.SetCurrentPlanetSphere(null);
            ui.DisableFocusView();
        }
        
    }
}
