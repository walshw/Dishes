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

    public Washing washingDishes;

    void Start()
    {
        cam = Camera.main;
        characterController = GetComponent<CharacterController>();
        Physics.IgnoreLayerCollision(6, 7);
    }

    void Update()
    {
        if (washingDishes)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                washingDishes.StopWashing();
                washingDishes = null;
            }
            return;
        }

        transform.Rotate(0, Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime, 0);
        head.transform.Rotate(-Input.GetAxis("Mouse Y") * turnSpeed * Time.deltaTime, 0f, 0f);

        Vector3 zDirection = moveSpeed * Input.GetAxis("Vertical") * transform.forward;
        Vector3 xDirection = Input.GetAxis("Horizontal") * moveSpeed * 0.5f * transform.right;
        Vector3 dir = zDirection + xDirection;
        dir.y -= 9.8f;
        characterController.Move(dir * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, 3f, LayerMask.GetMask("Usage")))
            {
                washingDishes = hit.transform.gameObject.GetComponentInParent<Washing>();
                if (washingDishes != null)
                {
                    washingDishes.BeginWashing(this);
                }
            }
        }
    }
}
