using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEditor.VersionControl;
public class MoveToPosition {

    private class Point {
        public Vector2 position;
        public float gScore = 0;
        public float hScore = 0;
        public NodeState state = NodeState.Untested;
        public Point previous;
        public bool isWalkable;

        public float getFscore() {
            return gScore + hScore;
        }

        public enum NodeState { Untested, Open, Closed }
    }

    public static void run(MovingObject currentObject, Vector3 target) {
        aStar(currentObject, target);
    }

    private static void retarded(MovingObject currentObject, Vector2 target) {
        Vector2 end = (target - (Vector2)currentObject.transform.position).normalized;

        currentObject.Move(end.x, end.y);
    }

    private static void aStar(MovingObject currentObject, Vector3 target) {
        Vector2 startingPosition = currentObject.transform.position;
        target = new Vector2(Mathf.RoundToInt(target.x),
            Mathf.RoundToInt(target.y));

        if (target.x < 0)
            target.x = 0;
        if (target.y < 0)
            target.y = 0;
        Dictionary<Vector2, Point> grid = new Dictionary<Vector2, Point>();
        for (int x = 0; x <= GameManager.Instance._boardScript.BoardWidth; x++) {
            for (int y = 0; y <= GameManager.Instance._boardScript.BoardHeight; y++) {
                Vector2 pos = new Vector2(x, y);
                Point point = new Point();
                point.position = pos;
                point.gScore = distance(startingPosition, pos);
                point.hScore = distance(pos, target);
                point.isWalkable = GameManager.Instance._boardScript._gridPositions[pos];
                point.previous = null;

                grid.Add(pos, point);
            }
        }

        Vector2 start = new Vector2(Mathf.RoundToInt(startingPosition.x),
            Mathf.RoundToInt(startingPosition.y));
        if (grid.ContainsKey(start)) {
            bool success = search(grid[start], grid, target);

            if (success) {
                Point moveToPoint = new Point();
                Point node = grid[target];
                while (node.previous != null) {
                    moveToPoint = node;
                    node = node.previous;
                }

                //moveToPoint.position -= new Vector2(0.5f, 0.5f);

                float dX = moveToPoint.position.x - currentObject.transform.position.x;
                float dY = moveToPoint.position.y - currentObject.transform.position.y;

                currentObject.Move(dX, dY);
            }
        }
    }

    private static float distance(Vector2 start, Vector2 end) {
        return Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y);
    }

    private static bool search(Point point, Dictionary<Vector2, Point> grid, Vector2 target) {
        point.state = Point.NodeState.Closed;
        List<Point> nextNodes = getWalkAdjacent(point, grid);
        nextNodes.Sort((node1, node2) => node1.getFscore().CompareTo(node2.getFscore()));
        foreach (Point curPoint in nextNodes) {
            if (curPoint.position.Equals(target))
                return true;

            if (search(curPoint, grid, target))
                return true;
        }
        return false;
    }

    private static List<Point> getWalkAdjacent(Point point, Dictionary<Vector2, Point> grid) {
        List<Point> walkablePoints = new List<Point>();
        List<Point> adjacentPoints = getAllAdjacent(point, grid);

        int maxWidth = GameManager.Instance._boardScript.BoardWidth;
        int maxHeight = GameManager.Instance._boardScript.BoardHeight;
        foreach (Point curPoint in adjacentPoints) {
            float x = curPoint.position.x;
            float y = curPoint.position.y;

            if (x < 0 || x > maxWidth || y < 0 || y > maxHeight)
                continue;

            if (!curPoint.isWalkable)
                continue;

            if (curPoint.state == Point.NodeState.Closed)
                continue;
            
            if (curPoint.state == Point.NodeState.Open) {
                float traverseCost = distance(curPoint.position, curPoint.previous.position);
                float gTemp = point.gScore + traverseCost;
                if (gTemp < curPoint.gScore) {
                    curPoint.previous = point;
                    walkablePoints.Add(curPoint);
                }
            } else {
                curPoint.previous = point;
                curPoint.state = Point.NodeState.Open;
                walkablePoints.Add(curPoint);
            }
        }

        return walkablePoints;
    }

    private static List<Point> getAllAdjacent(Point point, Dictionary<Vector2, Point> grid) {
        List<Point> points = new List<Point>();
        float x = point.position.x;
        float y = point.position.y;

        Vector2 temp = new Vector2(x + 1, y);
        if (grid.ContainsKey(temp))
            points.Add(grid[temp]);

        temp = new Vector2(x - 1, y);
        if (grid.ContainsKey(temp))
            points.Add(grid[temp]);

        temp = new Vector2(x, y + 1);
        if (grid.ContainsKey(temp))
            points.Add(grid[temp]);

        temp = new Vector2(x, y - 1);
        if (grid.ContainsKey(temp))
            points.Add(grid[temp]);

        temp = new Vector2(x + 1, y + 1);
        if (grid.ContainsKey(temp))
            points.Add(grid[temp]);

        temp = new Vector2(x + 1, y - 1);
        if (grid.ContainsKey(temp))
            points.Add(grid[temp]);

        temp = new Vector2(x - 1, y + 1);
        if (grid.ContainsKey(temp))
            points.Add(grid[temp]);

        temp = new Vector2(x - 1, y - 1);
        if (grid.ContainsKey(temp))
            points.Add(grid[temp]);

        return points;
    }
}
