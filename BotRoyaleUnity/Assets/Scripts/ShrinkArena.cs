using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ShrinkArena : MonoBehaviour
{
    [Header("Grow animation")]
    [SerializeField] private float GrowTime = 0.5f;
    [SerializeField] private AnimationCurve GrowCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("Sequence timings")]
    [Tooltip("Time before arena starts shrinking")]
    [SerializeField] private float ShrinkSequenceDelay = 5f;
    [Tooltip("Time it takes for arena to shrink to minimum size")]
    [SerializeField] private float ShrinkSequenceLength = 45f;
    [SerializeField] private AnimationCurve ShrinkCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [Tooltip("Time it takes for walls to collapse after arena is min size")]
    [SerializeField] private float WallCollapseLength = 5f;
    [SerializeField] private AnimationCurve WallCollapseCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [Header("Ending sizes")]
    [SerializeField] private float MinimumScale = 0.5f;
    [SerializeField] private float MinimumWallHeight = 0.01f;

    [Header("Required references")]
    [SerializeField] private Transform Walls = default;

    private float initialWallHeight;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(Walls, "Walls reference is required");
        initialWallHeight = Walls.localScale.y;
    }

    public IEnumerator GrowToSize(float size)
    {
        StopAllCoroutines();
        float elapsedTime = 0f;
        Vector3 initialScale = transform.localScale;
        Vector3 finalScale = new Vector3(size, 1f, size);
        while (elapsedTime < GrowTime)
        {
            transform.localScale = Vector3.LerpUnclamped(initialScale, finalScale, GrowCurve.Evaluate(elapsedTime / GrowTime));
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        transform.localScale = finalScale;
    }

    public void StartShrinkSequence()
    {
        StopAllCoroutines();
        StartCoroutine(ShrinkSequence());
    }

    private IEnumerator ShrinkSequence()
    {
        // put walls up
        Walls.localScale = new Vector3(Walls.localScale.x, initialWallHeight, Walls.localScale.z);

        // delay before shrink
        yield return new WaitForSeconds(ShrinkSequenceDelay);

        // shrink to minimum size
        float elapsedShrinkTime = 0f;
        Vector3 initialScale = transform.localScale;
        Vector3 finalScale = new Vector3(MinimumScale, 1f, MinimumScale);
        while (elapsedShrinkTime < ShrinkSequenceLength)
        {
            transform.localScale = Vector3.LerpUnclamped(initialScale, finalScale, ShrinkCurve.Evaluate(elapsedShrinkTime / ShrinkSequenceLength));
            yield return null;
            elapsedShrinkTime += Time.deltaTime;
        }
        transform.localScale = finalScale;

        // collapse walls
        float elapsedWallCollapseTime = 0f;
        float finalWallHeight = MinimumWallHeight;

        while(Walls.position.y > -2.9f){
            Walls.position = new Vector3(Walls.position.x, Walls.position.y - Time.deltaTime*0.5f, Walls.position.z);
            yield return null;
        }
        
        /*
        while (elapsedWallCollapseTime < WallCollapseLength)
        {
            float wallHeight = Mathf.LerpUnclamped(initialWallHeight, finalWallHeight, WallCollapseCurve.Evaluate(elapsedWallCollapseTime / WallCollapseLength));
            Walls.localScale = new Vector3(Walls.localScale.x, wallHeight, Walls.localScale.z);
            yield return null;
            elapsedWallCollapseTime += Time.deltaTime;
        }
        Walls.localScale = new Vector3(Walls.localScale.x, finalWallHeight, Walls.localScale.z);
        */

        while (transform.localScale.x > 1)
        {
            transform.localScale -= new Vector3(1f,0,1f) * Time.deltaTime;
            yield return null;
        }

        // sequence done.
    }

    public Vector3 GetLocalScale(){
        return transform.localScale;
    }
}
