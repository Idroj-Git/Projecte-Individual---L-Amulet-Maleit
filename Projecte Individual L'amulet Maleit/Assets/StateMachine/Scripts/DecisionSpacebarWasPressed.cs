using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Player/Decisions/SpacebarWasPressed ")]
public class DecisionSpacebarWasPressed : Decision {
    public override bool Decide(StateController stateController) {
        return Input.GetKeyDown(KeyCode.Space);
    }
}
