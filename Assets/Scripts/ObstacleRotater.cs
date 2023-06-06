using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Upperpik;

public class ObstacleRotater : MonoBehaviour
{
    private GameObject playerObject;

    private Transform otherTransform;



    void Start()
    {
        playerObject = ObjectControl.Instance.Player;

        transform.DOLocalRotate(new Vector3(0, 360, 0), Random.Range(1.5f,4), RotateMode.LocalAxisAdd)
                    .SetLoops(-1).SetUpdate(UpdateType.Fixed)
                    .SetEase(Ease.Linear);
    }
    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.GetComponent<ICrash>()?.Crashed(true);
        
    }
}
