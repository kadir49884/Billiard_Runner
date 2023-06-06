using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Upperpik;

public class PlayerCollider : MonoBehaviour, IBallInteract, IFinishControl, ICrash
{

    private GameObject prefabBall;
    private GameObject playerObject;
    private GameObject playerParent;
    private GameObject newBall;
    private ObjectControl objectControl;
    private GameObject startCueParent;
    private GameObject unityBallParent;
    private bool finishControl = false;
    private GameManager gameManager;

    private StackControl stackControl;


    [SerializeField]
    private List<Transform> unityBallList = new List<Transform>();



    private void Start()
    {
        gameManager = GameManager.Instance;

        objectControl = ObjectControl.Instance;
        prefabBall = objectControl.PrefabBall;
        playerObject = objectControl.Player;
        playerParent = objectControl.PlayerParent;
        unityBallParent = objectControl.UnityBallParent;
        startCueParent = objectControl.StartCue.transform.parent.gameObject;

        stackControl = playerParent.GetComponent<StackControl>();
      
        for (int i = 0; i < unityBallParent.transform.childCount; i++)
        {
            unityBallList.Add(unityBallParent.GetChild(i));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            GetStack(transform.GetChild(0).GetComponent<MeshRenderer>().material);
        }
    }

    public void BallCollision(GameObject newObject, Collision other)
    {
        newObject.GetComponent<BallOnRoad>()?.BallGoHole(transform, other);
    }
    public void GetStack(Material newMat)
    {
        Vector3 newPos = playerObject.transform.position;

        newPos.z -= 10;
        newBall = Instantiate(prefabBall, position: newPos, Quaternion.identity, playerParent.transform);
        newBall.transform.GetChild(0).GetComponent<MeshRenderer>().material = newMat;
        newBall.AddComponent<StackCollider>();
        newBall.SetLayer("StackBall");
        playerParent.GetComponent<StackControl>()?.AddNewBall(newBall.transform);

    }


    public void GameFinished()
    {
        if (!finishControl)
        {
            
            List<Transform> tempList =  stackControl.StackList;

            UpperEnum.State = GameStatus.Finish1;
            gameManager.GameWin();
            CameraMovement.Instance.CameraNewPos();

            Rigidbody playerRb = GetComponent<Rigidbody>();
            playerRb.isKinematic = true;

            unityBallParent.GetComponent<MassShoot>().AddList(tempList);


            Rigidbody childBody;
            for (int i = 2; i < tempList.Count; i++)
            {
                Transform stack = tempList[i];
                Transform stack2 = unityBallList[tempList.Count - i - 1];
                float delayTime = i * 0.2f;
                stack.GetComponent<Rigidbody>().isKinematic = true;

                stack.DOMove(stack2.position, 0.5f).OnComplete(() =>
                {
                    childBody = stack2.GetComponent<Rigidbody>();
                    childBody.StopMovement();
                }).SetDelay(delayTime);
            }

            startCueParent.transform.DOMoveX(playerObject.transform.position.x, 1).SetEase(Ease.Linear);
            startCueParent.transform.DOMoveZ(playerObject.transform.position.z, 1).SetEase(Ease.Linear);

            Run.After(0.9f, () =>
            {
                startCueParent.transform.LookAtY(objectControl.FinishTableCenter.transform, 1);
                startCueParent.transform.SetLocalAngleX(90);
            });

            Run.After(tempList.Count * 0.3f, () =>
            {
                objectControl.ScalaParent.SetActive(true);

                finishControl = true;
                for (int i = 1; i < tempList.Count; i++)
                {
                    Transform stack = tempList[i];
                    stack.GetComponent<Rigidbody>().isKinematic = false;
                }
                playerRb.isKinematic = false;

            });

        }
    }

    public void Crashed(bool right)
    {
        //UpperEnum.State = GameStatus.Fail;
        //gameManager.GameFail();

        //playerObject.SetLayer("FailBall");
        //Vector3 direction = right ? Vector3.left : Vector3.right;
        //gameObject.GetComponent<Rigidbody>().AddForce(direction * 40000);

        //Run.After(2, () =>
        //{
        //    CanvasManager.Instance.GameFailed();
        //});
    }
}
