using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public interface IDamageable
{
	void TakeDamage(Int32 damage);
}

public interface IDestroyable
{
	void Destroy();
}

[RequireComponent(typeof(BoxCollider))]
public class Block : MonoBehaviour, IDamageable, IDestroyable
{
	public void Destroy()
	{
		this.Destroy_Impl();
	}

	public void TakeDamage(Int32 damage)
	{
		this.TakeDamage_Impl(damage);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void TakeDamage_Impl(Int32 value)
	{
		if (this.isInvulnerable)
		{
			return;
		}

		value *= (value > 0 ? -1 : +1);

		this.AlterHealth_Impl(value);
		this.EvaluateHealth_Impl();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void AlterHealth_Impl(Int32 value)
	{
		this.healthCurrent += value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void EvaluateHealth_Impl()
	{
		if (this.healthCurrent > 0)
		{
			return;
		}

		this.Destroy_Impl();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Destroy_Impl()
	{
		Destroy(this.gameObject);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void AcquireComponents_Impl()
	{
		this.collider ??= this.GetComponent<BoxCollider>();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void InitializeHealth_Impl()
	{
		this.healthCurrent = this.health;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void UpdateColliderTriggerType_Impl()
	{
		this.collider.isTrigger = !this.isSolid;
	}

#pragma warning disable IDE0051
	private void Awake()
	{
		this.AcquireComponents_Impl();
		this.InitializeHealth_Impl();
	}

	private void Start()
	{

	}

	private void Reset()
	{
		this.AcquireComponents_Impl();
	}

	private void OnValidate()
	{
		this.AcquireComponents_Impl();
		this.UpdateColliderTriggerType_Impl();
	}
#pragma warning restore IDE0051

	private new BoxCollider collider;

	private Int32 healthCurrent;

	[SerializeField]
	protected Boolean isSolid;

	[SerializeField]
	protected Boolean isInvulnerable;

	[SerializeField]
	protected Int32 health;
}