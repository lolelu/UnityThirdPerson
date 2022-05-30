using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
        [SerializeField]
        private List<Target> targets = new();

        public Target CurrentTarget
        {
                get;
                private set;
        }

        private void OnTriggerEnter(Collider other)
        {
                if (!other.TryGetComponent(out Target target)) return;
                targets.Add(target);
        }

        private void OnTriggerExit(Collider other)
        {
                if (!other.TryGetComponent(out Target target)) return;
                targets.Remove(target);
        }

        public bool SelectTarget()
        {
                if (targets.Count == 0) return false;

                CurrentTarget = targets[0];
                return true;
        }

        public void Cancel()
        {
                CurrentTarget = null;
        }
}