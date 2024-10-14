using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Player/States/Dash")]
public class DashState : State {
    float timer = 0;
    [SerializeField] float duration = 0.3f;
    public override void OnEnter() {
        Debug.Log("EnterDash");
    }

    public override void OnExit() {

        Debug.Log("ExitDash");
    }

    public override void UpdateState() {
        Debug.Log("UpdateDash");
        if (timer >= duration) {
            isDone = true;
        } else {
            timer += Time.deltaTime;
        }
    }
}
