using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackCollider : MonoBehaviour, ICrash
{
    private GameObject playerParent;
    private GameObject playerObject;

    private void Start()
    {
        playerParent = ObjectControl.Instance.PlayerParent;
        playerObject = ObjectControl.Instance.PlayerParent;
    }

    public void Crashed(bool newBool)
    {
        if(playerParent.transform.childCount > 2)
        {
            playerParent.GetComponent<StackControl>()?.RemoveAtList(gameObject);
            transform.parent = null;
            gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * 20000);

        }



    }
}
