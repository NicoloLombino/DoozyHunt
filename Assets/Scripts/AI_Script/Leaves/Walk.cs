using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Walk : Leaf
{
    public async override Task<Outcome> Run(GameObject agent, Dictionary<string, object> blackboard)
    {
        float runTimer = Random.Range(0,
            agent.GetComponent<Enemy>().runToPlayerMaxTime);

        while (runTimer > 0)
        {
            runTimer -= Time.deltaTime;
            if (agent.GetComponent<Enemy>().isInMeleeDistance)
            {
                runTimer = -1;
                return Outcome.SUCCESS;
            }
        }
        await Task.Delay((int)((0.5f) * 1000));


        return Outcome.FAIL;
    }
}
