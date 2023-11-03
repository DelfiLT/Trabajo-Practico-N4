using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TurretAI : MonoBehaviour {

    public enum TurretType
    {
        Single = 1,
        Dual = 2,
        Catapult = 3,
    }

    [Header("Turret Config")]
    public TurretType turretType = TurretType.Single;

    [SerializeField] private Transform turreyHead;
    [SerializeField] private Transform muzzleMain;
    [SerializeField] private Transform muzzleSub;
    [SerializeField] private GameObject muzzleEff;
    [SerializeField] private GameObject bullet;

    private GameObject currentTarget;

    [Header("Turret Stats")]
    [SerializeField] private float attackDist;
    [SerializeField] private float shootCoolDown;
    [SerializeField] private float loockSpeed;

    private bool shootLeft = true;
    private Vector3 randomRot;
    private float timer;
    private Animator animator;
    private Transform lockOnPos;


    void Start () 
    {
        InvokeRepeating("ChackForTarget", 0, 0.5f);

        if (transform.GetChild(0).GetComponent<Animator>())
        {
            animator = transform.GetChild(0).GetComponent<Animator>();
        }

        randomRot = new Vector3(0, Random.Range(0, 359), 0);
    }
	
	void Update () 
    {
        if (currentTarget != null)
        {
            FollowTarget();

            float currentTargetDist = Vector3.Distance(transform.position, currentTarget.transform.position);
            if (currentTargetDist > attackDist)
            {
                currentTarget = null;
            }
        }
        else
        {
            IdleRotate();
        }

        timer += Time.deltaTime;
        if (timer >= shootCoolDown)
        {
            if (currentTarget != null)
            {
                timer = 0;
                
                if (animator != null)
                {
                    animator.SetTrigger("Fire");
                    Shoot(turretType);
                }
                else
                {
                    Shoot(turretType);
                }
            }
        }
	}

    private void ChackForTarget()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, attackDist);
        float distAway = Mathf.Infinity;

        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].tag == "Player")
            {
                float dist = Vector3.Distance(transform.position, colls[i].transform.position);
                if (dist < distAway)
                {
                    currentTarget = colls[i].gameObject;
                    distAway = dist;
                }
            }
        }
    }

    private void FollowTarget()
    {
        Vector3 targetDir = currentTarget.transform.position - turreyHead.position;
        targetDir.y = 0;

        if (turretType == TurretType.Single)
        {
            turreyHead.forward = targetDir;
        }
        else
        {
            turreyHead.transform.rotation = Quaternion.RotateTowards(turreyHead.rotation, Quaternion.LookRotation(targetDir), loockSpeed * Time.deltaTime);
        }
    }

    public void Shoot(TurretType type)
    {
        switch (type)
        {
            case TurretType.Catapult:
                SpawnBullet(muzzleMain);
                break;
            case TurretType.Dual:
                if (shootLeft)
                {
                    SpawnBullet(muzzleMain);
                }
                else
                {
                    SpawnBullet(muzzleSub);
                }
                shootLeft = !shootLeft;
                break;
            case TurretType.Single:
                SpawnBullet(muzzleMain);
                break;
        }
    }

    private void SpawnBullet(Transform spawn)
    {
        Instantiate(muzzleEff, spawn.transform.position, spawn.rotation);

        bullet = ObjectPool.SharedInstance.GetPooledObject(turretType.ToString());

        if (bullet != null)
        {
            bullet.transform.position = spawn.position;
            bullet.transform.rotation = spawn.rotation;
            bullet.SetActive(true);
            Debug.Log(bullet.ToString());
        }

        Projectile projectile = bullet.GetComponent<Projectile>();
        projectile.target = currentTarget.transform;
    }

    public void IdleRotate()
    {
        bool refreshRandom = false;
        
        if (turreyHead.rotation != Quaternion.Euler(randomRot))
        {
            turreyHead.rotation = Quaternion.RotateTowards(turreyHead.transform.rotation, Quaternion.Euler(randomRot), loockSpeed * Time.deltaTime * 0.2f);
        }
        else
        {
            refreshRandom = true;

            if (refreshRandom)
            {

                int randomAngle = Random.Range(0, 359);
                randomRot = new Vector3(0, randomAngle, 0);
                refreshRandom = false;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDist);
    }
}
