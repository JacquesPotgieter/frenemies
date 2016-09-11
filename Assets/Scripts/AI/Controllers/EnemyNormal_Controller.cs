using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyNormalController : AI_Controller {

    private float minDistance = 5;

    public override void run() {
        changeTarget();
        moveToTarget();

        shootTarget(true);
    }

    private void moveToTarget() {
        if (target != null) {
            float distance = Vector3.Distance(this.currentObject.transform.position, this.target.transform.position);

            if (distance > minDistance)
                MoveToPosition.run(currentObject, target.transform.position);
        }
    }

    private void changeTarget() {
        List<MovingObject> targets = new List<MovingObject>();
        for (int i = 0; i < GameManager.Instance.players.Count; i++)
            targets.Add(GameManager.Instance.players[i]);

        this.target = FindClosestTarget.closestTarget(currentObject, targets);
    }

    private void shootTarget(bool MainFire) {
        if (target != null)
            ShootPosition.run(currentObject, target.transform.position, MainFire);
    }
}
