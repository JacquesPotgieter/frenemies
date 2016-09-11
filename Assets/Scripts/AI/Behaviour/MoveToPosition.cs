using UnityEngine;
using System.Collections;

public class MoveToPosition : MonoBehaviour {

    public static void run(MovingObject currentObject, Vector3 position) {
        int yDir = 0, xDir = 0;

        if (Mathf.Abs(position.y - currentObject.transform.position.y) > 3) {
            yDir = position.y > currentObject.transform.position.y ? 1 : -1;
        }

        if (Mathf.Abs(position.x - currentObject.transform.position.x) > 3)
            xDir = position.x > currentObject.transform.position.x ? 1 : -1;
        
        if (Mathf.Abs(xDir) > double.Epsilon || Mathf.Abs(yDir) > double.Epsilon)
            currentObject.Move(xDir, yDir);
    }
}
