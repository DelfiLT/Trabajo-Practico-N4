using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [Header("Projectile Config")]
    public TurretAI.TurretType type = TurretAI.TurretType.Single;
    public Transform target;
    public ParticleSystem explosion;
    public int poolAmmount;

    [Header("Projectile Stats")]
    [SerializeField] private float speed = 1;
    [SerializeField] private float turnSpeed = 1;
    [SerializeField] private float knockBack = 0.1f;
    [SerializeField] private bool catapult;
    public float boomTimer;

    public bool lockOn;

    private void Start()
    {
        boomTimer = 0;

        if (catapult)
        {
            lockOn = true;
        }
    }

    private void Update()
    {
        if (type == TurretAI.TurretType.Single)
        {
            Vector3 direction = target.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        CheckTurret(type);

        if (target == null)
        {
            Explosion();
        }

        if (this.gameObject.activeInHierarchy)
        {
            boomTimer += Time.deltaTime;

            if(boomTimer > 5)
            {
                boomTimer = 0;
                Explosion();
            }
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
            Explosion();
        }
    }

    public void Explosion()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        this.gameObject.SetActive(false);
        lockOn = true;
    }
}