﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Draggable), typeof(ReturnToPointAfterCountdown))]
public class Food : MonoBehaviour
{
    //Raycast constants
    private const int RAYCAST_HIT_ARRAY_SIZE = 1;
    private const float MAX_RAYCAST_DISTANCE = 50;

    //Inspector
    public float HungerFulmilmentPerRation => _hungerFulmilmentPerRation;
    [SerializeField] float _hungerFulmilmentPerRation = 1;

    //References
    private new Transform transform;
    private Renderer[] renderers;
    private Collider[] colliders;
    private ReturnToPointAfterCountdown returnToPoint;
    private Draggable draggable;

    //Cached raycast stuff
    private RaycastHit[] hits;
    LayerMask raycastLayerMask;
    

    public void OnLetGo(PointerEventData pointerEventData)
    {
        //If we're over a Feedable, feed them and do out Eaten stuff.
        Camera camera = pointerEventData.pressEventCamera;
        Ray ray = camera.ScreenPointToRay(pointerEventData.position);

        int hitCount = Physics.RaycastNonAlloc(ray, hits, MAX_RAYCAST_DISTANCE, raycastLayerMask, QueryTriggerInteraction.Collide);

        Feedable feedable;
        for (int i = 0; i < hitCount; i++)
        {
            feedable = hits[i].collider.GetComponent<Feedable>();
            if (feedable && feedable.enabled)
            {
                feedable.Feed(this);
                OnEaten();
                break;
            }
        }
    }


    private void OnEaten()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = false;
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }

        draggable.enabled = false;

        returnToPoint.tweenToPosition = false;
        returnToPoint.OnReturnedToPoint.AddListener(OnRespawned);
    }

    private void OnRespawned()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = true;
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = true;
        }

        draggable.enabled = true;

        returnToPoint.tweenToPosition = true;
        returnToPoint.OnReturnedToPoint.RemoveListener(OnRespawned);
    }


    private void Awake()
    {
        //Gather references
        transform = GetComponent<Transform>();
        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();
        returnToPoint = GetComponent<ReturnToPointAfterCountdown>();
        draggable = GetComponent<Draggable>();

        //Ensure correct layer setup
        int feedableLayer = LayerMask.NameToLayer(Feedable.FEEDABLE_LAYER_NAME);
        Debug.Assert(gameObject.layer != feedableLayer, 
            $"Food components must not be in {Feedable.FEEDABLE_LAYER_NAME}.If they are, they will hit themselves with their raycasts.", this);

        //Create necessary stuff
        hits = new RaycastHit[RAYCAST_HIT_ARRAY_SIZE];
        raycastLayerMask = LayerMask.GetMask(Feedable.FEEDABLE_LAYER_NAME);
    }
}
