using System.Collections;
using UnityEngine;

public class SuckedObject : MonoBehaviour
{
    private Transform target;
    private float suckSpeed = 5f;
    private float shrinkSpeed = 2f;
    private bool isSucked = false;

    public void StartSuck(Transform holeTransform)
    {
        target = holeTransform;
        GetComponent<Collider>().enabled = false;
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }
        StartCoroutine(SuckRoutine());
    }

    IEnumerator SuckRoutine()
    {
        float duration = 1.5f;
        float timer = 0f;

        Vector3 startPos = transform.position;
        Vector3 startScale = transform.localScale;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            transform.position = Vector3.Lerp(startPos, target.position, t);
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);

            yield return null;
        }

        Destroy(gameObject);
    }


    void Update()
    {
        if (!isSucked) return;

        // Bergerak ke lubang
        transform.position = Vector3.MoveTowards(transform.position, target.position, suckSpeed * Time.deltaTime);
        // Mengecil
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, shrinkSpeed * Time.deltaTime);

        // Hancurkan jika sudah kecil
        if (transform.localScale.magnitude < 0.05f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Terdeteksi collider: " + other.name);

        var suck = other.GetComponent<SuckedObject>();
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
