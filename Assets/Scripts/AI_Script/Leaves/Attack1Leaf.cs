using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Attack1Leaf : Leaf
{
    public async override Task<Outcome> Run(GameObject agent, Dictionary<string, object> blackboard)
    {
        if (agent.GetComponent<Enemy>().isFollowingPizza)
        {
            return Outcome.FAIL;
        }

            Debug.Log("PUNCH");
        if (agent.GetComponent<Enemy>().isInMeleeDistance)
        {
            Debug.Log("PUNCH ENTER");
            agent.GetComponent<Animator>().SetTrigger("Punch");
            agent.GetComponent<Enemy>().BlockMovement();
            await Task.Delay((int)((2) * 1000));
            Debug.Log("PUNCH STOP");

            return Outcome.SUCCESS;
        }
        else
        {
            return Outcome.FAIL;
        }
    }
}
