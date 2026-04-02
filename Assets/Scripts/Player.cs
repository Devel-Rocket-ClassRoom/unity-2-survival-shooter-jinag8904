using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    private int Health;
    public bool isDead;

    public UIManager uiManager;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        Health = 100;
        isDead = false;
    }

    public void OnDamage(int damage, Vector3 position, Vector3 normal)
    {
        Health -= damage;
        uiManager.SetSlider(Health);

        if (Health <= 0 && !isDead)
        {
            Health = 0;
            isDead = true;
            animator.SetTrigger("Die");
        }
    }
}