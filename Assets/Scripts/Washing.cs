using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Washing : MonoBehaviour
{
    public Transform dish;
    public Transform sponge;
    public GameObject grimePrefab;
    public MeshCollider dishModelCollider;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 1000; i++)
        {
            GameObject grimule = Instantiate(grimePrefab, dish);
            float radius = dishModelCollider.bounds.extents.x;
            float randX = Random.Range(-dishModelCollider.bounds.extents.x, dishModelCollider.bounds.extents.x);
            float zRange = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(randX, 2));
            float randZ = Random.Range(-zRange, zRange);
            grimule.transform.localPosition = new Vector3(randX, 0f, randZ);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // x 1. Sponge follows mouse
        // x 2. Sponge orients to dish on hover
        // x 3. Move over grime particles to destroy them
        Vector3 mousePosition = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(mousePosition.x, mousePosition.y, mousePosition.z));
        if (Physics.Raycast(ray, out RaycastHit hit, 10f, LayerMask.GetMask("Dish")))
        {
            sponge.SetPositionAndRotation(hit.point, hit.transform.rotation);
        }
        else
        {
            sponge.transform.LookAt(Camera.main.transform.position, Vector3.up);
            sponge.transform.Rotate(90f, 0f, 0f);
            sponge.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0.5f));
        }
    }
}
