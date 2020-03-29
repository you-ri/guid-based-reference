using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;


public class EntityGuidManager
{
    static EntityGuidManager instance;

    // instance data
    private Dictionary<System.Guid, Entity> guidToObjectMap = new Dictionary<Guid, Entity> ();

    public static bool Add (Entity entity, in GuidComponentData guidComponent)
    {
        if (instance == null) {
            instance = new EntityGuidManager ();
        }
        return instance.InternalAdd (entity, guidComponent);
    }


    public static Entity Resolve (GuidReference guidComponent)
    {
        if (instance == null) {
            instance = new EntityGuidManager ();
        }
        return instance.ResolveGuidInternal (guidComponent.guid);
    }



    private bool InternalAdd (Entity entity, in GuidComponentData guidComponent)
    {
        Guid guid = guidComponent.Value;

        if (!guidToObjectMap.ContainsKey (guid)) {
            guidToObjectMap.Add (guid, entity);
            return true;
        }

        //Entity existingInfo = guidToObjectMap[guid];
#if false
        if (existingInfo.go != null && existingInfo.go != guidComponent.gameObject) {
            // normally, a duplicate GUID is a big problem, means you won't necessarily be referencing what you expect
            if (Application.isPlaying) {
                Debug.AssertFormat (false, guidComponent, "Guid Collision Detected between {0} and {1}.\nAssigning new Guid. Consider tracking runtime instances using a direct reference or other method.", (guidToObjectMap[guid].go != null ? guidToObjectMap[guid].go.name : "NULL"), (guidComponent != null ? guidComponent.name : "NULL"));
            }
            else {
                // however, at editor time, copying an object with a GUID will duplicate the GUID resulting in a collision and repair.
                // we warn about this just for pedantry reasons, and so you can detect if you are unexpectedly copying these components
                Debug.LogWarningFormat (guidComponent, "Guid Collision Detected while creating {0}.\nAssigning new Guid.", (guidComponent != null ? guidComponent.name : "NULL"));
            }
            return false;
        }
#endif
        guidToObjectMap[guid] = entity;
        return true;
    }

    // nice easy api to find a GUID, and if it works, register an on destroy callback
    // this should be used to register functions to cleanup any data you cache on finding
    // your target. Otherwise, you might keep components in memory by referencing them
    private Entity ResolveGuidInternal (System.Guid guid)
    {
        Entity info;
        if (guidToObjectMap.TryGetValue (guid, out info)) {
            return info;
        }
        return default (Entity);
    }


    /// <summary>
    /// Support disabling reload domain. 
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    void Initialize ()
    {
        instance = null;
    }

}

