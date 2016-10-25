using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Panda;
using Assets.Scripts.AI.Behaviours;

public class Behaviour : MonoBehaviour {

    private MovingObject EnemyObject;
    private Vector2 ShootingTarget;
    private Vector2 MovementTarget;

    private float minDistanceNotToIgnore = 3f;
    private int SwarmSize = 2;
    private float range = 3f;

    private Vector3 PrevMovementTarget;


    private List<Vector2> movementPath;

    #region MOVEMENT METHODS ------------------------------------------------------------------------------------------------------------------------------
    [Task]
    public void BuildPath() { // Builds a path to be taken towards the MovementTarget
        MovingObject currentObject = gameObject.GetComponent<MovingObject>();

        if (MovementTarget != null)
            movementPath = FindPath.run(currentObject, currentObject.transform.position, MovementTarget);

        Task.current.Succeed();
    }

    [Task]
    public void BuildFlankPath(bool opposite) { // Builds a path to be taken towards the MovementTarget
        MovingObject currentObject = gameObject.GetComponent<MovingObject>();

        if (MovementTarget != null)
            movementPath = FindFlankPath.run(currentObject, MovementTarget, opposite);

        if (movementPath == null)
            Task.current.Fail();

        Task.current.Succeed();
    }

    [Task]
    public void Move() { // Goes to the next position in the Movmement Path
        MovingObject obj = gameObject.GetComponent<MovingObject>();

        if (MovementTarget != null && !MovementTarget.Equals((Vector2) obj.transform.position)) {
            MoveToPosition.run(obj, MovementTarget, movementPath);
            Task.current.Succeed();
            return;
        }

        Task.current.Fail();
    }

    [Task]
    public void MoveToEnemy() { // Changes MovementTarget to a ClosestEnemy
        MovingObject obj = gameObject.GetComponent<MovingObject>();

        List<MovingObject> players = GameManager.Instance.players.Cast<MovingObject>().ToList();
        EnemyObject = FindClosestTarget.closestTarget(obj, players);
        if (EnemyObject != null) {
            MovementTarget = EnemyObject.transform.position;
            Task.current.Succeed();
            return;
        }

        Task.current.Fail();
    }

    [Task]
    public void MoveToHidingSpot() { // Changes MovementTarget to a HidingSpot
        MovingObject obj = gameObject.GetComponent<MovingObject>();

        List<MovingObject> players = GameManager.Instance.players.Cast<MovingObject>().ToList();
        EnemyObject = FindClosestTarget.closestTarget(obj, players);
        MovementTarget = FindClosestHidingSpot.run(obj, EnemyObject);

        if (MovementTarget.Equals((Vector2) obj.transform.position)) {
            Task.current.Fail();
            return;
        }

        Task.current.Succeed();
    }



    #endregion MOVEMENT METHODS -------------------------------------------------------------------------------------------------------------------------


    #region SWARM METHODS -------------------------------------------------------------------------------------------------------------------------

    [Task]
    public void SwarmMoveToEnemy()
    { // Changes MovementTarget to ClosestEnemy or Enemy with lowest health.
        MovingObject currentObject = gameObject.GetComponent<MovingObject>();
        List<MovingObject> targets = new List<MovingObject>();
        for (int i = 0; i < GameManager.Instance.players.Count; i++)
            targets.Add(GameManager.Instance.players[i]);

        MovingObject target = FindLowestHealthTarget.LowestTarget(currentObject, targets);
        MovingObject potentialTarget = FindClosestTarget.closestTarget(currentObject, targets);
        if (target != null && potentialTarget != null)
        {
            float distance = Vector3.Distance(currentObject.transform.position, potentialTarget.transform.position);
            if (distance <= minDistanceNotToIgnore)
                MovementTarget = potentialTarget.transform.position;
            else
                MovementTarget = target.transform.position;

            Task.current.Succeed();
            return;
        }

        Task.current.Fail();
    }

    [Task]
    private void InRange() // Checks if inrange to shootfaster
    {
        MovingObject currentObject = gameObject.GetComponent<MovingObject>();
        int count = 0;
        for (int i = 0; i < GameManager.Instance.enemies.Count; i++)
            if (Vector3.Distance(currentObject.transform.position, GameManager.Instance.enemies[i].transform.position) < range)
                count++;
        if (count > SwarmSize)
            currentObject.TimeBetweenShotsMain = 0.2f;
        else
            currentObject.TimeBetweenShotsMain = 0.6f;
        Task.current.Succeed();
    }

    #endregion SWARM METHODS -------------------------------------------------------------------------------------------------------------------------

