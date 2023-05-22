using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Character
{
    public class StateMachine : MonoBehaviour
    {
        public const string Regular = "Regular";
        public const string Dodge = "Dodge";
        public const string Cast = "Cast";
        public const string Stagger = "Stagger";
        public const string Stun = "Stun";

        private static Dictionary<TextAsset, List<State>> _generatedMachines = new Dictionary<TextAsset, List<State>>();
        
        [SerializeField] private TextAsset _stateTable;
        [Header("RUNTIME DEBUG ONLY")]
        [SerializeField] private State _currentState;
        [SerializeField] private List<State> _states = new List<State>();

        public State CurrentState
        {
            get => _currentState;
            private set => _currentState = value;
        }

        public State GetState(string stateName) => _states.Find(state => state.Name.Equals(stateName));

        public bool TryEnterState(string stateName) => TryEnterState(stateName, false);
        
        private bool TryEnterState(string stateName, bool regularAllowed)
        {
            if (!regularAllowed && stateName.Equals(Regular))
                throw new ArgumentException(
                    "Entering Regular state is frohibited." +
                    "Use TryExitState to enter Regular state from specific state");
            
            CheckStateForExistance(stateName);
            State newState = _states.FirstOrDefault(state => state.Name.Equals(stateName));
            if (newState == null || !CurrentState.CanSwitchToState(stateName))
                return false;

            State oldState = CurrentState;
            CurrentState = newState;
            oldState.Exit?.Invoke();
            newState.Enter?.Invoke();
            return true;
        }

        public bool TryExitState(string stateName)
        {
            if (IsCurrentState(stateName))
                return TryEnterState(Regular, true);
            return false;
        }

        public bool IsCurrentState(string stateName)
        {
            CheckStateForExistance(stateName);
            return CurrentState.Name.Equals(stateName);
        }

        public bool IsCurrentStateOneOf(params string[] stateNames) =>
            stateNames.Any(stateName => CurrentState.Name.Equals(stateName));

        public bool IsCurrentStateNoneOf(params string[] statesNames) => !IsCurrentStateOneOf(statesNames);

        public bool HasState(string stateName) => GetState(stateName) != null;

        private void Awake()
        {
            if (_generatedMachines.ContainsKey(_stateTable))
                _states = _generatedMachines[_stateTable];
            else
                GeneraleStateList();
        }

        private void Start()
        {
            CurrentState = _states[0];
            print(CurrentState);
        }

        private void GeneraleStateList()
        {
            _states = new List<State>();
            string[] lines = _stateTable.text.Split('\n');
            
            List<string> stateNames = lines[0].Split(',').ToList();
            stateNames.RemoveAt(0);
            
            for (int y = 1; y < lines.Length; y++)
            {
                _states.Add(GenerateState(lines, y, stateNames));
            }
            
            _generatedMachines.Add(_stateTable, _states);
        }

        private State GenerateState(string[] lines, int y, List<string> stateNames)
        {
            string[] currentLine = lines[y].Split(',');
            List<string> forbiddenTransitions = new List<string>();
            for (int x = 1; x < currentLine.Length; x++)
            {
                if (currentLine[x][0] == 'F')
                    forbiddenTransitions.Add(stateNames[x - 1]);
            }
            State state = new State(y, currentLine[0], forbiddenTransitions);

            return state;
        }

        private void CheckStateForExistance(string stateName)
        {
            if (!_states.Any(state => state.Name.Equals(stateName)))
                throw new Exception($"{_stateTable} has no {stateName} state");
        }
    }

    [Serializable]
    public class State
    {
        [SerializeField] private string _name;
        [SerializeField] private int _id;
        [SerializeField] private List<string> _forbiddenTransitions;

        public int ID => _id;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public Action Enter;
        public Action Exit;

        public State(int id, string name, List<string> frobiddenTransitions)
        {
            _id = id;
            Name = name;
            _forbiddenTransitions = frobiddenTransitions;
        }

        public bool CanSwitchToState(string stateName)
        {
            return !_forbiddenTransitions.Contains(stateName);
        }
    }
}
