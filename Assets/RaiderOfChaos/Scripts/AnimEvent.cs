using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hyhy.RaidersOfChaos
{
    public class AnimEvent : MonoBehaviour
    {
        public Actor actor;
        private GameObject weapon;

        private void Start()
        {
            
        }

        public void Dash()
        {
            if (actor)
            {
                actor.Dash();
            }
        }
    }
}
