using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public abstract class MachineBehaviourState<TMonoBehaviour>
    {
        public TMonoBehaviour owner;

        public abstract void Enter();

        public abstract void Excute();

        public abstract void Exit();
    }
}
