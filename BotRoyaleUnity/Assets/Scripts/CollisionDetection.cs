using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private List<ContactPoint> CachedContactPoints = new List<ContactPoint>();

    void OnCollisionEnter(Collision collision)
    {
        collision.GetContacts(CachedContactPoints);
        foreach (var contact in CachedContactPoints)
        {
            Collider thisCollider = contact.thisCollider;
            Collider otherCollider = contact.otherCollider;

            // calculate and inflict damage if collided with another robot part
            if (otherCollider != null && otherCollider.GetComponent<PartHealth>() != null &&
                thisCollider != null && thisCollider.GetComponent<PartHealth>() != null)
            {
                string thisPartType = thisCollider.tag;
                string otherPartType = otherCollider.tag;

                float thisVelocity = GetComponent<Rigidbody>().GetPointVelocity(contact.point).sqrMagnitude;
                float otherVelocity = otherCollider.GetComponentInParent<Rigidbody>().GetPointVelocity(contact.point).sqrMagnitude;

                // only take damage if this part is moving slower than the other part
                if (thisVelocity < otherVelocity)
                {
                    float damage = DamageCalculator.DamageToInflictOnCollision(thisPartType, otherPartType);
                    bool killed = thisCollider.GetComponent<PartHealth>().SubtractHealth(damage);
                    if(killed){
                        otherCollider.GetComponentInParent<StatsTracker>().IncrementKills();
                    }
                    otherCollider.GetComponentInParent<StatsTracker>().AddDamageDealt(damage);
                }
            }
        }
    }
}
