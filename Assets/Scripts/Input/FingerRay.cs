using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class FingerRay : MonoBehaviour
{
    private Ray ray;


    // Update is called once per frame
    void Update()
    {
        var handJointService = MixedRealityToolkit.Instance.GetService<IMixedRealityHandJointService>();
        if (handJointService != null)
        {
            Transform jointTransform = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Right);
            FingerTipController.SharedInstance.pointOfInterest = jointTransform.position;
        }
        Vector3 target = FingerTipController.SharedInstance.pointOfInterest;
        
        ray = new Ray(Vector3.zero, target);
        Vector3 dist = target - Vector3.zero;
        float d = dist.magnitude;
        FingerTipController.SharedInstance.rayPoint = ray.GetPoint(d * d * 8f + 0.8f);

        //var gestureService = MixedRealityToolkit.Instance.get
    }
}
