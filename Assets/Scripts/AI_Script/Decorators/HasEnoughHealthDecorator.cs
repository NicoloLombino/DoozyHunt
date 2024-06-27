using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


[DisallowMultipleComponent]
public class HasEnoughHealthDecorator : Decorator
{

    public override async Task<Outcome> Run(GameObject agent, Dictionary<string, object> blackboard)
    {
        if (agent.GetComponent<Enemy>().currentHealth > agent.GetComponent<Enemy>().healthToRunAway)
        {
            return Outcome.SUCCESS;
        }

        return await child.Run(agent, blackboard);
    }
}