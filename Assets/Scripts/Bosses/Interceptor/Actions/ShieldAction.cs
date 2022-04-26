using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAction : BossAction
{
    [SerializeField] private GameObject shield;

    public override void Action()
    {

    }

    public override void DoActionMove()
    {

    }

    public override void EndAction()
    {

    }

    public override void StartAction()
    {
        // controller.ActivateAnimation("Special");
    }

    public void CreateShield()
    {
        var container = Instantiate(shield, transform.position, transform.rotation);
        container.SetActive(true);
        controller.StartCoroutine(ShieldFollow(container));
    }

    private IEnumerator ShieldFollow(GameObject shield)
    {
        while(shield.activeSelf)
        {
            shield.transform.position = transform.position;
            shield.transform.rotation = transform.rotation;
            yield return new WaitForEndOfFrame();
        }
    }
}
