using UnityEngine;

public class FingerTipController
{
    private static FingerTipController instance = null;
    public static FingerTipController SharedInstance
    {
        get
        {
            if (instance == null)
            {
                instance = new FingerTipController();
            }
            return instance;
        }
    }

    public Vector3 pointOfInterest;
    public Vector3 rayPoint;
}
