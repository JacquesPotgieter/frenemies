using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindPath : MonoBehaviour {

    public static List<Vector2> run(MovingObject currentObject, Vector3 startingPosition, Vector3 target) {
        Grid grid = GameManager.Instance.grid;
        List<Vector2> movementPath = new List<Vector2>();
        Point startPosition = grid.WorldToGrid(startingPosition);
        Point endPosition = grid.WorldToGrid(target);

        //Find path from player to clicked position
        BreadCrumb bc = PathFinder.FindPath(grid, startPosition, endPosition);

        int count = 0;
        LineRenderer lr = currentObject.GetComponent<LineRenderer>();
        if (GameManager.Instance.globalSettings.DebugMode) {            
            lr.SetVertexCount(100);  //Need a higher number than 2, or crashes out
            lr.SetWidth(0.1f, 0.1f);
            lr.SetColors(Color.yellow, Color.yellow);
        }

        //Draw out our path
        while (bc != null) {
            Vector2 curPos = grid.GridToWorld(bc.position);
            movementPath.Add(curPos);

            if (GameManager.Instance.globalSettings.DebugMode) {
                lr.SetPosition(count, curPos);
            }            
            bc = bc.next;
            count += 1;
        }
        if (GameManager.Instance.globalSettings.DebugMode)
            lr.SetVertexCount(count);

        return movementPath;
    }
}
