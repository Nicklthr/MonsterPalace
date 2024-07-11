using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityNavMeshAgent
{
    [TaskCategory("Unity/NavMeshAgent")]
    [TaskDescription("Has the agent arrived?")]
    public class HasArrived : Conditional
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;

        public SharedBool hasArrived = true;

        // cache the navmeshagent component
        private NavMeshAgent navMeshAgent;
        private GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject)
            {
                navMeshAgent = currentGameObject.GetComponent<NavMeshAgent>();
                prevGameObject = currentGameObject;
            }

        }

        public override TaskStatus OnUpdate()
        {
            if (navMeshAgent == null)
            {
                Debug.LogWarning("NavMeshAgent is null");
                return TaskStatus.Failure;
            }

            if (hasArrived.Value)
            {
                return navMeshAgent.remainingDistance == 0 ? TaskStatus.Success : TaskStatus.Failure;
            }
            else
            {
                return navMeshAgent.remainingDistance == 0 ? TaskStatus.Failure : TaskStatus.Success;
            }
           
        }

        public override void OnReset()
        {
            targetGameObject = null;
        }
    }
}
