using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Upperpik;

public class MassShoot : MonoBehaviour
{

    [SerializeField]
    private LayerMask layerMask;
    [SerializeField, ReadOnly]
    private List<Collider> holeCollider;

    [SerializeField]
    private List<float> ballGoHoleTime;
    [SerializeField]
    private List<int> holeIndex;

    private GameObject playerObject;
    private GameObject playerParent;
    [SerializeField, ReadOnly]
    private List<Transform> stackList;

    void Start()
    {
        holeCollider = Physics.OverlapSphere(transform.position, 20, layerMask).ToList();
        holeCollider.Sort(a => a.name);
        playerObject = ObjectControl.Instance.Player;
        playerParent = ObjectControl.Instance.PlayerParent;

    }

    private void OnTriggerEnter(Collider other)
    {
        UnityBallCrashActive();
    }

    private void Update()
    {
        for (int i = 0; i < stackList.Count; i++)
        {
            stackList[i].position = new Vector3(stackList[i].position.x, Mathf.Clamp(stackList[i].position.y, -2, 1.1f), stackList[i].position.z);
        }

        playerParent.GetChild(1).position = playerObject.transform.position;
        playerParent.GetChild(1).rotation = playerObject.transform.rotation;

    }

    private void UnityBallCrashActive()
    {

        Rigidbody playerBody = playerObject.GetComponent<Rigidbody>();
        playerBody.mass = 30;
        DOTween.To((float value) => { playerBody.drag = value; }, 0, 5, 3f);
        DOTween.To((float value) => { playerBody.angularDrag = value; }, 0, 3, 3f);

        playerObject.transform.DOLocalMoveY(0, 1);


        if (UpperEnum.State == GameStatus.Finish1)
        {
            int j = 1;
            for (int i = stackList.Count-1; i > 1 ; i -= (int)ScalaControl.ScalaStatus + 1)
            {
                
                    stackList[i].DOMoveX(holeCollider[holeIndex[j - 1]].transform.position.x, ballGoHoleTime[j - 1]).SetUpdate(UpdateType.Fixed).SetEase(Ease.Linear);
                    stackList[i].DOMoveZ(holeCollider[holeIndex[j - 1]].transform.position.z, ballGoHoleTime[j - 1]).SetUpdate(UpdateType.Fixed).SetEase(Ease.Linear);
                    stackList[i].LookAtY(holeCollider[holeIndex[j - 1]].transform, 1);

                    stackList[i].DOLocalRotate(new Vector3(360, 0, 0), 0.5f, RotateMode.LocalAxisAdd)
                        .SetLoops(-1, LoopType.Incremental)
                        .SetEase(Ease.Linear);
           
                j++;
            }

            for (int i = 1; i < stackList.Count; i++)
            {
                Rigidbody body = stackList[i].GetComponent<Rigidbody>();

                DOTween.To((float value) => { body.drag = value; }, 0, 3, 5f).SetDelay(2);
                DOTween.To((float value) => { body.angularDrag = value; }, 0, 3, 5f).SetDelay(2);
            }


        }
    }

    public void AddList(List<Transform> newList)
    {
        stackList = newList;
    }

}
