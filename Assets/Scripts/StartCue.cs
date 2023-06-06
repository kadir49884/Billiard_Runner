using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Upperpik;

public class StartCue : MonoBehaviour
{
    [SerializeField]
    private Ease ease;

    private static StartCue instance;

    public static StartCue Instance { get => instance; set => instance = value; }

    private GameObject playerObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        playerObject = ObjectControl.Instance.Player;
    }

    public void GameStarted()
    {
        transform.DOMoveZ(transform.position.z - 4, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOMoveZ(transform.position.z + 4.5f, 0.1f).SetEase(ease).OnComplete(() =>
            {

                playerObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * 40000);

                GameManager.Instance.GameStart();
            });
        });
    }

    public void CueFinishShoot()
    {

        transform.DOMove(-transform.up * 4, 0.5f).SetRelative().SetEase(Ease.Linear).OnComplete(() =>
          {
              transform.DOMove(transform.up * 4.5f, 0.1f).SetRelative().SetEase(ease).OnComplete(() =>
              {
                  ObjectControl.Instance.Player.GetComponent<PlayerMovementController>().FinishShoot();
              });

          });
    }
}
