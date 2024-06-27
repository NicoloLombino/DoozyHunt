using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[DisallowMultipleComponent]
public class RandomMovement : Composite
{
    public override async Task<Outcome> Run(GameObject agent, Dictionary<string, object> blackboard)
    {
        int child = 1;
        if(agent.GetComponent<Enemy>().currentHealth <= agent.GetComponent<Enemy>().healthToRunAway)
        {
            int rnd = Random.Range(0, 10);
            if (rnd < agent.GetComponent<Enemy>().percentageToRunAwayFromFight)
            {
                // run
                child = 0;
            }
            else
            {
                // fight
                child = 1;
            }
        }
        return await children[child].Run(agent, blackboard);
    }
}
