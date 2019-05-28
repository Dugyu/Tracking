using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memo
{
    // shared
    static float maxRadius = 0.2f;
    static float averageHeight = 0.0f;
    public static float range = 0.2f;

    public static float maximumDist = 0.3f;
    public static float minimumDist = 0.05f;

    public static Vector3 nucleus = new Vector3(0.0f,-0.5f,5.0f);
    public static float centerStrength = 1.0f;
    // self
    public GameObject obj;
    public Vector3 pos = Vector3.zero;
    public Vector3 vel = Vector3.zero;
    public Vector3 acc = Vector3.zero;
    public Vector3 origin = Vector3.zero;
    public float m = 1.0f;

    public int sepaCount = 0;
    public int alignCount = 0;
    public int attractCount = 0;

    public Vector3 sepaSteer = Vector3.zero;
    public Vector3 alignSteer = Vector3.zero;
    public Vector3 attractSteer = Vector3.zero;


    // inter
    public Vector3 interForce = Vector3.zero;
    public Vector3 target = Vector3.zero;
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
        pos = origin + nucleus;

        obj.transform.position = pos;

        m = Random.Range(0.01f, 0.03f);
        obj.transform.localScale = new Vector3(2.0f*m,2.0f*m,2.0f*m);

        target = nucleus;
    }


    

    public static void CalculateInterForce(List<Memo> _memos)
    {

        foreach (Memo memo in _memos)
        {
            memo.interForce = Vector3.zero;
        }

        for (int j = 0; j < _memos.Count; ++j)
        {
            for (int i = j + 1; i < _memos.Count; ++i)
            {
                Vector3 dm = _memos[i].pos - _memos[j].pos;
                float d = dm.magnitude;
                if (d <= 0.0001f) continue;
                Vector3 f = dm * (Mathf.Exp(-0.001f * d * d) / d) * 0.05f;
                _memos[i].interForce -= f;
                _memos[j].interForce += f;
            }
        }

    }

    public static void CalculateSwarm(List<Memo> _memos)
    {
        foreach(Memo memo in _memos)
        {
            memo.alignCount = 0;
            memo.sepaCount = 0;
            memo.attractCount = 0;

            memo.sepaSteer = Vector3.zero;
            memo.alignSteer = Vector3.zero;
            memo.attractSteer = Vector3.zero;

            memo.interForce = Vector3.zero;
        }

        for (int j = 0; j < _memos.Count; ++j)
        {
            for (int i = j + 1; i < _memos.Count; ++i)
            {

                Vector3 dm = _memos[i].pos - _memos[j].pos;
                float d = dm.magnitude;
                dm.Normalize();

                if (d < 0.05f) d = 0.05f;

                // too far away, attract
                if (d > maximumDist)
                {
                    Vector3 f = dm * (d - maximumDist) / d * 0.005f;
                    _memos[i].attractSteer -= f;         
                    _memos[j].attractSteer += f;

                    _memos[i].attractCount += 1;
                    _memos[j].attractCount += 1;
                }

                else if (d < minimumDist)
                {
                    Vector3 f = dm * (minimumDist - d) / d * 0.005f;
                    _memos[i].sepaSteer += f;
                    _memos[j].sepaSteer -= f;

                    _memos[i].sepaCount += 1;
                    _memos[j].sepaCount += 1;
                }

            }
        }

        foreach (Memo memo in _memos)
        {
            if (memo.sepaCount > 0) memo.interForce += memo.sepaSteer / memo.sepaCount;
            if (memo.attractCount > 0) memo.interForce += memo.attractSteer / memo.attractCount;
            memo.TrackNucleus();
        }


    }


    public void TrackNucleus()
    {
        target = target * (1.0f - 0.3f) + nucleus * 0.3f;

        Vector3 dm = target - pos;
        float d = dm.magnitude;
        if (d > 0.1f)
        {
            Vector3 f = dm * (Mathf.Exp(-0.001f * d * d) / d) * 50.0f;
            interForce += f;
        }


    }


    public void Update()
    {
        Vector3 dist = pos - nucleus;
        if (dist.magnitude > 1.0f)
        {
            Place();
        }
        else
        {
            Move();
        }
    }

    public void Move()
    {
        vel *= 0.8f;
        acc += interForce * m;
        vel += acc;
        if (vel.magnitude > 1.0f)
        {
            vel.Normalize();
        }

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
