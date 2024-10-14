using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Player/Decisions/StateIsDone ")]
public class DecisionStateIsDone : Decision {
    public override bool Decide(StateController stateController) {
        return stateController.CurrentStateIsDone();
    }
}
