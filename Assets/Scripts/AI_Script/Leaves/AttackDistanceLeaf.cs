

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class AttackDistanceLeaf : Leaf
{
    public async override Task<Outcome> Run(GameObject agent, Dictionary<string, object> blackboard)
    {
        if (agent.GetComponent<Enemy>().isFollowingPizza)
        {
            return Outcome.FAIL;
        }

        if (agent.GetComponent<Enemy>().isInThrowDistance)
        {
            agent.transform.LookAt(agent.GetComponent<Enemy>().playerTransform);
            agent.GetComponent<Animator>().SetTrigger("ThrowObject");
            agent.GetComponent<Enemy>().BlockMovement();
            await Task.Delay((int)((2) * 1000));
            return Outcome.FAIL;
        }
        else
        {
            return Outcome.SUCCESS;
        }
    }
}