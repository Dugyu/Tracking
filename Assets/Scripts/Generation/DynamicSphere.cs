using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSphere : MonoBehaviour
{
    public static int memoCount = 50;
    private List<Memo> memos = new List<Memo>();
    public GameObject memoGraphic;

    public float _range;
    public Transform _center;

    // Start is called before the first frame update
    void Start()
    {
        Memo.range = _range;

        for (int i = 0; i < memoCount; i++)
        {
            Memo memo = new Memo(memoGraphic);
            memos.Add(memo);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Memo.nucleus = _center.position;
        Memo.CalculateSwarm(memos);
        for (int i = 0; i < memoCount; i++)
        {
            memos[i].Move();
        }
    }
}
