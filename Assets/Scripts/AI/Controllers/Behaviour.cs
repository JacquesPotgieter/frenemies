using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Panda;

public class Behaviour : MonoBehaviour {

    private MovingObject EnemyObject;
    private Vector2 ShootingTarget;
    private Vector2 MovementTarget;

    private List<Vector2> movementPath;

    #region MOVEMENT METHODS ------------------------------------------------------------------------------------------------------------------------------
    [Task]
    public void BuildPath() { // Builds a path to be taken towards the MovementTarget
        MovingObject currentObject = gameObject.GetComponent<MovingObject>();

        if (MovementTarget != null)
            movementPath = FindPath.run(currentObject, MovementTarget);

        Task.current.Succeed();
    }

    [Task]
    public void Move() { // Goes to the next position in the Movmement Path
        MovingObject obj = gameObject.GetComponent<MovingObject>();

        if (MovementTarget != null) {
            MoveToPosition.run(obj, MovementTarget, movementPath);
            Task.current.Succeed();
        }

        Task.current.Fail();
    }

    [Task]
    public void MoveToEnemy() { // Changes MovementTarget to a ClosestEnemy
        MovingObject obj = gameObject.GetComponent<MovingObject>();

        List<MovingObject> players = GameManager.Instance.players.Cast<MovingObject>().ToList();
        EnemyObject = FindClosestTarget.closestTarget(obj, players);
        if (EnemyObject != null)
            MovementTarget = EnemyObject.transform.position;

        Task.current.Succeed();
    }

    [Task]
    public void MoveToHidingSpot() { // Changes MovementTarget to a HidingSpot
        MovingObject obj = gameObject.GetComponent<MovingObject>();

        List<MovingObject> players = GameManager.Instance.players.Cast<MovingObject>().ToList();
        EnemyObject = FindClosestTarget.closestTarget(obj, players);
        MovementTarget = FindClosestHidingSpot.run(obj, EnemyObject);

        Task.current.Succeed();
    }

    #endregion MOVEMENT METHODS -------------------------------------------------------------------------------------------------------------------------



    [Task]
    public void ShootTarget() {
        bool mainFire = !(Random.value > 0.7);

        MovingObject obj = gameObject.GetComponent<MovingObject>();

        ShootPosition.run(obj, ShootingTarget, mainFire);

        Task.current.Succeed();
    }
}