using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBrain
{
    protected EnemyStateMachine stateMachine;

    public EnemyBrain(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    public abstract bool ShouldAttack();
}
