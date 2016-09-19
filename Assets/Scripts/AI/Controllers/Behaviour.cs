using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Panda;

public class Behaviour : MonoBehaviour {

    private MovingObject target;

    private List<Vector2> movementPath;

    [Task]
    public void TargetClosestEnemy() {
        MovingObject obj = gameObject.GetComponent<MovingObject>();

        List<MovingObject> players = GameManager.Instance.players.Cast<MovingObject>().ToList();
        target = FindClosestTarget.closestTarget(obj, players);

        Task task = Task.current;
        task.Succeed();
    }

    [Task]
    public void Move() {
        MovingObject obj = gameObject.GetComponent<MovingObject>();
        Task task = Task.current;

        if (target != null) {
            MoveToPosition.run(obj, target.transform.position);
            task.Succeed();
        }

        task.Fail();
    }

    [Task]
    public void ShootTarget() {
        bool mainFire = !(Random.value > 0.7);

        MovingObject obj = gameObject.GetComponent<MovingObject>();

        ShootPosition.run(obj, target.transform.position, mainFire);

        Task.current.Succeed();
    }

    [Task]
    public void BuildPath() {
        
    }
}