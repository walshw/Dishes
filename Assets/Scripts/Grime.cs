using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Grime : MonoBehaviour
{
    public float integrity = 100f;
    private Vector3 originalScale;

    public Action decrementCount;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = originalScale * (integrity / 100);
    }

    void OnTriggerStay(Collider other)
    {
        integrity -= 10f;
        if (integrity <= 0)
        {
            decrementCount();
            Destroy(gameObject);
        }
    }
}
