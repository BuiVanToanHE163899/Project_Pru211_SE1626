using TMPro;
using UnityEngine;

[RequireComponent(typeof(TakeDamage))]
[RequireComponent(typeof(DestroyManager))]
public class Health : MonoBehaviour
{
    public enum ObjectType
    {
        Player,
        Enemy
    }
    [SerializeField] private bool createHealth = false;
	[SerializeField] private FloatVariable health;
	[SerializeField] private bool resetHealth;
	[SerializeField] private FloatReference startingHealth;
	[SerializeField] private FloatReference maxHealth;
    [SerializeField] private FloatReference kill;

    [SerializeField] private TMP_Text killsText;
	private int kills = 0;
	public ObjectType objectType;
    public event System.Action<int> OnKillsChanged;


    void Awake()
	{
		if (createHealth)
		{
			health = ScriptableObject.CreateInstance<FloatVariable>();
		}
		if (resetHealth)
		{
			health.value = startingHealth.value;
		}
		if (maxHealth != null)
		{
			maxHealth.value = startingHealth.value;
		}
	}
    

    public void ReduceHealth(float damage)
	{
		health.value -= damage;
		if (health.value < 0)
		{
			die();
		}
	}

	public void AddMaxHealth(float health)
	{
		maxHealth.value += health;
	}

	public FloatReference GetMaxHealth()
	{
		return maxHealth;
	}

	public void SetMaxHealth(float newValue)
	{
		maxHealth.value = newValue;
	}

	private void die()
	{
        if (objectType == ObjectType.Enemy)
        {
            kills++;
           OnKillsChanged?.Invoke(kills);
        }
		
		kill.value += kills;
		Debug.Log(kill.value);
        Destroy(gameObject);
	}

   
}


