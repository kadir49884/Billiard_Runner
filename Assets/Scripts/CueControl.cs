using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Upperpik;

public class CueControl : MonoBehaviour
{
    [SerializeField]
    private bool right;

    private GameObject playerObject;

    private Transform otherTransform;

    void Start()
    {
        playerObject = ObjectControl.Instance.Player;

        if (right)
            transform.parent.DOMoveX(5, Random.Range(0.5f, 1)).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InQuart);
        else
            transform.parent.DOMoveX(-5, Random.Range(0.5f, 1)).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InQuart);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<ICrash>()?.Crashed(right);
    }

}
