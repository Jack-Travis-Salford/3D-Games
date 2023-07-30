using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aggressive : EnemyBrain
{
    public Aggressive(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override bool ShouldAttack()
    {
        return true;
    }
}
