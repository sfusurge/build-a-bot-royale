using System;
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

                Vector3 thisVelocity = GetComponent<Rigidbody>().GetPointVelocity(contact.point);
                Vector3 otherVelocity = otherCollider.GetComponentInParent<Rigidbody>().GetPointVelocity(contact.point);
                Vector3 netVelocity = otherVelocity-thisVelocity;

                float damageBalance;
                // Only take half damage if velocity is greater than other persons velocity.
                if(thisVelocity.sqrMagnitude < otherVelocity.sqrMagnitude){
                    damageBalance = 1/150f;
                }else{
                    damageBalance = 1/300f;
                }

                float damage = DamageCalculator.DamageToInflictOnCollision(thisPartType, otherPartType);
                damage = damage * Math.Max(otherVelocity.magnitude * netVelocity.magnitude * damageBalance , 0.25f);
                float damageDone = Math.Min(thisCollider.GetComponent<PartHealth>().ReturnHealth(), damage);
                bool killed = thisCollider.GetComponent<PartHealth>().SubtractHealth(damage);
                if(killed){
                    otherCollider.GetComponentInParent<StatsTracker>().IncrementKills();
                }
                otherCollider.GetComponentInParent<StatsTracker>().AddDamageDealt(damageDone);
                thisCollider.GetComponentInParent<StatsTracker>().SetLastTouched(otherCollider.transform.parent.gameObject);
            }
        }
    }
}
