using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RunAwayFromFightLeaf : Leaf
{
    [SerializeField]
    private Transform runAwayPosition;
    public async override Task<Outcome> Run(GameObject agent, Dictionary<string, object> blackboard)
    {
        agent.GetComponent<Enemy>().RunAwayFromFight(runAwayPosition);
        await Task.Delay((int)((5) * 1000));
        agent.GetComponent<Enemy>().moneyToGive = 0;
        Destroy(agent);

        return Outcome.FAIL;
    }
}
