using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindClosestHidingSpot : MonoBehaviour {

    private static float MaxDistance = 4;

    public static Vector2 run(MovingObject currentObject, MovingObject target) {
        GameManager gameManger = GameManager.Instance;

        Vector2 hidingSpot = currentObject.transform.position;

        for (int x = -2; x < 33; x++) 
            for (int y = -3; y < 20; y++) {
                Vector2 vector = new Vector2(x, y);
                float distance = Vector2.Distance(currentObject.transform.position, vector);

                if (distance < MaxDistance) {
                    Node node = getNode(vector);

                    if (node != null && !node.BadNode) {
                        if (canHide(node, currentObject, target)) {
                            hidingSpot = node.Position;
                        }
                    }
                }
            }

        return hidingSpot;
    }

    private static bool canHide(Node node, MovingObject currentObj, MovingObject enemyObj) {
        Vector2 direction = node.Position - (Vector2)enemyObj.transform.position;
        direction.Normalize();
        float dist = Vector2.Distance(node.Position, enemyObj.transform.position);

        BoxCollider2D col = currentObj.GetComponent<BoxCollider2D>();
        BoxCollider2D colObj = enemyObj.GetComponent<BoxCollider2D>();
        col.enabled = false;
        colObj.enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(enemyObj.transform.position, direction, dist);
        col.enabled = true;
        colObj.enabled = true;

        return (hit.collider != null);
    }

    private static Node getNode(Vector2 worldPosition) {
        Grid grid = GameManager.Instance.grid;
        Point point = grid.WorldToGrid(worldPosition);
        if (point != null)
            return grid.Nodes[point.X, point.Y];
        return null;
    }
}
