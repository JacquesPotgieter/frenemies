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

        Task.current.Succeed();
    }

    [Task]
    public void Move() {
        MovingObject obj = gameObject.GetComponent<MovingObject>();

        if (target != null) {
            MoveToPosition.run(obj, target.transform.position, movementPath);
            Task.current.Succeed();
        }

        Task.current.Fail();
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
        MovingObject currentObject = gameObject.GetComponent<MovingObject>();

        movementPath = FindPath.run(currentObject, target.transform.position);

        Task.current.Succeed();
    }
}