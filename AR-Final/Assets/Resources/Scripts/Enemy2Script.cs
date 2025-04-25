using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Script : EnemyScript
{
    public float VisibilityDuration = 3f; 
    private Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        StartCoroutine(ToggleVisibility());
    }

    IEnumerator ToggleVisibility()
    {
        while (true)
        {
            rend.enabled = !rend.enabled;
            yield return new WaitForSeconds(VisibilityDuration);
        }
    }
}
