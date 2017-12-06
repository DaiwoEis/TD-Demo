using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace FSM
{
    public class FSM<TStateType>
    {
        public class State
        {
            public event Action<State> OnEnter;
            public event Action<State> OnUpdate;
            public event Action<State> OnExit;

            public readonly TStateType StateType;

            protected FSM<TStateType> _fsm;

            public State(FSM<TStateType> fsm, TStateType stateType)
            {
                _fsm = fsm;
                StateType = stateType;
            }

            public virtual void Enter()
            {
                if (OnEnter != null) OnEnter(this);
            }

            public virtual void Update()
            {
                if (OnUpdate != null) OnUpdate(this);
            }

            public virtual void Exit()
            {
                if (OnExit != null) OnExit(this);
            }

            public T As<T>() where T : State
            {
                return (T)this;
            }
        }

        public object Owner;

        private readonly Dictionary<TStateType, State> stateDictionary;

        private State currentState;

        [ShowInInspector]
        public TStateType CurrentState
        {
            get { return currentState.StateType; }

            set { ChangeState(value); }
        }

        public FSM(object owner)
        {
            Owner = owner;
            stateDictionary = new Dictionary<TStateType, State>();
        }

        public void AddState(State state)
        {
            stateDictionary[state.StateType] = state;
        }

        public void ChangeState(TStateType newState)
        {
            if (currentState != null)
            {
                currentState.Exit();
            }

            currentState = stateDictionary[newState];

            if (currentState != null)
            {
                currentState.Enter();
            }
        }

        public void Update()
        {
            if (currentState != null)
            {
                currentState.Update();
            }
        }
    }
}
