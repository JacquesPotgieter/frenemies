using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySwarm_controller : AI_Controller {

    private float minDistance = 3f;
    private float range = 3f;
    private float minDistanceNotToIgnore = 3f;
    private MovingObject prevTarget;
    private int SwarmSize = 2;

    public override void run()
    {
        changeTarget();
        moveToTarget();
        if(InRange())
            shootTarget(true);
    }

    private void moveToTarget()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(this.currentObject.transform.position, this.target.transform.position);

            if (distance > minDistance)
                MoveToPosition.run(currentObject, target.transform.position);
        }
    }

    private void changeTarget()
    {
        prevTarget = this.target;

        List<MovingObject> targets = new List<MovingObject>();
        for (int i = 0; i < GameManager.Instance.players.Count; i++)
            targets.Add(GameManager.Instance.players[i]);
        
        MovingObject target = FindLowestHealthTarget.LowestTarget(currentObject, targets);
        MovingObject potentialTarget = FindClosestTarget.closestTarget(currentObject, targets);
        float distance = Vector3.Distance(this.currentObject.transform.position, potentialTarget.transform.position);
        if (distance <= minDistanceNotToIgnore)
            this.target = potentialTarget;
        else
            this.target = target;

    }

    private void shootTarget(bool MainFire)
    {
        if (target != null)
            ShootPosition.run(currentObject, PredictivePosition.run(this.target.transform.position,prevTarget.transform.position), MainFire);

        //PredictivePosition.run(currentObject, target.transform.position, prevTarget.transform.position, MainFire);
    }

    private bool InRange()
    {
        // List<MovingObject> targets = new List<MovingObject>();
        bool isInRange = false;
        int count = 0;
        for (int i = 0; i < GameManager.Instance.enemies.Count; i++)
            if (Vector3.Distance(this.currentObject.transform.position, GameManager.Instance.enemies[i].transform.position) < range)
                count++;
        if (count > SwarmSize)
            isInRange = true;
        return isInRange;
        
    }
}
