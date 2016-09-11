using UnityEngine;
using System.Collections;

public class ShootPosition : MonoBehaviour {

    public static void run(MovingObject currentObject, Vector3 position, bool MainFire) {
        int yDir = position.y > currentObject.transform.position.y ? 1 : -1;
        int xDir = position.x > currentObject.transform.position.x ? 1 : -1;

        currentObject.TryShoot(yDir, xDir, MainFire, currentObject.DamageDealt);
    }
}
