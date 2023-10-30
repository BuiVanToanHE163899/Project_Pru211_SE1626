using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	[SerializeField] private float startTime;
	[SerializeField] private FloatReference timer;
	[SerializeField] private GameEvent endGameEvent;
    private bool isPaused = false;


    private void Start()
	{
		timer.value = startTime;
	}

	private void Update()
	{
		timer.value += Time.deltaTime;
		if (timer.value >= 900)
		{
			endGame();
		}
	}
    public void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
        isPaused = false;
    }
    public void goToMenu()
	{
		SceneManager.LoadScene("MainMenu");
		Time.timeScale = 1;
		GameManager.s_instance.changeGameSate(GameState.MainMenu);
	}

	public void reloadGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		Time.timeScale = 1;
	}

	private void endGame()
	{
		endGameEvent.Raise();
		Time.timeScale = 0;
	}
}
