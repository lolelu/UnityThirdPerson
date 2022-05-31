using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private List<Target> targets = new();

    [SerializeField] private CinemachineTargetGroup cineTargetGroup;

    private Camera mainCamera;

    public Target CurrentTarget { get; private set; }

    private void Start()
    {
        mainCamera = Camera.main;
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

  
    public void Cancel()
    {
        if (CurrentTarget == null) return;
        cineTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }
    public bool SelectTarget()
    {
        if (targets.Count == 0) return false;


        Target closestTarget = null;
        var closestTargetDistance = Mathf.Infinity;
        foreach (var target in targets)
        {
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);
            if(!(viewPos.x is >= 0 and <= 1 && viewPos.y is >=0 and <=1)) continue;
            var toCenter = viewPos - new Vector2(0.5f, 0.5f);
            if (toCenter.sqrMagnitude < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }

        }

        if (closestTarget == null) return false;
        CurrentTarget = closestTarget;
        cineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2);
        return true;
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