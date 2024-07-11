using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.MonsterTask
{
    [TaskCategory("Monster")]
    [TaskDescription("Search for eating")]
    public class SearchEating : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;

        // cache the navmeshagent component
        private MonsterController controller;

        public override void OnStart()
        {

            controller = GetDefaultGameObject(targetGameObject.Value).GetComponent<MonsterController>();
        }

        public override TaskStatus OnUpdate()
        {


            bool value = controller.searchEating();

            return value ? TaskStatus.Success : TaskStatus.Failure;


        }

        public override void OnReset()
        {
            targetGameObject = null;
        }
    }



}
