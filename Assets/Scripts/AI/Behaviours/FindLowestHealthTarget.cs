using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindLowestHealthTarget : MonoBehaviour {

    public static MovingObject LowestTarget(MovingObject currentObject, List<MovingObject> targets) {
        int lowest = -1;
        double minHealth = double.MaxValue;

        for (int i = 0; i < targets.Count; i++) {
           int health= GameManager.Instance.players[i].getHealth();

            if (health < minHealth) {
                lowest = i;
                minHealth = health;
            }
        }

        if (lowest == -1)
            return null;
        return targets[lowest];
     }
}
