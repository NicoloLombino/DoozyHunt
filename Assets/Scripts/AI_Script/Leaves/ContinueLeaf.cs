using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ContinueLeaf : Leaf
{
    public async override Task<Outcome> Run(GameObject agent, Dictionary<string, object> blackboard)
    {
        await Task.Delay((int)((0.5f) * 1000));

        return Outcome.SUCCESS;
    }
}
