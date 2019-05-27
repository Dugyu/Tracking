using UnityEngine;

public class MouseInController
{
    private static MouseInController instance = null;
    public static MouseInController SharedInstance
    {
        get {
            if (instance == null)
            {
                instance = new MouseInController();
            }
            return instance;
        }
    }

    public Vector3 pointOfInterest;

}


