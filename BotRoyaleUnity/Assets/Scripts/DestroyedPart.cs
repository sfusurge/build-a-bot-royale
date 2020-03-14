using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedPart : MonoBehaviour
{
    [SerializeField] private float lifespan = 1.5f;
    [SerializeField] private float propForce = 10f;

    private List<Rigidbody> rbs;
    private Dictionary<Rigidbody, Vector3> propInitialScales;

    void Start()
    {
        rbs = new List<Rigidbody>();
        GetComponentsInChildren(rbs);

        propInitialScales = new Dictionary<Rigidbody, Vector3>();

        foreach (var rb in rbs)
        {
            propInitialScales.Add(rb, rb.transform.localScale);
            rb.AddExplosionForce(propForce, transform.position, 1f, 5f);
        }
        StartCoroutine(Shrink());
    }

    private IEnumerator Shrink()
    {
        float elapsedTime = 0f;
        while (elapsedTime < lifespan)
        {
            foreach (var rb in rbs)
            {
                rb.transform.localScale = Vector3.Lerp(propInitialScales[rb], Vector3.zero, elapsedTime / lifespan);
            }
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        Destroy(gameObject);
    }
}
