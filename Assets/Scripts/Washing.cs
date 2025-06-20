using UnityEngine;

public class Washing : MonoBehaviour
{
    private bool playing = false;
    public Transform dish;
    public Transform sponge;
    public Transform playerStandsHere;
    public GameObject grimePrefab;
    public MeshCollider dishModelCollider;
    public float spongeHoverOffset = 0.1f;
    public int grimeToSpawn = 100;
    public float spongeCleaningMoveSpeed = 0.02f;
    private int grimeCount;
    private Player player;
    private Vector3 spongeDefaultPosition;
    private Quaternion spongeDefaultRotation;
    private bool grimeSpawned;
    private bool spunchPickedUp;
    private float timeHeld;
    private Vector3 lastMousePosition;
    public float smoothTime = 0.3f;
    public float lookSpeed = 5f;

    void Start()
    {
        spongeDefaultPosition = sponge.transform.position;
        spongeDefaultRotation = sponge.transform.rotation;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!playing)
        {
            return;
        }

        if (!spunchPickedUp)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), 10f, LayerMask.GetMask("Sponge")) && Input.GetMouseButtonDown(0))
            {
                spunchPickedUp = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                lastMousePosition = new Vector3(Screen.width / 2, Screen.height / 2);
                sponge.rotation = dish.rotation;
            }
            return;
        }

        Vector3 mousePosition = new Vector3(lastMousePosition.x + Input.GetAxis("Mouse X") * lookSpeed, lastMousePosition.y + Input.GetAxis("Mouse Y") * lookSpeed, 0f);
        mousePosition.x = Mathf.Clamp(mousePosition.x, 0, Screen.width);
        mousePosition.y = Mathf.Clamp(mousePosition.y, 0, Screen.height);
        lastMousePosition = mousePosition;

        if (Input.GetMouseButton(0))
        {
            timeHeld = Mathf.Clamp01(timeHeld + Time.deltaTime);
            Vector3 newPosition = new Vector3(sponge.position.x + Input.GetAxis("Mouse X") * 0.01f, sponge.position.y + Input.GetAxis("Mouse Y") * 0.01f, sponge.position.z);
            Vector3 offset = newPosition - dish.position;
            sponge.position = dish.position + Vector3.ClampMagnitude(offset, dishModelCollider.bounds.extents.x);
            sponge.position = Vector3.Lerp(sponge.position, dishModelCollider.ClosestPoint(sponge.position), timeHeld / 1f);
            return;
        }

        timeHeld = Mathf.Clamp01(timeHeld - Time.deltaTime);
        sponge.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 1f));
    }

    public void BeginWashing(Player p)
    {
        player = p;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        p.transform.SetPositionAndRotation(playerStandsHere.position, Quaternion.LookRotation(-transform.right));
        p.head.transform.rotation = Quaternion.Euler(26f, 0f, 0f);

        if (!grimeSpawned)
        {
            for (int i = 0; i < grimeToSpawn; i++)
            {
                GameObject grimule = Instantiate(grimePrefab, dish);
                float radius = dishModelCollider.bounds.extents.x;
                float randX = Random.Range(-dishModelCollider.bounds.extents.x, dishModelCollider.bounds.extents.x);
                float zRange = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(randX, 2));
                float randZ = Random.Range(-zRange, zRange);
                grimule.transform.localPosition = new Vector3(randX, 0.02f, randZ);

                Grime g = grimule.GetComponent<Grime>();
                g.decrementCount = DecrementGrime;
            }
            grimeCount = grimeToSpawn;
            grimeSpawned = true;
        }

        playing = true;
    }

    public void StopWashing()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playing = false;
        player.washingDishes = null;
        spunchPickedUp = false;
        sponge.SetPositionAndRotation(spongeDefaultPosition, spongeDefaultRotation);
    }

    private void DecrementGrime()
    {
        if (--grimeCount == 0)
        {
            grimeSpawned = false;
            StopWashing();
        }
    }
}
