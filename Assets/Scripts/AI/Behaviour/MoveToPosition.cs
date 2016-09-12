using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEditor.VersionControl;
public class MoveToPosition : MonoBehaviour {
    private static int pos = 0;
    private static float lastDistance = float.MaxValue;
    private static Vector2 target;
     
    public static void run(MovingObject currentObject, Vector3 position) {
        Vector2 start = new Vector2(Mathf.RoundToInt(currentObject.transform.position.x),
            Mathf.RoundToInt(currentObject.transform.position.y));

        if (pos >= currentObject.movement.Count) {
            currentObject.movement = aStarSearch(start, position);
            pos = 0;
            target = position;
        }


        Vector2 end = getCurrentMove(currentObject.movement, start);
        float yDir = end.y - start.y;
        float xDir = end.x - start.x;

        if (Mathf.Abs(xDir) > double.Epsilon || Mathf.Abs(yDir) > double.Epsilon)
            currentObject.Move(xDir, yDir);
    }

    private static Vector2 getCurrentMove(List<Vector2> movement, Vector2 current) {
        for (; pos < movement.Count; pos++)
        {
            float tempD = Vector2.Distance(movement[pos], target);

            if (tempD < lastDistance)
            {
                lastDistance = tempD;
                return movement[pos];
            }
        }

        return Vector2.zero;
    }

    private static List<Vector2> aStarSearch(Vector2 start, Vector2 end) {
        List<Vector2> closedSet = new List<Vector2>();
        List<Vector2> openSet = new List<Vector2>();
        openSet.Add(start);

        Dictionary<Vector2, Vector2> cameFrom = new Dictionary<Vector2, Vector2>();

        Dictionary<Vector2, Double> gScore = new Dictionary<Vector2, Double>();
        gScore.Add(start, 0);
        Dictionary<Vector2, Double> fScore = new Dictionary<Vector2, Double>();

        fScore.Add(start, heuristicDistance(start, end));

        while (openSet.Count > 0) {
            Vector2 current = lowestFvalue(fScore, openSet);

            if (isGoal(current, end)) {
                return reconstructPath(cameFrom, current);
            }

            openSet.Remove(current);
            closedSet.Add(current);
            List<Vector2> children = getChildren(current);

            foreach (Vector2 child in children) {
                if (!closedSet.Contains(child)) {
                    double score = getScore(gScore, current) + heuristicDistance(current, child);

                    if (!openSet.Contains(child)) 
                        openSet.Add(child);
                    else if (score > getScore(gScore, child))
                        continue;

                    cameFrom.Remove(child);
                    cameFrom.Add(child, current);
                    gScore.Remove(child);
                    gScore.Add(child, score);
                    fScore.Remove(child);
                    fScore.Add(child, score + heuristicDistance(child, end));
                }
            }

        }

        return new List<Vector2>();
    }

    private static double getScore(Dictionary<Vector2, Double> map, Vector2 point) {
        if (map.ContainsKey(point))
            return map[point];
        return Double.MaxValue;
    }

    private static List<Vector2> getChildren(Vector2 point) {
        float minDistance = 0.5f;

        float sphereSize = 2f;

        List<Vector2> children = new List<Vector2>();

        Vector3 newPoint = new Vector3(point.x - minDistance, point.y, 0f);
        if (!Physics.CheckSphere(newPoint, sphereSize))
            children.Add(newPoint);

        newPoint = new Vector3(point.x + minDistance, point.y, 0f);
        if (!Physics.CheckSphere(newPoint, sphereSize))
            children.Add(newPoint);

        newPoint = new Vector3(point.x, point.y - minDistance, 0f);
        if (!Physics.CheckSphere(newPoint, sphereSize))
            children.Add(newPoint);

        newPoint = new Vector3(point.x, point.y + minDistance, 0f);
        if (!Physics.CheckSphere(newPoint, sphereSize))
            children.Add(newPoint);

        newPoint = new Vector3(point.x - minDistance, point.y - minDistance, 0f);
        if (!Physics.CheckSphere(newPoint, sphereSize))
            children.Add(newPoint);

        newPoint = new Vector3(point.x - minDistance, point.y + minDistance, 0f);
        if (!Physics.CheckSphere(newPoint, sphereSize))
            children.Add(newPoint);

        newPoint = new Vector3(point.x + minDistance, point.y - minDistance, 0f);
        if (!Physics.CheckSphere(newPoint, sphereSize))
            children.Add(newPoint);

        newPoint = new Vector3(point.x + minDistance, point.y + minDistance, 0f);
        if (!Physics.CheckSphere(newPoint, sphereSize))
            children.Add(newPoint);

        return children;
    }

    private static double heuristicDistance(Vector2 start, Vector2 end) {
        double dist = 0;

        dist += Mathf.Abs(start.x - end.x);
        dist += Mathf.Abs(start.y - end.y);

        return dist;
    }

    private static Vector2 lowestFvalue(Dictionary<Vector2, Double> fValue, List<Vector2> openSet ) {
        Vector2 minPoint = Vector2.zero;
        double minScore = Double.MaxValue;

        foreach (Vector2 key in openSet) {
            double tempScore = fValue[key];

            if (tempScore < minScore)
            {
                minPoint = key;
                minScore = tempScore;
            }
        }

        return minPoint;
    }

    private static bool isGoal(Vector2 current, Vector2 end) {
        int minDistance = 1;

        return (Mathf.Abs(current.x - end.x) < minDistance
                && Mathf.Abs(current.y - end.y) < minDistance);
    }

    private static List<Vector2> reconstructPath(Dictionary<Vector2, Vector2> cameFrom, Vector2 start) {
        Vector2 current = start;
        List<Vector2> movement = new List<Vector2>();

        while (cameFrom.ContainsKey(current)) {
            movement.Add(current);
            current = cameFrom[current];
        }

        return movement;
    }
}
