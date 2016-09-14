using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindClosestHidingSpot : MonoBehaviour {

    private class Point {
        public float distance;
        public Vector2 position;
    }

    public static Vector2 run(MovingObject currentObject, Vector2 target) {
        Dictionary<Vector2, Point> obstacles = new Dictionary<Vector2, Point>();

        Dictionary<Vector2, bool> grid = GameManager.Instance._boardScript._gridPositions;
        Point best = new Point();
        best.distance = float.MaxValue;

        foreach (Vector2 curPosition in grid.Keys) {
            if (grid[curPosition]) {
                Point point = new Point();
                point.distance = Vector2.Distance(currentObject.transform.position, curPosition);
                point.position = curPosition;
                obstacles.Add(curPosition, point);

                if (point.distance < best.distance)
                    best = point;
            }
        }

        List<Point> adjacent = getAllAdjacent(best, grid);

        Vector2 hidingSpot = currentObject.transform.position;
        float distance = float.MaxValue;

        foreach (Point cur in adjacent) {
            Vector2 direction = cur.position - target;
            direction.Normalize();
            Vector3 startingPosition = currentObject.transform.position;
//            if (yDir < -0.5)
//                startingPosition.y -= (currentObject._boxcollider.size.y + 0.5f);
//
//            if (yDir > 0.5)
//                startingPosition.y += (_boxcollider.size.y + 0.1f);
//
//            if (xDir > 0.5)
//                startingPosition.x += (_boxcollider.size.x + 0.2f);
//            if (xDir < -0.5)
//                startingPosition.x -= (_boxcollider.size.x + 0.2f);
            float dist = Vector2.Distance(cur.position, currentObject.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(target, direction, 0.1f);
            if (hit.collider != null) {
                if (dist < distance) {
                    distance = dist;
                    hidingSpot = cur.position;
                }
            }
        }

        return hidingSpot;
    }

    private static List<Point> getAllAdjacent(Point point, Dictionary<Vector2, bool> grid) {
        List<Point> points = new List<Point>();
        float x = point.position.x;
        float y = point.position.y;

        Vector2 temp = new Vector2(x + 1, y);
        Point tempPoint = new Point();
        tempPoint.position = temp;
        if (grid.ContainsKey(temp))
            points.Add(tempPoint);

        temp = new Vector2(x - 1, y);
        tempPoint = new Point();
        tempPoint.position = temp;
        if (grid.ContainsKey(temp))
            points.Add(tempPoint);

        temp = new Vector2(x, y + 1);
        tempPoint = new Point();
        tempPoint.position = temp;
        if (grid.ContainsKey(temp))
            points.Add(tempPoint);

        temp = new Vector2(x, y - 1);
        tempPoint = new Point();
        tempPoint.position = temp;
        if (grid.ContainsKey(temp))
            points.Add(tempPoint);

        temp = new Vector2(x + 1, y + 1);
        tempPoint = new Point();
        tempPoint.position = temp;
        if (grid.ContainsKey(temp))
            points.Add(tempPoint);

        temp = new Vector2(x + 1, y - 1);
        tempPoint = new Point();
        tempPoint.position = temp;
        if (grid.ContainsKey(temp))
            points.Add(tempPoint);

        temp = new Vector2(x - 1, y + 1);
        tempPoint = new Point();
        tempPoint.position = temp;
        if (grid.ContainsKey(temp))
            points.Add(tempPoint);

        temp = new Vector2(x - 1, y - 1);
        tempPoint = new Point();
        tempPoint.position = temp;
        if (grid.ContainsKey(temp))
            points.Add(tempPoint);

        return points;
    }
}
