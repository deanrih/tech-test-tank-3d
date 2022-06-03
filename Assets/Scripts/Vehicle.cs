using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Photon.Pun;
using UnityEngine;

public interface IOwner
{
	GameObject GetObject();
}

public interface ISpawnable
{
	void SetID(String id);
}

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody), typeof(EventEmitter))]
public class Vehicle : MonoBehaviour, IDamageable, IDestroyable, IOwner, ISpawnable
{
	public String ID => this.id;

	public void SetID(String id)
	{
		this.SetID_Impl(id);
	}

	public GameObject GetObject()
	{
		return this.gameObject;
	}

	public void AcquireComponents()
	{
		this.AcquireComponents_Impl();
	}

	public void AlterHealth(Int32 value)
	{
		this.AlterHealth_Impl(value);
	}

	public void Destroy()
	{
		this.Destroy_Impl();
	}

	public void EvaluateHealth()
	{
		this.EvaluateHealth_Impl();
	}

	public void Shoot()
	{
		this.Shoot_Impl();
	}

	public void TakeDamage(Int32 value)
	{
		this.TakeDamage_Impl(value);
	}

	public void SetValues()
	{
		this.SetValues_Impl();
	}

	public void SetHealth()
	{
		this.SetHealth_Impl();
	}

	public void SetSpeed()
	{
		this.SetSpeed_Impl();
	}

	public void SetAmmo()
	{
		this.SetAmmo_Impl();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void AcquireComponents_Impl()
	{
		this.collider ??= this.GetComponent<BoxCollider>();
		this.rigidbody ??= this.GetComponent<Rigidbody>();

		this.eventEmitter ??= this.GetComponent<EventEmitter>();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void AlterHealth_Impl(Int32 value)
	{
		this.healthCurrent += value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Destroy_Impl()
	{
		this.eventEmitter.EmitEvent("onDestroy", this, this.ID);
		Destroy(this.gameObject);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void EvaluateHealth_Impl()
	{
		if (this.healthCurrent > 0)
		{
			return;
		}

		this.Destroy();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SetID_Impl(string id)
	{
		this.id = id;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SetValues_Impl()
	{
		this.SetHealth();
		this.SetSpeed();
		this.SetAmmo();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SetHealth_Impl()
	{
		this.healthCurrent = this.health;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SetSpeed_Impl()
	{
		this.speedCurrent = this.speed;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SetAmmo_Impl()
	{
		this.ammoCurrent = this.ammo;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Shoot_Impl()
	{
		if (this.ammoCurrent < 1)
		{
			return;
		}

		var bullet = Instantiate<Bullet>(this.pfb_Bullet);
		bullet.transform.position = this.attachmentsRoot.Find("Attachment.BulletSpawn").position;
		bullet.transform.rotation = this.attachmentsRoot.Find("Attachment.BulletSpawn").rotation;

		// Physics.IgnoreCollision(bullet.GetComponent<Collider>(), this.collider);

		if (bullet is IOwnable ownable)
		{
			ownable.SetOwner(this);
		}

		bullet.Engage();
		this.ammoCurrent -= 1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void TakeDamage_Impl(Int32 value)
	{
		if (this.isInvulnerable)
		{
			return;
		}

		Debug.Log($@"{this.ID} took {value} damage");
		value *= (value > 0 ? -1 : +1);

		this.AlterHealth(value);
		this.EvaluateHealth();
	}

#pragma warning disable IDE0051
	private void Update()
	{
		if (!this.photonView.IsMine)
		{
			return;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			this.Shoot();
		}

		this.moveDirection = Vector3.zero;
		this.moveDirection += Vector3.forward * (Input.GetKey(KeyCode.W) ? 1 : 0);
		this.moveDirection += Vector3.back * (Input.GetKey(KeyCode.S) ? 1 : 0);
		this.moveDirection += Vector3.left * (Input.GetKey(KeyCode.A) ? 1 : 0);
		this.moveDirection += Vector3.right * (Input.GetKey(KeyCode.D) ? 1 : 0);
		this.moveDirection.Normalize();

		this.transform.RotateAround(this.transform.position, Vector3.up, Vector3.Angle(this.transform.forward, this.moveDirection));
	}

	private void FixedUpdate()
	{
		// this.rigidbody.MovePosition(this.rigidbody.position + ((this.moveDirection * this.speed) * Time.fixedDeltaTime));
		this.rigidbody.AddForce(this.moveDirection * this.speed, ForceMode.Impulse);
	}

	private void Awake()
	{
		this.AcquireComponents();
		this.SetValues();
	}

	private void Reset()
	{
		this.AcquireComponents();
	}

	private void OnValidate()
	{
		this.AcquireComponents();
	}

	private void Start()
	{
		this.eventEmitter.EmitEvent("onSpawn", this, this.ID);
	}
#pragma warning restore IDE0051

	private new BoxCollider collider;
	private new Rigidbody rigidbody;

	private EventEmitter eventEmitter;

	private Vector3 moveDirection;

	private Int32 healthCurrent;
	private Int32 speedCurrent;
	private Int32 ammoCurrent;

	[SerializeField]
	protected PhotonView photonView;

	[SerializeField]
	protected Bullet pfb_Bullet;

	[SerializeField]
	protected Boolean isInvulnerable;

	[SerializeField]
	protected Int32 health;

	[SerializeField]
	protected Int32 speed;

	[SerializeField]
	protected Int32 ammo;

	[SerializeField]
	protected Transform attachmentsRoot;

	[SerializeField]
	protected String id;
}
