using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Upperpik;

public class Turbo : MonoBehaviour
{
    private PlayerMovementController playerMovementController;
    private GameObject playerObject;
    private void Start()
    {
        playerObject = ObjectControl.Instance.Player;
        playerMovementController = playerObject.GetComponent<PlayerMovementController>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMovementController>())
        {

            if (UpperEnum.State == GameStatus.UnityBall)
            {
                playerMovementController.MultiSpeed = 2f;
                UpperEnum.State = GameStatus.Finish1;

                Run.After(1.5f, () =>
                {
                    UpperEnum.State = GameStatus.Normal;
                    playerMovementController.MultiSpeed = 1f;
                });
            }
            else
            {
                UpperEnum.State = GameStatus.UnityBall;
            }
        }

    }
}
