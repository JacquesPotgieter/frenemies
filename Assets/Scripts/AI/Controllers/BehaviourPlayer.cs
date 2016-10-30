using Assets.Scripts.AI.Behaviours;
using Panda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AI.Controllers {
    class BehaviourPlayer : MonoBehaviour {

        private MovingObject EnemyObject;
        private Vector2 ShootingTarget;
        private Vector2 MovementTarget;

        public String state = "Circle";

        private List<Vector2> movementPath;

        [Task]
        public void isState(String state) {
            if (this.state != null && this.state.Equals(state))
                Task.current.Succeed();
            else
                Task.current.Fail();
        }

        [Task]
        public void Move() {
            MovingObject obj = gameObject.GetComponent<MovingObject>();
            if (MovementTarget != null && !MovementTarget.Equals((Vector2)obj.transform.position)) {
                MoveToPosition.run(obj, MovementTarget, movementPath);
                Task.current.Succeed();
                return;
            }
            Task.current.Fail();
        }

        [Task]
        public void ShootTarget() {
            bool mainFire = !(UnityEngine.Random.value > 0.7);

            MovingObject obj = gameObject.GetComponent<MovingObject>();

            if (ShootingTarget != null)
                ShootPosition.run(obj, ShootingTarget, mainFire);

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

        #region Select Movement Tree
        [Task]
        public void MovementToClosestEnemy() {
            MovingObject obj = gameObject.GetComponent<MovingObject>();

            List<MovingObject> enemies = GameManager.Instance.enemies.Cast<MovingObject>().ToList();
            EnemyObject = FindClosestTarget.closestTarget(obj, enemies);
            if (EnemyObject != null) {
                MovementTarget = EnemyObject.transform.position;
                Task.current.Succeed();
                return;
            }

            Task.current.Fail();
        }

        [Task]
        public void MovementToHidingSpot() {
            MovingObject obj = gameObject.GetComponent<MovingObject>();

            List<MovingObject> enemies = GameManager.Instance.enemies.Cast<MovingObject>().ToList();
            EnemyObject = FindClosestTarget.closestTarget(obj, enemies);
            if (EnemyObject != null)
                MovementTarget = FindClosestHidingSpot.run(obj, EnemyObject);

            if (MovementTarget.Equals((Vector2)obj.transform.position)) {
                Task.current.Fail();
                return;
            }

            Task.current.Succeed();
        }
        #endregion

        #region Building Movement Tree
        [Task]
        public void BuildPath() {
            MovingObject currentObject = gameObject.GetComponent<MovingObject>();

            if (MovementTarget != null)
                movementPath = FindPath.run(currentObject, currentObject.transform.position, MovementTarget);

            Task.current.Succeed();
        }

        [Task]
        public void BuildCirclePath() {
            MovingObject currentObject = gameObject.GetComponent<MovingObject>();

            if (MovementTarget != null) {
                float max = 0.5f;
                movementPath = FindPlayerPath.run(currentObject, MovementTarget, new Vector2(0f, UnityEngine.Random.Range(-1 * max, max)));
            }

            Task.current.Succeed();
        }
        #endregion

        #region Select Shooting Tree
        [Task]
        public void AimForMovementEnemy() {
            if (EnemyObject != null)
                ShootingTarget = EnemyObject.transform.position;

            if (ShootingTarget == null) {
                Task.current.Fail();
                return;
            }

            Task.current.Succeed();
        }

        #endregion

        #region State Changer Tree

        [Task]
        public void ShouldHide() {
            if (!state.Equals("Hiding") && false) {
                MovingObject obj = gameObject.GetComponent<MovingObject>();
                if (EnemyObject != null) {
                    if (Vector2.Distance(obj.transform.position, EnemyObject.transform.position) < 1f) {
                        Debug.Log(Vector2.Distance(obj.transform.position, EnemyObject.transform.position));
                        state = "Hiding";
                        Task.current.Succeed();
                        return;
                    }
                }
            }

            Task.current.Fail();
        }

        [Task]
        public void ShouldCircle() {
            if (!state.Equals("Circle")) {
                MovingObject obj = gameObject.GetComponent<MovingObject>();
                if (EnemyObject != null) {
                    if (Vector2.Distance(obj.transform.position, EnemyObject.transform.position) > 1f) {
                        state = "Circle";
                        Task.current.Succeed();
                        return;
                    }
                }
            }

            Task.current.Fail();
        }
        #endregion

    }
}
