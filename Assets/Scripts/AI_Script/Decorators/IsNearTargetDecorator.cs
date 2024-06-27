using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


[DisallowMultipleComponent]
public class IsNearTargetDecorator : Decorator
{
    public override async Task<Outcome> Run(GameObject agent, Dictionary<string, object> blackboard)
    {
        /*
        if (!agent.GetComponent<EnemyPriets>().isInThrowDistance)
        {
            return Outcome.FAIL;
        }

        return await child.Run(agent, blackboard);
        */

        if (agent.GetComponent<Enemy>().isInThrowDistance)
        {
            if(agent.GetComponent<Enemy>().isInMeleeDistance)
            {
                return Outcome.SUCCESS;
            }
            else
            {
                return await child.Run(agent, blackboard);
            }
        }

        return Outcome.FAIL;

    }
}