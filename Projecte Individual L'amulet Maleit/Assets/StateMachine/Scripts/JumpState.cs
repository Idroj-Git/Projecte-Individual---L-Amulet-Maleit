using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Player/States/JumpState")]
public class JumpState : State {
    [SerializeField] float duration;
    float timer = 0;
    public override void OnEnter() {
        Debug.Log("Empiezo animación saltar");
    }

    public override void OnExit() {
        Debug.Log("Cancela si hace falta");
    }

    public override void UpdateState() {
        timer += Time.deltaTime;
        if (timer > duration) {
            isDone = true;
        } else {
            Debug.Log("Espero a terminar animación saltar");
        }
    }
}
