using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomAttack : AttackPatternTemplate
{
    [SerializeField] private float interval;

    public override event EventHandler OnSequenceEnd;

    public override IEnumerator Sequence(List<EnemyAttackController> attackers)
    {

        List<EnemyAttackController> _attackers = new List<EnemyAttackController>();
        EnemyAttackController[] array = new EnemyAttackController[attackers.Count];
        attackers.CopyTo(array);
        _attackers = array.ToList();

        //for (int i = 0; i < attackers.Count; i++)
        while(_attackers.Count > 0)
        {
            int rdm = UnityEngine.Random.Range(0, _attackers.Count);
            _attackers[rdm].Attack();
            _attackers.RemoveAt(rdm);
            yield return new WaitForSeconds(interval);
        }

        OnSequenceEnd?.Invoke(this, EventArgs.Empty);

        StopCoroutine("Sequence");
    }

}
