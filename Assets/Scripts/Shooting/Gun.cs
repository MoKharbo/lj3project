using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

//[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour
{
    [SerializeField] private Transform spawn;
    [SerializeField] private Transform ShellSpawn;
    [SerializeField] private Rigidbody shellPrefab;
    [SerializeField] private float rpm;
    [SerializeField] private int damage = 25;
    [SerializeField] private float trailDuration = 0.05f;
    [SerializeField] private float trailWidth = 0.04f;

    private float secondsBetweenShots;
    private float nextPossibleShootTime;

    private void Start()
    {
        secondsBetweenShots = 60f / rpm;
    }
    public void Shoot()
    {
        if (CanShoot())
        {

            Ray ray = new Ray(spawn.position, spawn.forward);
            RaycastHit hit;
            float shotDistance = 20f;
            if (Physics.Raycast(ray, out hit, shotDistance))
            {
                shotDistance = hit.distance;

                EnemyAi enemy = hit.collider.GetComponentInParent<EnemyAi>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }

            nextPossibleShootTime = Time.time + secondsBetweenShots;

            //GetComponent<AudioSource>().Play();
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * shotDistance, Color.red, 1f);
            StartCoroutine(ShowRaycastTrail(ray.origin, ray.origin + ray.direction * shotDistance));

            Rigidbody newShell = Instantiate(shellPrefab, ShellSpawn.position, Quaternion.identity) as Rigidbody;
            newShell.AddForce(ShellSpawn.forward * Random.Range(150f, 200f) + spawn.forward * Random.Range(-10f, 10f));
        }
    }

    private bool CanShoot()
    {
        bool canShoot = true;
        if (Time.time < nextPossibleShootTime)
        {
            canShoot = false;
        }

        return canShoot;
    }

    private IEnumerator ShowRaycastTrail(Vector3 start, Vector3 end)
    {
        GameObject trailObject = new GameObject("RaycastTrail");
        TrailRenderer trail = trailObject.AddComponent<TrailRenderer>();
        trail.time = trailDuration;
        trail.startWidth = trailWidth;
        trail.endWidth = 0f;
        trail.minVertexDistance = 0.01f;
        trail.material = new Material(Shader.Find("Sprites/Default"));
        trail.startColor = Color.red;
        trail.endColor = Color.clear;

        trailObject.transform.position = start;

        float elapsed = 0f;
        while (elapsed < trailDuration)
        {
            elapsed += Time.deltaTime;
            trailObject.transform.position = Vector3.Lerp(start, end, elapsed / trailDuration);
            yield return null;
        }

        trailObject.transform.position = end;
        yield return new WaitForSeconds(trailDuration);
        Destroy(trailObject);
    }
}
