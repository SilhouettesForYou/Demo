using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class StateMachine<T>
    {
        // previous state
        public MachineBehaviourState<T> PreState { get; private set; }

        // current state
        public MachineBehaviourState<T> CurrentState { get; private set; }

        // global state
        public MachineBehaviourState<T> GlobalState { get; private set; }

        public StateMachine(T owner, MachineBehaviourState<T> firstState, MachineBehaviourState<T> globalState = null)
        {
            if (globalState != null)
            {
                GlobalState = globalState;
                GlobalState.owner = owner;
                GlobalState.Enter();
            }
            CurrentState = firstState;
            CurrentState.owner = owner;
            CurrentState.Enter();
        }

        public void ChangeState(MachineBehaviourState<T> targetState)
        {
            if (targetState == null)
            {
                throw new System.Exception("Can't find target state.");
            }
            targetState.owner = CurrentState.owner;
            PreState = CurrentState;
            CurrentState.Exit();
            CurrentState = targetState;
            CurrentState.Enter();
        }

        public void StateMacheUpdate()
        {
            if (GlobalState != null)
                GlobalState.Excute();
            if (CurrentState != null)
                CurrentState.Excute();
        }
    }
}
