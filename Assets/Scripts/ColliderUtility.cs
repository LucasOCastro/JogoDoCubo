
using UnityEngine;

public static class ColliderUtility
{
    public static HealthManager HealthFromCollider(this Collider col)
    {
        if (col.TryGetComponent(out HealthManager health))
        {
            return health;
        }
        if (col.attachedRigidbody != null && col.attachedRigidbody.TryGetComponent(out health))
        {
            return health;
        }
        return null;
    }
}
