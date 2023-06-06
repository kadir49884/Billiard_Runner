using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBallInteract 
{
    void BallCollision(GameObject newObject,Collision other);
}
