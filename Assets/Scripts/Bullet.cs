using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public interface IOwnable
{
	void SetOwner(IOwner value);
}

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class Bullet : MonoBehaviour, IDestroyable, IOwnable
{
	public void AcquireComponents()
	{
		this.AcquireComponents_Impl();
	}

	public void Destroy()
	{
		this.Destroy_Impl();
	}

	public void Engage()
	{
		this.Engage_Impl();
	}

	public void SetOwner(IOwner owner)
	{
		this.SetOwner_Impl(owner);
	}

	public void SetValues()
	{
		this.SetValues_Impl();
	}

	public void SetDamage(Int32 value)
	{
		this.SetDamage_Impl(value);
	}

	public void SetRange(Int32 value)
	{
		this.SetRange_Impl(value);
	}

	public void SetSpeed(Int32 value)
	{
		this.SetSpeed_Impl(value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Destroy_Impl()
	{
		Destroy(this.gameObject);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Engage_Impl()
	{
		this.isMoving = true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SetOwner_Impl(IOwner owner)
	{
		this.owner = owner;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SetDamage_Impl(Int32 value)
	{
		this.damageCurrent = value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SetRange_Impl(Int32 value)
	{
		this.rangeCurrent = value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SetSpeed_Impl(Int32 value)
	{
		this.velocityCurrent = value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void SetValues_Impl()
	{
		this.SetDamage(this.damage);
		this.SetRange(this.range);
		this.SetSpeed(this.speed);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void AcquireComponents_Impl()
	{
		this.collider ??= this.GetComponent<BoxCollider>();
		this.rigidbody ??= this.GetComponent<Rigidbody>();
	}

#pragma warning disable IDE0051
	private void FixedUpdate()
	{
		if (!this.isMoving)
		{
			return;
		}

		this.rigidbody.MovePosition(this.transform.position + ((this.transform.forward * this.velocityCurrent) * Time.fixedDeltaTime));
	}

	private void OnTriggerEnter(Collider other)
	{
		var instances = other.GetComponents<MonoBehaviour>();

		foreach (var instance in instances)
		{
			if (instance is IOwner owner && owner == this.owner)
			{
				return;
			}

			if (instance is IDamageable damageable)
			{
				damageable.TakeDamage(this.damageCurrent);
			}
		}

		this.Destroy();
	}

	private void Awake()
	{
		this.AcquireComponents();
		this.SetValues();

		this.isMoving = false;
	}

	private void Reset()
	{
		this.AcquireComponents();
	}

	private void OnValidate()
	{
		this.AcquireComponents();
	}
#pragma warning restore IDE0051

	private new BoxCollider collider;
	private new Rigidbody rigidbody;

	private IOwner owner;

	private Boolean isMoving;

	private Int32 damageCurrent;
	private Int32 rangeCurrent;
	private Int32 velocityCurrent;

	[SerializeField]
	protected Int32 damage;

	[SerializeField]
	protected Int32 range;

	[SerializeField]
	protected Int32 speed;
}