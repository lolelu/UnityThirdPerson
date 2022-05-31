using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;
    
    protected PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * (deltaTime));
    }

    protected void FaceTarget()
    {
        if (stateMachine.Targeter.CurrentTarget == null) return;
        var lookPos = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;
        lookPos.y = 0f;
        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);

    }
    
    
    
}
