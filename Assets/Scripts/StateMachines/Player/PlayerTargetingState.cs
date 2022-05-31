using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerTargetingState : PlayerBaseState


    {
        private readonly int _targetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
        private readonly int _targetingForwardHash = Animator.StringToHash("TargetingForward");
        private readonly int _targetingRightHash = Animator.StringToHash("TargetingRight");

        public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.InputReader.CancelEvent += OnCancel;
            stateMachine.Animator.Play(_targetingBlendTreeHash);
        }

        public override void Tick(float deltaTime)
        {
            if (stateMachine.InputReader.IsAttacking)
            {
                stateMachine.SwitchState(new PlayerAttackingState(stateMachine));
                return;
            }
            if (stateMachine.Targeter.CurrentTarget == null)
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                return;
            }

            var movement = CalculateMovement();
            Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);

            UpdateAnimator(deltaTime);

            FaceTarget();
        }


        public override void Exit()
        {
            stateMachine.InputReader.CancelEvent -= OnCancel;
        }

        private void OnCancel()
        {
            stateMachine.Targeter.Cancel();
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }

        private Vector3 CalculateMovement()
        {
            var movement = new Vector3();

            var stateMachineTransform = stateMachine.transform;
            movement += stateMachineTransform.right * stateMachine.InputReader.MovementValue.x;
            movement += stateMachineTransform.forward * stateMachine.InputReader.MovementValue.y;

            return movement;
        }

        private void UpdateAnimator(float deltaTime)
        {
            stateMachine.Animator.SetFloat(_targetingForwardHash,
                stateMachine.InputReader.MovementValue.y == 0 ? 0f :
                stateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f,
                0.1f, deltaTime);
            
            stateMachine.Animator.SetFloat(_targetingRightHash,
                stateMachine.InputReader.MovementValue.x == 0 ? 0f :
                stateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f,
                0.1f, deltaTime);
        }
    }
}