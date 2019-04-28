using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class CollisionHighlights : MonoBehaviour
{
    MeshRenderer rend;
    MaterialPropertyBlock prop;

    public bool leaveHighlighted = false;
    public Color initialColor = Color.black;

    Coroutine activeCTN;

    private void Awake()
    {
        rend = GetComponent<MeshRenderer>();
        prop = new MaterialPropertyBlock();
        rend.SetPropertyBlock(prop);
    }

    IEnumerator HighlightEnumerator(float target, float duration)
    {
        target = Mathf.Clamp(target, 0, 1);
        float total = duration;
        var eof = new WaitForEndOfFrame();
        while (initialColor.a != target)
        {
            if (target == 0)
            {
                initialColor.a = (total / duration);
            } else
            {
                initialColor.a = 1 - (total / duration);
            }
            initialColor.a = Mathf.Clamp(initialColor.a, 0, 1);
            prop.SetColor("_Color", initialColor);
            total -= Time.deltaTime;
            rend.SetPropertyBlock(prop);
            yield return eof;
        }
    }

    void StartHighlight()
    {
        if (activeCTN != null)
        {
            StopCoroutine(activeCTN);
        }
        activeCTN = StartCoroutine(HighlightEnumerator(0, .2f));
    }

    void EndHighLight()
    {
        if (activeCTN != null)
        {
            StopCoroutine(activeCTN);
        }
        activeCTN = StartCoroutine(HighlightEnumerator(1, .2f));
    }

    int totalCollides = 0;

    private void OnCollisionEnter(Collision collision)
    {
        totalCollides++;
        if (totalCollides == 1)
        {
            StartHighlight();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        totalCollides--;
        if (totalCollides == 0 && !leaveHighlighted)
        {
            EndHighLight();
        }
    }
}
