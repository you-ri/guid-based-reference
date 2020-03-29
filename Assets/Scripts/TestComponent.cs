using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

namespace Lilium.CrossSceneReference.Samples
{
    [GenerateAuthoringComponent]
    public class TestComponent : IComponentData
    {
        public int hoge;
    }

    public class TestComponentSystem : ComponentSystem
    {
        protected override void OnUpdate ()
        {
            Entities
                .ForEach ((TestComponent copyTransform, ref LocalToWorld localToWorld) => {
                    Debug.Log (copyTransform.hoge);
                });
        }
    }

}