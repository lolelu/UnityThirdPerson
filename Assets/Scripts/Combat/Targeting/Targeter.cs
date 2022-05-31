using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Targeter : MonoBehaviour
{
        [SerializeField]
        private List<Target> targets = new();

        [SerializeField] private CinemachineTargetGroup cineTargetGroup;

        public Target CurrentTarget
        {
                get;
                private set;
        }

        private void OnTriggerEnter(Collider other)
        {
                if (!other.TryGetComponent(out Target target)) return;
                targets.Add(target);
                target.OnDestroyed += RemoveTarget;
        }

       

        private void OnTriggerExit(Collider other)
        {
                if (!other.TryGetComponent(out Target target)) return;
                RemoveTarget(target);
        }

        public bool SelectTarget()
        {
                if (targets.Count == 0) return false;

                CurrentTarget = targets[0];
                cineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2);
                return true;
        }

        public void Cancel()
        {
                if (CurrentTarget == null) return;
                cineTargetGroup.RemoveMember(CurrentTarget.transform);
                CurrentTarget = null;
        }
        
        private void RemoveTarget(Target target)
        {
                if (CurrentTarget == target)
                {
                        cineTargetGroup.RemoveMember(CurrentTarget.transform);
                        CurrentTarget = null;
                }

                target.OnDestroyed -= RemoveTarget;
                targets.Remove(target);
        }
}
