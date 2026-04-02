using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public ParticleSystem gunParticle;
    public LineRenderer bulletLineEffect;

    public Transform fireTransform;

    public LayerMask targetLayer;

    public int damage = 25;

    private Coroutine coShoot;

    public Player player;

    private void Awake()
    {
        bulletLineEffect.positionCount = 2;
        bulletLineEffect.enabled = false;
    }

    public void Shoot()
    {
        Vector3 hitPosition = new();
        Ray ray = new(fireTransform.position, player.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hitEnemy, 100, targetLayer))
        {
            hitPosition = hitEnemy.point;
            var target = hitEnemy.collider.gameObject.GetComponent<IDamageable>();

            if (target != null)
            {
                target.OnDamage(damage, hitEnemy.point, hitEnemy.normal);
            }
        }

        else
        {
            hitPosition = fireTransform.position + player.transform.forward * 100;
        }

        gunParticle.Play();

        if (coShoot != null)
        {
            StopCoroutine(coShoot);
            coShoot = null;
        }

        coShoot = StartCoroutine(CoShoot(hitPosition));
    }

    private IEnumerator CoShoot(Vector3 hitPosition)
    {
        bulletLineEffect.SetPosition(0, fireTransform.position);
        bulletLineEffect.SetPosition(1, hitPosition);
        bulletLineEffect.enabled = true;

        yield return new WaitForSeconds(0.03f);

        bulletLineEffect.enabled = false;

        coShoot = null;
    }
}
