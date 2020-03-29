using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;


public struct GuidComponentState: ISystemStateComponentData { }

public struct GuidComponentData : IComponentData
{
    public System.Guid Value;
}


public class GuidReferenceSystem : JobComponentSystem
{
    EntityQuery onStartQuery;

    protected override JobHandle OnUpdate (JobHandle inputDeps)
    {
        Entities
            .WithoutBurst ()
            .WithNone<GuidComponentState> ()
            .ForEach ((Entity entity, ref GuidComponentData data) => {
                EntityGuidManager.Add (entity, data);
            })
            .WithStoreEntityQueryInField (ref onStartQuery)
            .Run ();

        EntityManager.AddComponent<GuidComponentState> (onStartQuery);

        return inputDeps;
    }


}