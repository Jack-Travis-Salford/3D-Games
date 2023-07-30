using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBrain : EnemyBrain
{
    private float approxSecUntilAttack;
    
    public RandomBrain(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        approxSecUntilAttack = Random.Range(0.5f, 4.0f);
    }
    public override bool ShouldAttack()
    {
        int upperBound = (int)(approxSecUntilAttack * (1 / Time.deltaTime));
        if (Random.Range(0,upperBound) == 0)
        {
            approxSecUntilAttack = Random.Range(0.5f, 4.0f);
            return true;
        }
        else
        {
            return false;
        }
    }

    
}
