using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AI.Behaviours {
    class FindPlayerPath : MonoBehaviour {

        public static List<Vector2> run(MovingObject currentObject, Vector3 target, Vector2 direction) {
            Vector2 startingLocation = Vector2.zero;
            Vector2 normal = Vector2.zero;
            if (!(direction.x == 0f && direction.y == 0f))
                startingLocation = GetStartingLocation(currentObject, target, out normal);

            if (startingLocation == Vector2.zero)
                return null;
            List<Vector2> firstPart = FindPath.run(currentObject, currentObject.transform.position, startingLocation);
            List<Vector2> lastPart = FindPath.run(currentObject, startingLocation, target);

            firstPart.Add(normal);
            if (lastPart.Count > 0) {
                firstPart.Add(lastPart.First());
                firstPart.AddRange(lastPart);
            }

            LineRenderer lr = currentObject.GetComponent<LineRenderer>();
            if (GameManager.Instance.DebugMode) {
                lr.SetVertexCount(100);  //Need a higher number than 2, or crashes out
                lr.SetWidth(0.1f, 0.1f);
            }

            int count = 0;
            foreach (Vector2 pos in firstPart) {
                if (GameManager.Instance.DebugMode) {
                    lr.SetPosition(count, pos);
                }
                count++;
            }

            if (GameManager.Instance.DebugMode)
                lr.SetVertexCount(count);
            return firstPart;
        }

        private static float DistanceOut = 1f;
        private static Vector2 GetStartingLocation(MovingObject currentObject, Vector2 target, out Vector2 tempPosition) {
            Vector2 currentPosition = ((Vector2)currentObject.transform.position);
            Vector2 newVec = target - currentPosition;
            Vector2 middle = target + (currentPosition - target);
            Vector2 normal = Vector3.Cross(newVec, Vector3.forward);
            normal.Normalize();
            tempPosition = Vector2.zero;

            Node node = null;
            float tempDistance = DistanceOut;
            while (node == null && tempDistance > -1 * DistanceOut) {
                Vector2 change = new Vector2(tempDistance * normal.x, tempDistance * normal.y);
                tempPosition = new Vector2(middle.x + change.x, middle.y + change.y);

                tempDistance -= 0.5f;
                Point point = GameManager.Instance.grid.WorldToGrid(tempPosition);
                node = GameManager.Instance.grid.Nodes[point.X, point.Y];

                if (node.BadNode)
                    node = null;
            }

            return node.Position;
        }
    }
}