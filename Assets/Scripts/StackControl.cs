using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackControl : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private List<Transform> stackList = new List<Transform>();
    private Vector3 tempPos;
    private GameManager gameManager;

    public List<Transform> StackList { get => stackList; set => stackList = value; }

    private void Awake()
    {
        //var colliders = GetComponentsInChildren<SphereCollider>();
        //Physics.IgnoreCollision(colliders[0], colliders[1]);
    }

    private void Start()
    {
        StackList.Add(transform.GetChild(0));
        StackList.Add(transform.GetChild(1));
        gameManager = GameManager.Instance;
    }

    private void FixedUpdate()
    {
        if (!gameManager.ExecuteGame)
            return;

        for (int i = 1; i < StackList.Count; i++)
        {
            tempPos = StackList[i - 1].position;

            if(i==1)
            {
                StackList[1].position = tempPos;

            }
            else
            {
                tempPos.z -= 0.6f;
                StackList[i].position = Vector3.Lerp(StackList[i].position, tempPos, 0.35f);
            }

            StackList[i].rotation = StackList[i - 1].rotation;
        }

    }

    public void AddNewBall(Transform newTransform)
    {

        StackList.Add(newTransform);
    }
    public void RemoveAtList(GameObject newObject)
    {
        if(transform.childCount>2)
        {
            StackList.Remove(newObject.transform);

        }

    }

}
