using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

namespace Lilium.CrossSceneReference.Samples
{
    [GenerateAuthoringComponent]
    public class CopyCrossSceneTransformToGameObject : IComponentData
    {
        public GuidReference reference;
    }

    [ExecuteAlways]
    [UpdateInGroup (typeof (TransformSystemGroup))]
    [UpdateAfter (typeof (EndFrameLocalToParentSystem))]
    public class CopyCrossSceneTransformToGameObjecttSystem : ComponentSystem
    {
        protected override void OnUpdate ()
        {
            Entities
                .ForEach ((CopyCrossSceneTransformToGameObject copyTransform, ref LocalToWorld localToWorld) => {
                    Debug.Assert (copyTransform != null);

                    var target = GuidManager.Resolve(copyTransform.reference);
                    if (target == null) return;

                    target.transform.position = localToWorld.Position;
                    target.transform.rotation = localToWorld.Rotation;
                });
        }
    }

}