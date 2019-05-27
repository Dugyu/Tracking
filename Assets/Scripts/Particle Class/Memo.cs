using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memo
{
    // shared
    static float maxRadius = 0.5f;
    static float averageHeight = 0.5f;
    public static float range = 0.2f;

    // self
    public GameObject obj;
    public Vector3 pos = Vector3.zero;
    public Vector3 vel = Vector3.zero;
    public Vector3 acc = Vector3.zero;
    public Vector3 origin = Vector3.zero;
    public Vector3 interForce = Vector3.zero;

    public int id;

    // limited resources
    static LinkedList<int> unusedIndices = new LinkedList<int>();
    static int lastIndex = 1;

    static int getIndex()
    {
        int idx = 0;
        if (unusedIndices.Count != 0)
        {
            idx = unusedIndices.Last.Value;
            unusedIndices.RemoveLast();
        }
        else
        {
            idx = lastIndex;
            lastIndex++;
        }

        return idx;
    }

    static void releaseIndex(int idx)
    {
        unusedIndices.AddLast(idx);
    }




    // initialize
    public Memo(GameObject _memo)
    {
        id = getIndex();
        obj = Object.Instantiate(_memo);
        Place();
    }

    // first placement
    public void Place()
    {
        float phi = Random.value * Mathf.PI * 2;
        float theta = Random.value * Mathf.PI;
        float r = RandomGaussian(range, range * 0.1f) + maxRadius * (1.0f - Mathf.Pow(Random.value, 7.0f));

        //sphere
        float x = r * Mathf.Cos(phi) * Mathf.Sin(theta);
        float z = r * Mathf.Sin(phi) * Mathf.Sin(theta);
        float y = r * Mathf.Cos(theta);
            
            
        y += averageHeight;
        origin = new Vector3(x, y, z);
        obj.transform.position = origin;
    }

    // reset when not grabbing
    public void Reset()
    {

    }

    

    public void CalculateInterForce()
    {

    }




    public void Move()
    {
        vel *= 0.8f;
        vel += acc + interForce;
        pos += vel * 0.08f;
        obj.transform.position = pos;
    }




    // helper function
    public float RandomGaussian(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value;       //uniform(0,1] random doubles
        float u2 = 1.0f - Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);//random normal(0,1)
        float randNormal = mean + stdDev * randStdNormal;  //random normal(mean,stdDev^2)
        return randNormal;
    }

}
