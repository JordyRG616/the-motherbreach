using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmatureActionCaller : MonoBehaviour
{
    private BossController controller;
    [SerializeField] private List<ParticleSystem> particles;
    private Animator _animator;
    [SerializeField] private Transform tracker;
    private bool tracking;
    private bool reseting;

    void Start()
    {
        controller = GetComponentInParent<BossController>();
    }

    public void CallAction()
    {
        controller.TRIGGERDELAYEDACTION();
    }

    public void StartDeployEffect()
    {
        particles.ForEach(x => x.Play());
    }

    public void StopDeployEffect()
    {
        particles.ForEach(x => x.Stop());
    }

    public void Track()
    {
        tracking = true;
    }

    public void StopTracking()
    {
        tracking = false;
        reseting = true;
    }
}
