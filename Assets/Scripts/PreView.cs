using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreView : MonoBehaviour
{
    private RaycastHit hit;

    [SerializeField]
    private LayerMask layerMask;
    private LineRenderer lineRenderer;
    private GameObject playerObject;
    private Vector3 newPos;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        playerObject = ObjectControl.Instance.Player; 
    }


    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, 100, layerMask))
        {
            lineRenderer.SetPosition(0, hit.transform.position);
            
            newPos = new Vector3(hit.transform.position.x - ( transform.position.x - hit.transform.position.x) * 2f, 0.5f, hit.transform.position.z+10);
            lineRenderer.SetPosition(1, newPos );

            
        }
    }
}
