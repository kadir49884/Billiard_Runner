using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Button]

    public void SetName()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).name = "BallOnRoad" + i;
        }
    }

}
