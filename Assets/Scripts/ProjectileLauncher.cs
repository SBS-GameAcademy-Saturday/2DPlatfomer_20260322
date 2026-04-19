using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileLauncher : MonoBehaviour
{
    public Projectile projectilePrefab;
    public Transform lauchPoint;
    public float returnProjectileTime = 5.0f;

    private ObjectPool<Projectile> _projectilePool;
    private Vector3 originScale;

    private void Start()
    {
        _projectilePool = new ObjectPool<Projectile>(
            createFunc: CreateProjectile,
            actionOnGet: ActiveProjectile,
            actionOnRelease: DeactiveProjectile,
            actionOnDestroy: DestoryProjectile,
            defaultCapacity: 5,
            maxSize: 50
        );
    }

    private Projectile CreateProjectile() 
    {
        Projectile projectile = Instantiate<Projectile>(projectilePrefab);
        originScale = projectile.transform.localScale;
        return projectile;
    }
    
    private void ActiveProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
        projectile.transform.position = lauchPoint.position;
        projectile.transform.rotation = projectilePrefab.transform.rotation;
    }

    private void DeactiveProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    private void DestoryProjectile(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }


    public void FireProjectile()
    {
        Projectile projectile = _projectilePool.Get();
        float value = transform.localScale.x > 0 ? 1 : -1;
        projectile.transform.localScale = new Vector3
        {
            x = originScale.x * value,
            y = originScale.y * value,
            z = originScale.z,
        };
        projectile.SetVelocity();

        StartCoroutine(ReturnProjectile(projectile, returnProjectileTime));
    }

    // ÄÚ·çÆ¾
    private IEnumerator ReturnProjectile(Projectile projectile, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _projectilePool.Release(projectile);
    }
}
