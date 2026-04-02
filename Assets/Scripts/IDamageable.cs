using UnityEngine;

public interface IDamageable
{
    public void OnDamage(int damage, Vector3 position, Vector3 normal);
}