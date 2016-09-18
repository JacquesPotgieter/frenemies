using UnityEngine;
using System.Collections;

public class ShootPosition : MonoBehaviour {

    public static void run(MovingObject currentObject, Vector3 position, bool MainFire) {
        float dY = currentObject.transform.position.y - position.y;// + Random.Range(-0.5f, 0.5f);
        float dX = currentObject.transform.position.x - position.x;// + Random.Range(-0.5f, 0.5f);

        currentObject.TryShoot(-dX, -dY, MainFire, currentObject.DamageDealt);
    }
}
