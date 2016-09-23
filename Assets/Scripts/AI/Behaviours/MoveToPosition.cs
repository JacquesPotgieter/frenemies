using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEditor.VersionControl;
public class MoveToPosition {

    public static void run(MovingObject currentObject, Vector3 target, List<Vector2> movementPath) {
        Vector2 point = getNextPoint(currentObject.transform.position, target, movementPath);
        Vector2 movement = point - (Vector2) currentObject.transform.position;
        //movement.Normalize();
        currentObject.Move(movement.x, movement.y);
    }

    private static Vector2 getNextPoint(Vector2 currentPos, Vector2 target, List<Vector2> movementPath) {
        float minDistance = 0.6f;

        for (int i = 0; i < movementPath.Count; i++) {
            float curDistance = Vector2.Distance(movementPath[i], currentPos);

            if (curDistance > minDistance)
                return movementPath[i];
        }

        return currentPos;
    }
}
