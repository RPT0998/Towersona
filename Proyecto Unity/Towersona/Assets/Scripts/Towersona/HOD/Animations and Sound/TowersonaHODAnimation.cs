﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 649
public class TowersonaHODAnimation : MonoBehaviour
{
	public LookAwayFromTouch lookAway;

	[SerializeField] private LookAt lookAt;
	[SerializeField] private Animator bodyAnimator;
	[SerializeField] private TowersonaLOD towersonaLOD;

    private bool isLookingAtFood;
	private Animator lodBodyAnimator;

	private void Start()
	{
		lodBodyAnimator = towersonaLOD.GetComponent<TowersonaLODAnimation>().bodyAnimator;
	}

	public void Eat()
	{
		//JIJI
	}
  
    public void SetLoneliness(bool loneliness)
	{
		//TODO: evitar que esto se esté llamando todo el rato
		/*bodyAnimator.SetBool("isLonely", loneliness);
		lodBodyAnimator.SetBool("isLonely", loneliness);*/
	}

	public void TakeAShit()
	{
		//bodyAnimator.SetTrigger("takeADump");
	}

    public void CaressStart()
    {
		if(lookAway) lookAway.isBeingCaressed = true;
	}

    public void CaressEnd()
    {
		if (lookAway) lookAway.isBeingCaressed = false;
	}
    
    public void SetIsLookingAtFood(bool _isLookingAtFood)
    {
        isLookingAtFood = _isLookingAtFood;
        if(lookAt) lookAt.enabled = _isLookingAtFood;
    }

    public void SetLookAtTarget(Transform tr)
    {
        if(lookAt) lookAt.food = tr;
    }


}