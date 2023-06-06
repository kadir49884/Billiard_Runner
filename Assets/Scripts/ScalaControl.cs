using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Upperpik;

public enum ScalaStatus
{
    success1,success2,success3
}

public class ScalaControl : MonoBehaviour
{
    private float valueZ;

    private GameObject startCube;

    private static ScalaStatus scalaStatus;

    private GameObject scalaParent;

    public static ScalaStatus ScalaStatus { get => scalaStatus; set => scalaStatus = value; }

    void Start()
    {
        transform.DOLocalRotate(new Vector3(0, 0, 25), 0.7f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetId("RotateId");
        startCube = ObjectControl.Instance.StartCue;
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DOTween.Kill("RotateId");
            valueZ = transform.localEulerAngles.z;

            valueZ = FloatExt.ConvertToAngle180(valueZ);

            Run.After(0.5f, () =>
            {
                gameObject.transform.parent.gameObject.SetActive(false);
                GameManager.Instance.GameStart();
            });

            if (valueZ > -4 && valueZ < 4)
            {
                scalaStatus = ScalaStatus.success1;
            }
            else if (valueZ > -15 && valueZ < 15)
            {
                scalaStatus = ScalaStatus.success2;
            }
            else
            {
                scalaStatus = ScalaStatus.success3;
            }

            Run.After(1, () =>
            {
                startCube.GetComponent<StartCue>().CueFinishShoot();
                
            });
        }
    }
}
