using Panda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AI.Controllers {
    class BehaviourGroup : MonoBehaviour{

        private String state;
        private List<Behaviour> members = new List<Behaviour>();

        [Task]
        public void ValidateGroup() {
            int i = 0;

            while (i < members.Count) {
                Behaviour cur = members[i];

                if (cur == null)
                    members.RemoveAt(i);
                else
                    i++;
            }

            Task.current.Succeed();
        }

        [Task]
        public void isState(String state) {
            if (this.state.Equals(state))
                Task.current.Succeed();
            else
                Task.current.Fail();
        }

        [Task]
        public void BuildFlankPaths() {
            foreach (Behaviour cur in members) {
                cur.BuildFlankPath(BTRandom.randomValue() > 0.5f);
            }
        }

    }
}
