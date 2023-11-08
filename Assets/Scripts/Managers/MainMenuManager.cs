using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
	private void Start()
	{
		GameManager.s_instance.changeGameSate(GameState.MainMenu);
	}

	public void playGame()
	{
		GameManager.s_instance.changeGameSate(GameState.LoadLevel);
		//SceneManager.LoadScene("SampleScene");
		SceneManager.LoadScene("Map");
	}
    public void playGameMap1()
    {
        GameManager.s_instance.changeGameSate(GameState.Playing);
        //SceneManager.LoadScene("SampleScene");
        SceneManager.LoadScene("SampleScene");
    }
    //public void playGameMap2()
    //{
    //    GameManager.s_instance.changeGameSate(GameState.Playing);
    //    //SceneManager.LoadScene("SampleScene");
    //    SceneManager.LoadScene("Scene1");
    //}
}