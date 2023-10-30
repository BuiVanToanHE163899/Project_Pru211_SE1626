using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float movementSpeed = 5f;
	public PlayerDirection playerDirection;

	[SerializeField] private DynamicJoystick dynamicJoystick;
	[SerializeField] private GameEvent playerDead;
	private Formulas m_formulas;
	public Animator animator;



	// Start is called before the first frame update
	private void Start()
	{
		m_formulas = new Formulas();
		animator = GetComponent<Animator>();
#if UNITY_ANDROID
        dynamicJoystick.gameObject.SetActive(true);
#endif
	}

	// Update is called once per frame
	private void Update()
	{
#if UNITY_ANDROID
        joystickMovement();
#else

		keyboardMovement();

#endif
	}

	public void raisePlayerDead()
	{
		playerDead.Raise();
	}


	private void keyboardMovement()
	{
		float xMovement = Input.GetAxisRaw("Horizontal") * Time.deltaTime * movementSpeed;
		float yMovement = Input.GetAxisRaw("Vertical") * Time.deltaTime * movementSpeed;
		Vector3 movementVector = new Vector3(xMovement, yMovement, 0);
		transform.position = m_formulas.move(transform.position, movementVector);
		updatePlayerDirection(xMovement, yMovement);
		updatePlayerAnimator(xMovement, yMovement, movementVector.sqrMagnitude);

	}
	private void updatePlayerAnimator(float t_xMovement, float t_yMovement, float movementVector)
	{
		animator.SetFloat("Vertical", t_yMovement);
		animator.SetFloat("Horizontal", t_xMovement);
		animator.SetFloat("Speed", movementVector);
	}

	private void joystickMovement()
	{
		float xMovement = dynamicJoystick.Horizontal * Time.deltaTime * movementSpeed;
		float yMovement = dynamicJoystick.Vertical * Time.deltaTime * movementSpeed;
		Vector3 movementVector = new Vector3(xMovement, yMovement, 0);
		transform.position = m_formulas.move(transform.position, movementVector);
		updatePlayerDirection(xMovement, yMovement);
	}

	private void updatePlayerDirection(float t_xMovement, float t_yMovement)
	{
		switch (t_xMovement)
		{
			case < 0:
				playerDirection = PlayerDirection.West;
				transform.localScale = new Vector2(-1, 1);
				break;
			case > 0:
				playerDirection = PlayerDirection.East;
				transform.localScale = new Vector2(1, 1);
				break;
		}

		switch (t_yMovement)
		{
			case < 0:
				playerDirection = PlayerDirection.South;
				break;
			case > 0:
				playerDirection = PlayerDirection.North;
				break;
		}
	}
}

public enum PlayerDirection
{
	North,
	South,
	East,
	West
}