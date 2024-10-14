using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject {
    protected StateController stateController;
    public bool isDone = false;
    [SerializeField] Transition[] possibleTransitions;
    public abstract void OnEnter();
    public abstract void UpdateState();
    public abstract void OnExit();

    public void SetStateController(StateController stateController) {
        this.stateController = stateController;
    }

    public State CheckTransitions() {
        State exitState = null;
        for (int i = 0; i < possibleTransitions.Length && exitState == null; i++) {
            exitState = possibleTransitions[i].GetExitState(stateController);
        }
        return exitState;
    }
}
