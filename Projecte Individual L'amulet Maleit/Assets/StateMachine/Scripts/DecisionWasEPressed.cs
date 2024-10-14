using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Player/Decisions/WasEPressed ")]
public class DecisionWasEPressed : Decision {
    public override bool Decide(StateController stateController) {
        return Input.GetKeyDown(KeyCode.E);
    }
}
