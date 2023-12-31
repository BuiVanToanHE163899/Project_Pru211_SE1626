using System;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerShoot : PlayerBaseAttack
{
	public int currentLevel = 1;
	public float damage = 10;

	[SerializeField] private GameObject bullet;
	[SerializeField] public float cooldown;

	private PlayerController m_playerController;
	private float m_nextShootTime;
    [SerializeField] private ObjectPool bulletPool;
    private void Start()
	{
		m_playerController = GetComponent<PlayerController>();
	}

	private void Update()
	{
		m_nextShootTime -= Time.deltaTime;
		if (m_nextShootTime <= 0)
		{
			shoot();
		}
	}

	private void shoot()
	{
		switch (currentLevel)
		{
			case 1:
				shootLevel1();
				break;
			case 2:
				shootLevel2();
				break;
			default:
				shootLevel3();
				break;
		}
		m_nextShootTime = cooldown;
	}


	private void shootLevel1()
	{
        PlayerDirection direction = m_playerController.playerDirection;
        shootBullet(direction);
    }

	private void shootBullet(PlayerDirection t_playerDirection)
	{
        //GameObject tempBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        GameObject tempBullet = bulletPool.GetObjectFromPool(bullet);
		tempBullet.transform.position = transform.position;
        BulletMovement tempBulletMovement = tempBullet.GetComponent<BulletMovement>();
		tempBulletMovement.setDirection(t_playerDirection);
		tempBulletMovement.setDamage((int)damage);
        tempBulletMovement.SetObjectPool(bulletPool);
    }

	private void shootLevel2()
	{
		switch (m_playerController.playerDirection)
		{
			case PlayerDirection.North:
				shootBullet(PlayerDirection.North);
				shootBullet(PlayerDirection.South);
				break;

			case PlayerDirection.South:
				shootBullet(PlayerDirection.North);
				shootBullet(PlayerDirection.South);
				break;

			case PlayerDirection.East:
				shootBullet(PlayerDirection.East);
				shootBullet(PlayerDirection.West);
				break;

			case PlayerDirection.West:
				shootBullet(PlayerDirection.East);
				shootBullet(PlayerDirection.West);
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private void shootLevel3()
	{
		shootBullet(PlayerDirection.East);
		shootBullet(PlayerDirection.West);
		shootBullet(PlayerDirection.North);
		shootBullet(PlayerDirection.South);
	}
}
