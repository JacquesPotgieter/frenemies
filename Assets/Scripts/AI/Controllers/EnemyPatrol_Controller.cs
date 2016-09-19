using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AI.Controllers
{
    class EnemyPatrol_Controller: AI_Controller
    {
        private float minDistance = 0.1f;
        private MovingObject prevTarget;
        private Vector3 prevpos;
        private bool update = true;
        private float range = 3f;
        private float Startx = -0.5f;
        private float Endx = 29.5f;
        private float Starty = -0.5f;
        private float Endy = 15.5f;
        private bool atStart = false;


        public override void run()
        {
            moveToStart();
            changeTarget();
            ISatStart();
            if(atStart)
                moveToPosition();

            if(InRange())
                shootTarget(true);
            else
                shootTarget(false);

        }

        private bool CanMoveLeft()
        {
            if (this.currentObject.transform.position.x < Endx)
                return true;
            else
                return false;
        }
        private bool CanMoveDown()
        {
            if (this.currentObject.transform.position.y > Starty)
                return true;
            else
                return false;
        }

        private void ISatStart()
        {
            if (this.currentObject.transform.position == new Vector3(Startx, Starty, 0f))
                atStart = true;

        }
      

        private void moveToPosition()
        {
            if (CanMoveLeft()&&!CanMoveDown())
            {
                MoveToPosition.run(currentObject, new Vector3(Startx, Starty++, 0f));
            }
            if (CanMoveLeft() && CanMoveDown())
            {
                MoveToPosition.run(currentObject, new Vector3(Startx++, Starty, 0f));
            }
            if (!CanMoveLeft() && !CanMoveDown())
            {
                MoveToPosition.run(currentObject, new Vector3(Startx--, Starty, 0f));
            }
            if (!CanMoveLeft() && CanMoveDown())
            {
                MoveToPosition.run(currentObject, new Vector3(Startx, Starty--, 0f));
            }
        }
        private void moveToStart()
        {
           MoveToPosition.run(currentObject, new Vector3(Startx, Starty, 0f));
        }

        private void changeTarget()
        {
            //Predictive test
            if (update)
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

        private void shootTarget(bool MainFire)
        {


            if (target != null)
            {
               
                ShootPosition.run(currentObject, PredictivePosition.run(this.target.transform.position, prevpos), MainFire);
                
            }

        }

        private bool InRange()
        {
            // List<MovingObject> targets = new List<MovingObject>();
            bool isInRange = false;
            int count = 0;
            for (int i = 0; i < GameManager.Instance.players.Count; i++)
                if (Vector3.Distance(this.currentObject.transform.position, GameManager.Instance.players[i].transform.position) < range)
                    count++;
            if (count > 0)
                isInRange = true;
            return isInRange;

        }
    }
}
