using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Upperpik;
using Upperpik.Managers;

public class BallOnRoad : MonoBehaviour
{
    [SerializeField] private Ease ease;

    private Rigidbody rb;
    private Vector3 direction;
    private GameObject player;
    private GameObject playerParent;
    private PlayerMovementController playerMovementController;

    [SerializeField, ReadOnly]
    private List<Collider> holeCollider;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private bool ballLeft = false;

    [SerializeField]
    private bool isFinishBall = false;

    [SerializeField]
    private bool control = false;

    private CanvasManager canvasManager;

    private ParticleManager particleManager;
    private ObjectControl objectControl;
    private GameObject finishTableCenter;

    private float finishTableAxisZ;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        objectControl = ObjectControl.Instance;
        player = objectControl.Player;
        playerParent = objectControl.PlayerParent;
        finishTableCenter = objectControl.FinishTableCenter;
        playerMovementController = player.GetComponent<PlayerMovementController>();
        finishTableAxisZ = finishTableCenter.transform.position.z;


        holeCollider = Physics.OverlapSphere(transform.position, 15, layerMask).ToList();

        holeCollider.Sort(a => Vector3.Distance(a.transform.position, transform.position));

        if (holeCollider.Count > 0 && ballLeft && holeCollider[0].transform.position.x > 0)
        {
            holeCollider.Remove(holeCollider[0]);
        }
        else if (holeCollider.Count > 0 && !ballLeft && holeCollider[0].transform.position.x < 0)
        {
            holeCollider.Remove(holeCollider[0]);
        }


        canvasManager = CanvasManager.Instance.GetComponent<CanvasManager>();
        particleManager = GameManager.Instance.ParticleManager;
    }
    private void Update()
    {
        if(!isFinishBall)
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -2, 1), Mathf.Clamp(transform.position.z, 0, finishTableAxisZ));
    }

    public void BallInHole(Transform newPos)
    {
        transform.DOMoveY(transform.position.y - 8f, 0.8f).SetEase(Ease.Linear);
        canvasManager.ScoreUpdate();
        Vector3 particlePos = transform.position;

        particlePos.y -= 5f;
        particlePos.z += 2f;

        particleManager.GetParticle(ParticleType.HoleConfetti, particlePos/*, parent: transform*/);

        Material newMat = GetComponent<MeshRenderer>().material;

        Run.After(1f, () =>
        {
            Destroy(GetComponent<SphereCollider>());
            GetComponent<MeshRenderer>().enabled = false;
            if(!isFinishBall)
            {
                player.GetComponent<PlayerCollider>().GetStack(newMat);
            }
        });
        Destroy(rb);
    }

    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.GetComponent<IBallInteract>()?.BallCollision(gameObject, other);
    }



    public void BallGoHole(Transform newPos, Collision other)
    {

        if (UpperEnum.State != GameStatus.UnityBall && UpperEnum.State != GameStatus.Finish1)
        {
            ContactPoint contact = other.contacts[0];

            float distance = Vector3.Distance(transform.position, holeCollider[0].transform.position) / 8;


            if (contact.point.x > transform.position.x && ballLeft && transform.position.z < holeCollider[0].transform.position.z)
            {
                transform.DOMoveX(holeCollider[0].transform.position.x, distance).SetUpdate(UpdateType.Fixed).SetEase(ease, .01f);
                transform.DOMoveZ(holeCollider[0].transform.position.z, distance).SetUpdate(UpdateType.Fixed).SetEase(ease, .01f);
                transform.LookAtY(holeCollider[0].transform, -1);

                transform.DOLocalRotate(new Vector3(0, -360, 0), 0.5f, RotateMode.LocalAxisAdd)
                    .SetLoops(-1)
                    .SetEase(Ease.Linear).SetUpdate(UpdateType.Fixed);

            }
            else if (contact.point.x < transform.position.x && !ballLeft && transform.position.z < holeCollider[0].transform.position.z)
            {
                transform.DOMoveX(holeCollider[0].transform.position.x, distance).SetUpdate(UpdateType.Fixed).SetEase(ease, .01f);
                transform.DOMoveZ(holeCollider[0].transform.position.z, distance).SetUpdate(UpdateType.Fixed).SetEase(ease, .01f);
                transform.LookAtY(holeCollider[0].transform, -1);
                transform.DOLocalRotate(new Vector3(0, -360, 0), 0.5f, RotateMode.LocalAxisAdd)
                    .SetLoops(-1)
                    .SetEase(Ease.Linear).SetUpdate(UpdateType.Fixed);

            }
            else
            {
                //playerMovementController.MultiSpeed = 0.3f;
                //Run.After(1, () =>
                //{
                //    playerMovementController.MultiSpeed = 1f;
                //});

                direction = transform.position - newPos.position;
                rb.velocity = direction.normalized * 25;

            }
        }



    }

    




    //public void VelocityActive(GameObject newObject)
    //{
    //    if (!control)
    //    {
    //        Vector3 newDirection = (transform.position - newObject.transform.position).normalized;
    //        //rb.velocity = new Vector3(newDirection.x * 10, 0,  newDirection.z * 50);
    //        rb.velocity = newDirection * newObject.GetComponent<Rigidbody>().velocity.magnitude + Vector3.forward;
    //        //DOTween.To((float value) => { rb.drag = value; }, 0, 4, 3).SetDelay(2);

    //    }
    //}

    //public void SpeedPlus()
    //{
    //    //rb.velocity =  Vector3.forward * 10f;
    //}


}
