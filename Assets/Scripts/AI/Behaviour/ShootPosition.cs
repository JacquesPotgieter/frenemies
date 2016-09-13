using UnityEngine;
using System.Collections;

public class ShootPosition : MonoBehaviour {

    public static void run(MovingObject currentObject, Vector3 position, bool MainFire) {
        float dY = currentObject.transform.position.y - position.y;
        float dX = currentObject.transform.position.x - position.x;

        currentObject.TryShoot(-dX, -dY, MainFire, currentObject.DamageDealt);
    }
}
