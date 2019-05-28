using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRay : MonoBehaviour
{
    public Camera cam;
    private Ray ray;


    // Update is called once per frame
    void Update()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);

        MouseInController.SharedInstance.pointOfInterest = ray.GetPoint(5.0f);
    }

}
