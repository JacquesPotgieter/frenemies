using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindClosestHidingSpot : MonoBehaviour {

    public static Vector2 run(MovingObject currentObject, MovingObject target) {
        GameManager gameManger = GameManager.Instance;

        for (int x = -2; x < 33; x++) 
            for (int y = -3; y < 20; y++) {
                Node node = getNode(new Vector2(x, y));

                if (!node.BadNode) {
                    if (canHide(node, currentObject, target)) {
                        return node.Position;
                    }
                }
            }

        return currentObject.transform.position;
    }

    private static bool canHide(Node node, MovingObject currentObj, MovingObject enemyObj) {
        Vector3 direction = enemyObj.transform.position - currentObj.transform.position;
        direction.Normalize();
        float dist = Vector2.Distance(currentObj.transform.position, enemyObj.transform.position);

        BoxCollider2D col = currentObj.GetComponent<BoxCollider2D>();
        BoxCollider2D colObj = enemyObj.GetComponent<BoxCollider2D>();
        col.enabled = false;
        colObj.enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(enemyObj.transform.position, direction, dist * 0.8f);
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
