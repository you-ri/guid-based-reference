using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class CopyCrossSceneTransformFromEntity : MonoBehaviour
{
    public GuidReference reference;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var entityManger = World.DefaultGameObjectInjectionWorld.EntityManager;
        var entity = EntityGuidManager.Resolve (reference);
        if (entity != default(Entity)) {
            //var data = entityManger.GetComponentData<InputController> (entity);
            var localToWorld = entityManger.GetComponentData<LocalToWorld> (entity);
            this.transform.position = localToWorld.Position;
            this.transform.rotation = new quaternion (localToWorld.Value);
        }

    }
}
