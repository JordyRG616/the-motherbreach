using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Controller : MonoBehaviour
{
    [SerializeField] private List<FormationState> states;
    private int activeState = -1;


    void Start()
    {
        StartCoroutine(ManageStates());

        StartCoroutine(ActivateChildren());
    }

    private IEnumerator ActivateChildren()
    {
        var children = GetComponentsInChildren<EnemyRotationController>(true);

        float step = 0;

        while(step <= 1)
        {
            Vector2 size = Vector2.Lerp(Vector2.zero, Vector2.one, step);

            foreach(EnemyRotationController child in children)
            {
                child.transform.localScale = size;
            }

            step += 0.01f;

            yield return new WaitForSecondsRealtime(0.01f);
        }

    }

    private IEnumerator ManageStates()
    {
        FormationState state;

        while(true)
        {
            state = GetNextState();
            float duration = state.GetDuration();
            state.Enter();

            yield return new WaitForSecondsRealtime(duration);

            state.Exit();
        }
    }

    private FormationState GetNextState()
    {
        activeState++;

        if(activeState == states.Count) activeState = 0;

        return states[activeState];
    }
}
