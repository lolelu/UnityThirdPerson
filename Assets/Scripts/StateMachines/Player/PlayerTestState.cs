using UnityEngine;

public class PlayerTestState : PlayerBaseState
{
    public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
    }

    public override void Tick(float deltaTime)
    {
        var movement = new Vector3(stateMachine.InputReader.MovementValue.x, 0,
            stateMachine.InputReader.MovementValue.y);

        stateMachine.Controller.Move(movement * (deltaTime * stateMachine.FreeLookMovementSpeed));
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat("FreeLookSpeed", 0, 0.1f, deltaTime);
            return;
        }

        stateMachine.Animator.SetFloat("FreeLookSpeed", 1, 0.1f, deltaTime);
        stateMachine.transform.rotation = Quaternion.LookRotation(movement);
    }


    public override void Exit()
    {
    }
}