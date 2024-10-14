using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Transition {
    [SerializeField] Decision decisionToBeMade;
    [SerializeField] State onDecisionTrueExitState;
    [SerializeField] State onDecisionFalseExitState;

    public State GetExitState(StateController stateController) {
        return decisionToBeMade.Decide(stateController) ? onDecisionTrueExitState : onDecisionFalseExitState;
    }
}
