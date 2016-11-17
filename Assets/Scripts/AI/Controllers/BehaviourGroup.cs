using Assets.Scripts.AI.Behaviours;
using Panda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AI.Controllers {
    class BehaviourGroup : MonoBehaviour{

        private static MovingObject EnemyObject;
        private static Vector2 ShootingTarget;
        private static Vector2 MovementTarget;
        private static bool LeftFlanked = false;

        private bool flankDirection;
        private bool hiding = false;
        public String state = "Direct";
        private MovingObject partner;

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
        public void MovementToPartner() {
            if (partner != null) {
                MovementTarget = partner.transform.position;
                Task.current.Succeed();
                return;
            }

            Task.current.Fail();
        }

        [Task]
        public void MovementToClosestEnemy() {
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
        public void MovementToHidingSpot() {
            MovingObject obj = gameObject.GetComponent<MovingObject>();

            List<MovingObject> players = GameManager.Instance.players.Cast<MovingObject>().ToList();
            EnemyObject = FindClosestTarget.closestTarget(obj, players);
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
        public void BuildFlankPath() {
            MovingObject currentObject = gameObject.GetComponent<MovingObject>();

            if (MovementTarget != null) {
                movementPath = FindFlankPath.run(currentObject, MovementTarget, flankDirection);
            }

            if (movementPath == null)
                Task.current.Fail();

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
        public void ShouldGroupUp() {
            if (!state.Equals("Grouping")) {
                MovingObject obj = gameObject.GetComponent<MovingObject>();

                foreach (MovingObject other in GameManager.Instance.enemies) {
                    BehaviourGroup group = other.GetComponent<BehaviourGroup>();

                    if (group != null && (group != this || group.state != "Hiding")) {
                        partner = other;
                    }
                }

                if (partner != null 
                    && Vector2.Distance(obj.transform.position, partner.transform.position) > 10f) {
                    state = "Grouping";
                    Task.current.Succeed();
                    return;
                }
            } else {
                if (partner != null 
                    && (Vector2.Distance(gameObject.transform.position, partner.transform.position) <= 4f
                    || partner.GetComponent<BehaviourGroup>().state.Equals("Hiding"))) {
                    Task.current.Fail();
                    return;
                } else {
                    Task.current.Succeed();
                    return;
                }
            }

            Task.current.Fail();
        }

        [Task]
        public void ShouldHide() {
            if (state.Equals("Hiding")) {
                Task.current.Succeed();
                return;
            } else {
                Enemy obj = gameObject.GetComponent<Enemy>();
                if (obj.getHealth() <= 10) {
                    state = "Hiding";
                    Task.current.Succeed();
                    return;
                }
            }

            Task.current.Fail();
        }

        [Task]
        public void ShouldAttackHeadon() {
            if (!state.Equals("Direct")) {
                MovingObject obj = gameObject.GetComponent<MovingObject>();
                if (Vector2.Distance(obj.transform.position, MovementTarget) < 3f) {
                    state = "Direct";
                    Task.current.Succeed();
                    return;
                }
            }

            Task.current.Fail();
        }

        [Task]
        public void ShouldFlank() {
            if (!state.Equals("Flank")) {
                MovingObject obj = gameObject.GetComponent<MovingObject>();
                if (Vector2.Distance(obj.transform.position, MovementTarget) >= 3f) {
                    state = "Flank";
                    Task.current.Succeed();

                    flankDirection = LeftFlanked;
                    LeftFlanked = !LeftFlanked;
                    return;
                }
            }

            Task.current.Fail();
        }


        #endregion

    }
}
