using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyNormalController : AI_Controller {

    private float minDistance = 0.1f;
    private MovingObject prevTarget;
    private Vector3 prevpos;
    private bool update=true;
    

    public override void run() {
        //prevTarget = this.target;
        //StartCoroutine(Example());
        changeTarget();
        moveToTarget();
        
        if (Random.value > 0.7)
            shootTarget(false);
        else
            shootTarget(true);
        //update = false;
    }
   
    private IEnumerator Example()
    {
        update = true;
        yield return new WaitForSeconds(5);

    }
    private void moveToTarget() {
        if (target != null) {
            float distance = Vector3.Distance(this.currentObject.transform.position, this.target.transform.position);

            //if (distance > minDistance)
               //MoveToPosition.run(currentObject, target.transform.position);
               // MoveToPosition.run(currentObject, PredictivePosition.run(this.target.transform.position, prevTarget.transform.position));
        }
    }

    private void changeTarget() {
        //Predictive test
        if(update)
            prevTarget = this.target;
        List<MovingObject> targets = new List<MovingObject>();
        for (int i = 0; i < GameManager.Instance.players.Count; i++)
            targets.Add(GameManager.Instance.players[i]);

        this.target = FindClosestTarget.closestTarget(currentObject, targets);
        if (prevTarget == null)
        {
            prevTarget = this.target;

        }
        prevpos = prevTarget.transform.position;
    }

    private void shootTarget(bool MainFire) {

       
        if (target != null) {
            //ShootPosition.run(currentObject, this.target.transform.position, MainFire);
            ShootPosition.run(currentObject, PredictivePosition.run(this.target.transform.position, prevpos), MainFire);
            //PredictivePosition.run(currentObject, target.transform.position, prevTarget.transform.position, MainFire);
        }

    }
}
