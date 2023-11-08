using TMPro;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
	[SerializeField] private FloatReference currentLevel;

	[SerializeField] private FloatReference currentXP;
    [SerializeField] private FloatReference currentKills;

    [SerializeField] private FloatReference requiredXPForNextLevel;
	[SerializeField] private FloatReference requiredXPForNextLevelMultiplyer;
	[SerializeField] private GameEvent onXpPickup;
	[SerializeField] private GameEvent onLevelUp;
	[SerializeField] private AudioClip pickUpSound;
	[SerializeField] private TMP_Text levelText;
	[SerializeField] private TMP_Text killsText;
	[SerializeField] private TMP_Text coinText;

	private int coin;		
	private AudioSource m_audioSource;

	private void Start()
	{
		currentLevel.value = 1;
		currentKills.value = 0;
		currentXP.value = 0;
		requiredXPForNextLevel.value = 10;
		m_audioSource = gameObject.AddComponent<AudioSource>();
		m_audioSource.volume = 0.5f;
		m_audioSource.clip = pickUpSound;
		m_audioSource.playOnAwake = false;
		m_audioSource.loop = false;
     

    }
    private void Update()
    {
		UpdateKillsUI();
    }
    public void AddXP()
	{
		currentXP.value++;
		if (currentXP.value >= requiredXPForNextLevel.value)
		{
			LevelUp();
		}
	}

	
	private void LevelUp()
	{
		currentLevel.value++;
		currentXP.value = 0;
		requiredXPForNextLevel.value *= requiredXPForNextLevelMultiplyer.value;
		UpdateValuesOfLevel();

		onLevelUp.Raise();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("XP"))
		{
			AddXP();
			onXpPickup.Raise();
			m_audioSource.Play();
			Destroy(collision.gameObject);
		}
		if (collision.CompareTag("Coin"))
		{
			coin++;
			onXpPickup.Raise();
			m_audioSource.Play();

			Destroy(collision.gameObject);
			UpdateCoinsValues();

		}
	}
	private void UpdateValuesOfLevel()
	{
		levelText.text = currentLevel.value.ToString();
	}
    private void UpdateKillsValues(float newKills)
    {
        currentKills.value += newKills;
		Debug.Log(currentKills.value.ToString());

	}
  private void  UpdateKillsUI() => killsText.text = currentKills.value.ToString();
    private void UpdateCoinsValues() => coinText.text = coin.ToString();

}
