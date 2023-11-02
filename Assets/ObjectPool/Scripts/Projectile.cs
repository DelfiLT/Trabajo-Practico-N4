using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [Header("Projectile Config")]
    public TurretAI.TurretType type = TurretAI.TurretType.Single;
    public Transform target;
    public ParticleSystem explosion;

    [Header("Projectile Stats")]
    [SerializeField] private float speed = 1;
    [SerializeField] private float turnSpeed = 1;
    [SerializeField] private float knockBack = 0.1f;
    [SerializeField] private float boomTimer = 1;
    [SerializeField] private bool catapult;

    private bool lockOn;

    private void Start()
    {
        CheckTurret(type);

        if (catapult)
        {
            lockOn = true;
        }

        if (type == TurretAI.TurretType.Single)
        {
            Vector3 direction = target.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void Update()
    {
        boomTimer -= Time.deltaTime;

        if (target == null || transform.position.y < -0.2F || boomTimer < 0)
        {
            Explosion();
            return;
        }
    }

    public void CheckTurret(TurretAI.TurretType type)
    {
        switch (type)
        {
            case TurretAI.TurretType.Single:
                float singleSpeed = speed * Time.deltaTime;

                transform.Translate(transform.forward * singleSpeed * 2, Space.World);
                break;
            case TurretAI.TurretType.Dual:
                Vector3 direction = target.position - transform.position;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, Time.deltaTime * turnSpeed, 0.0f);

                transform.Translate(Vector3.forward * Time.deltaTime * speed);
                transform.rotation = Quaternion.LookRotation(newDirection);
                break;
            case TurretAI.TurretType.Catapult:
                if (lockOn)
                {
                    Vector3 velocity = CalculateCatapult(target.transform.position, transform.position, 1);

                    transform.GetComponent<Rigidbody>().velocity = velocity;
                    lockOn = false;
                }
                break;
            default:
                break;
        }
    }

    Vector3 CalculateCatapult(Vector3 target, Vector3 origen, float time)
    {
        Vector3 distance = target - origen;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0;

        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Vector3 dir = other.transform.position - transform.position;
            Vector3 knockBackPos = other.transform.position + (dir.normalized * knockBack);

            knockBackPos.y = 1;
            other.transform.position = knockBackPos;

            Explosion();
        }
    }

    public void Explosion()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}