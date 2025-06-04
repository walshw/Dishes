using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class FadeFromSight : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    public Transform playerTransform;
    public AnimationCurve curve;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        Color c = meshRenderer.material.color;
        Vector3 playerToMonster = transform.position - playerTransform.position;
        float angleRatio = Mathf.Clamp(Vector3.Angle(playerToMonster.normalized, Camera.main.transform.forward), 0, 90) / 90f;
        meshRenderer.material.color = new Color(c.a, c.g, c.b, curve.Evaluate(angleRatio));
    }
}
