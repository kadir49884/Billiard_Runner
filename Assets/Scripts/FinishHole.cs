using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Upperpik.Managers;

public class FinishHole : MonoBehaviour
{
    private CanvasManager canvasManager;
    private ParticleManager particleManager;


    private void Start()
    {
        canvasManager = CanvasManager.Instance;
        particleManager = GameManager.Instance.ParticleManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.DOMoveY(transform.position.y - 8f, 0.8f).SetEase(Ease.Linear);
        canvasManager.ScoreUpdate();
        Vector3 particlePos = transform.position;

        particlePos.y -= 5f;
        particlePos.z += 2f;

        particleManager.GetParticle(ParticleType.HoleConfetti, particlePos/*, parent: transform*/);
        Run.After(1f, () =>
        {
            other.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

        });

    }
}
