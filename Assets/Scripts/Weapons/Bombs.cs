﻿using UnityEngine;

/**
 * 
 */
public class Bombs : Weapon
{
	private float charge;
	public float initialCharge = 0.5f;
    public float chargeFactor = 3f;
    public float maxCharge = 3f;

	private float force = 25f;
	private float coolDownDuration = 0.25f;

	public Bomb bomb;
	private Bomb bombInstance;

	/**
	 * 
	 */
	public void Start()
	{
		this.charge = this.initialCharge;
	}

    /**
	 * 
	 */
    public override bool CanBeUsed(Player player)
	{
		CharacterController2D controller = player.GetComponent<CharacterController2D>();

		return controller && !controller.IsClimbing();
	}

	/**
	 * 
	 */
	public override void Press(Player player)
	{
		if (this.bombInstance || !this.IsCooledDown() || !this.CanBeUsed(player))
        {
            return;
        }

        this.Draw(player);
	}

	/**
	 * 
	 */
	public void Draw(Player player)
	{
		this.bombInstance = Instantiate(bomb, player.transform.position, Quaternion.identity) as Bomb;

		player.Grab(bombInstance.gameObject);

		this.bombInstance.LightFuse();
	}

	/**
	 * 
	 */
	public override void Hold(Player player)
	{
        if (!this.bombInstance)
        {
            return;
        }

        this.Charge(Time.deltaTime * chargeFactor);
	}

	/**
	 * 
	 */
	private void Charge(float deltaCharge)
	{
        if (!this.bombInstance)
        {
            return;
        }

        this.charge = Mathf.Min(this.maxCharge, this.charge + deltaCharge);
	}

	/**
	 * 
	 */
	public override void Release(Player player)
	{
        if (!this.bombInstance)
        {
            return;
        }

        this.Throw(player);

		this.SetCoolDown(this.coolDownDuration);
	}

	/**
	 * 
	 */
	private void Throw(Player player)
	{
		player.Throw(player.GetAim() * this.charge * this.force);

		this.bombInstance = null;

		this.charge = this.initialCharge;
	}
}
