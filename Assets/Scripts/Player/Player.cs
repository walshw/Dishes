using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform head;
    public float moveSpeed = 5f;
    public float turnSpeed = 180f;
    private Camera cam;
    private CharacterController characterController;

    public bool frozen;

    void Start()
    {
        cam = Camera.main;
        characterController = GetComponent<CharacterController>();
        Physics.IgnoreLayerCollision(6, 7);
    }

    void Update()
    {
        if (frozen)
        {
            // 🥶
            return;
        }

        transform.Rotate(0, Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime, 0);
        head.transform.Rotate(-Input.GetAxis("Mouse Y") * turnSpeed * Time.deltaTime, 0f, 0f);

        Vector3 direction = moveSpeed * Input.GetAxis("Vertical") * transform.forward;
        characterController.Move(direction * Time.deltaTime);
        characterController.Move(Input.GetAxis("Horizontal") * moveSpeed * 0.5f * Time.deltaTime * transform.right);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.width / 2, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, 3f, LayerMask.GetMask("Usage")))
            {
                Washing w = hit.transform.gameObject.GetComponentInParent<Washing>();
                if (w != null)
                {
                    w.BeginWashing(this);
                    frozen = true;
                }
            }
        }
    }
}
