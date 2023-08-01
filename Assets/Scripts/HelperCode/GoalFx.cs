using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GoalFx : MonoBehaviour
{
    [SerializeField, ColorUsage(true,true)] Color GoalColor;
    [SerializeField, ColorUsage(true, true)] Color OrdinaryColor;
    [SerializeField] float timeScale = 1.0f;
    [SerializeField] AnimationCurve GoalEventCurve;
    [SerializeField] MeshRenderer goalMeshRenderer;
    [SerializeField] float LocalTimer;
    Material goalMaterial;
    // Start is called before the first frame update
    void Start()
    {
        goalMaterial = goalMeshRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGoalAnimation()
    {
        StartCoroutine(GoalAnimation());
    }

    IEnumerator GoalAnimation()
    {
        LocalTimer = timeScale;
        while(LocalTimer >= 0)
        {
            LocalTimer -= Time.deltaTime / timeScale;
            yield return new WaitForEndOfFrame();
            goalMaterial.SetColor("_EmissionColor", Color.Lerp(OrdinaryColor, GoalColor, GoalEventCurve.Evaluate((timeScale - LocalTimer )/ timeScale )));
        }
        goalMaterial.SetColor("_EmissionColor", OrdinaryColor);
        yield return null;
    }
}