    [Task]
    public void PredictiveMoveToEnemy()
    { // Changes MovementTarget to a Predicted location of enemy
        MovingObject obj = gameObject.GetComponent<MovingObject>();

        List<MovingObject> players = GameManager.Instance.players.Cast<MovingObject>().ToList();
        EnemyObject = FindClosestTarget.closestTarget(obj, players);
        if (EnemyObject != null)
        {
            if (ShootingTarget != null)
            {
                PrevMovementTarget = MovementTarget;
            }
            else
            {
                PrevMovementTarget = EnemyObject.transform.position;
            }

            Vector3 finalpos = EnemyObject.transform.position;
            Vector3 velocity = (EnemyObject.transform.position - PrevMovementTarget) / Time.deltaTime;

            velocity *= Time.deltaTime;
            velocity *= 0.6f;
            finalpos += velocity;

            if (EnemyObject.transform.position.x - PrevMovementTarget.x > 0)
            {
                MovementTarget = finalpos;
                MovementTarget.x += 2.85f;
                Debug.Log("right");
              
            }
            else
            {
                MovementTarget = finalpos;
                MovementTarget.x += -2.85f;
                Debug.Log("left");
             
            }
         
            Task.current.Succeed();
            return;
        }

        Task.current.Fail();
    }

    [Task]
    public void PredictivePos()
    { // Changes MovementTarget to a ClosestEnemy
        MovingObject obj = gameObject.GetComponent<MovingObject>();

        List<MovingObject> players = GameManager.Instance.players.Cast<MovingObject>().ToList();
        EnemyObject = FindClosestTarget.closestTarget(obj, players);
        if (EnemyObject != null)
        {
            if (ShootingTarget != null)
            {
                PrevMovementTarget = ShootingTarget;
            }
            else
            {
                PrevMovementTarget = EnemyObject.transform.position;
            }
            //MovementTarget = PredictivePosition.run(EnemyObject.transform.position, PrevMovementTarget);
            Vector3 finalpos = EnemyObject.transform.position;
            Vector3 velocity = (EnemyObject.transform.position - PrevMovementTarget) / Time.deltaTime;

            velocity *= Time.deltaTime;
            velocity *= 0.6f;
            finalpos += velocity;
            //MovementTarget = EnemyObject.transform.position + (EnemyObject.transform.position * 0.15f);
            
            if (EnemyObject.transform.position.x - PrevMovementTarget.x> 0)
            {
                ShootingTarget = finalpos;
                ShootingTarget.x += 1.85f;
                Debug.Log("right");
                //if (EnemyObject.transform.position.y - PrevMovementTarget.y > 0)
                //{
                //    ShootingTarget.y += 1.15f;
                //    Debug.Log("right-Up");
                //}
                //else {
                //    ShootingTarget.y -= 1.15f;
                //    Debug.Log("right-Down");
                //}
            }
            else
            {
                ShootingTarget = finalpos;
                ShootingTarget.x += -1.85f;
                Debug.Log("left");
                //if (EnemyObject.transform.position.y - PrevMovementTarget.y > 0)
                //{
                //    ShootingTarget.y += 1.15f;
                //    Debug.Log("left-Up");
                //}
                //else
                //{
                //    ShootingTarget.y -= 1.15f;
                //    Debug.Log("left-Down");
                //}
            }
            //if (EnemyObject.transform.position.y - PrevMovementTarget.y > 0)
            //{


            //    MovementTarget.y += 1.15f;
            //}
            //else
            //{

            //    MovementTarget.y += -1.15f;
            //}
            Task.current.Succeed();
            return;
        }

        Task.current.Fail();
    }


    #region Shooting Methods
    [Task]
    public void AimForMovementEnemy() {
        this.ShootingTarget = this.MovementTarget;

        Task.current.Succeed();
    }

    [Task]
    public void EnemyVisible() {
        Vector2 direction = (Vector2)gameObject.transform.position - ShootingTarget;
        direction.Normalize();
        float dist = Vector2.Distance((Vector2)gameObject.transform.position, ShootingTarget);

        BoxCollider2D col = gameObject.GetComponent<BoxCollider2D>();
        col.enabled = false;
        RaycastHit2D hit = Physics2D.Raycast((Vector2)gameObject.transform.position, direction, dist);
        col.enabled = true;

        if (hit.collider == null)
            Task.current.Succeed();
        else
            Task.current.Fail();
    }

    [Task]
    public void ShootTarget() {
        bool mainFire = !(Random.value > 0.7);

        MovingObject obj = gameObject.GetComponent<MovingObject>();

        ShootPosition.run(obj, ShootingTarget, mainFire);

        Task.current.Succeed();
    }

    #endregion Shooting Methods
}