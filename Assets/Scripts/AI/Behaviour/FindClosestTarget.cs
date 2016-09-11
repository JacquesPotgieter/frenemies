using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindClosestTarget : MonoBehaviour {

    public static MovingObject closestTarget(MovingObject currentObject, List<MovingObject> targets) {
        int closest = -1;
        double minDistance = double.MaxValue;

        for (int i = 0; i < targets.Count; i++) {
            float distance = Vector3.Distance(currentObject.transform.position, targets[i].transform.position);

            if (distance < minDistance) {
                closest = i;
                minDistance = distance;
            }
        }

        if (closest == -1)
            return null;
        return targets[closest];
     }
}
