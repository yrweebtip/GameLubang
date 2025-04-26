using UnityEngine;

public class HoleController : MonoBehaviour
{
    private Camera cam;
    private bool isDragging = false;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            MoveWithMouse();
        }
    }

    void MoveWithMouse()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Ground")))
        {
            Vector3 targetPos = hit.point;
            targetPos.y = transform.position.y; // Tetap di level yang sama (tidak lompat)
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 10f);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == this.gameObject)
        {
            // Kalau collider ini diri sendiri (hole), abaikan
            return;
        }

        Debug.Log("Terdeteksi collider: " + other.name);

        SuckedObject suck = other.GetComponent<SuckedObject>();
        if (suck == null)
        {
            Debug.LogWarning("Objek ini tidak punya script SuckedObject: " + other.name);
        }
        else
        {
            Debug.Log("Memulai hisapan pada: " + other.name);
            suck.StartSuck(transform);
        }
    }

}
