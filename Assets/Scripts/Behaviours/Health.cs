using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

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
    [SerializeField] private float regenerationRate = 1f;
    [SerializeField] private float regenerationAmount = 1f;

    private EnemySpawner enemySpawner;

    void Awake()
	{
		if (createHealth)
		{
			health = ScriptableObject.CreateInstance<FloatVariable>();
          
        }
		if (resetHealth&& health != null)
		{
			health.value = startingHealth.value;
		}
		if (maxHealth != null)
		{
			maxHealth.value = startingHealth.value;
		}

        //if (objectType == ObjectType.Player)
        //{
        //    StartCoroutine(RegenerateHealth());
        //}
        //if (objectType == ObjectType.Enemy)
        //{

        //    StartCoroutine(IncreaseHealthAfterDelay(5 * 60, 1.5f));

           
        //    StartCoroutine(IncreaseHealthAfterDelay(10 * 60, 2.0f));
        //}

    }

    private IEnumerator RegenerateHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / regenerationRate);
            health.value = Mathf.Min(health.value + regenerationAmount, maxHealth.value); // Hồi máu 1 đơn vị mỗi giây, giới hạn không vượt quá maxHealth
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
            OnKillsChanged?.Invoke(kills);
        }
        kill.value ++;
        Debug.Log(kill.value);
        Destroy(gameObject);

	}

    private IEnumerator IncreaseHealthAfterDelay(float delayInSeconds, float increaseMultiplier)
    {
        yield return new WaitForSeconds(delayInSeconds);

        maxHealth.value *= increaseMultiplier;

    }
}


