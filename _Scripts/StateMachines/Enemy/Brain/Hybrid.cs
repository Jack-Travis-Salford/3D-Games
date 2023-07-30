using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hybrid : EnemyBrain
{
    public Hybrid(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override bool ShouldAttack()
    {
        if (!stateMachine.Render.isVisible)
        {
            return true;
        }
        int upperBound = (int)(3 * (1 / Time.deltaTime));
        if (Random.Range(0,upperBound) == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
