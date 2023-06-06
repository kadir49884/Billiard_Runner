using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Upperpik;

public class HelperWall : MonoBehaviour
{
    [SerializeField]
    private bool right;

    private void OnTriggerEnter(Collider other)
    {
        if(UpperEnum.State == GameStatus.Fail)
        {
            Destroy(other.gameObject.GetComponent<SphereCollider>());
            Vector3 direction = right ? Vector3.right : Vector3.left;

            other.gameObject.GetComponent<Rigidbody>().AddForce(direction*30000 + Vector3.up*30000);

            Run.After(1, () =>
            {
                transform.position = Vector3.zero;
            });
        }
    }
}
