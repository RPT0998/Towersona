﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{   
    public static BuildManager Instance { get; private set; }

    [Header("Parameters")]
    public float timeBetweenTowersonas = 40f;

    [HideInInspector]
    public bool maxReached = false;
    [HideInInspector]
    public float lastXUsed = 0f;

    [Header("References")]
    [SerializeField]
    private GameObject[] towersonaPrefabs;

    public GameObject detailedTowersonaViewPrefab;

    [SerializeField]
    private NodeUI nodeUI;
    [SerializeField]
    private GameObject buildEffect;

    //Private parameters  
    private GameObject towersonaToBuild;
    private Towersona towersonaSelected;

    //Private references

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }        
    }

    private void Update()
    {
        //Hides nodeUI if not clicked on it
        if (nodeUI.UIIsActive)
        {          
            //https://answers.unity.com/questions/615771/how-to-check-if-click-mouse-on-object.html
            if (Input.GetMouseButtonDown(0))
            {
                PointerEventData pointerData = new PointerEventData(EventSystem.current);

                pointerData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                if (results.Count <= 0)               
                {
                    //Check if another towersona was clicked
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit))
                    {
                        Towersona t = hit.transform.gameObject.GetComponent<Towersona>();
                        if (t)
                        {
                            //A towersona was hit
                            SelectTowersona(t);
                        }
                        else
                        {
                            DeselectTowersona();
                        }                     
                    }
                }
            }
        }
    }

    public void SpawnTowersona(BuildingPlace place)
    {
        if (towersonaToBuild)
        {          
            GameObject towersonaGameObject = Instantiate(towersonaToBuild);
            towersonaGameObject.transform.SetParent(GameObject.FindGameObjectWithTag("Towersonas Parent").transform, true);
            towersonaGameObject.name = towersonaToBuild.name;

            Towersona towersona = towersonaGameObject.GetComponent<Towersona>();
            towersona.Spawn(place, towersonaGameObject.transform);
            PlayerStats.Instance.SpendMoney(towersona.costs[(int)towersona.towersonaLevel]);

            SpawnEffect(buildEffect, place.transform.position);
            
            //world.SelectTile(_tile);                   

            towersonaToBuild = null;
        }
    }    

    public void SelectTowersonaToBuild(Towersona _towersona)
    {
        towersonaToBuild = _towersona.gameObject;

        DeselectTowersona();
    }

    public void SelectTowersona(Towersona towersona)
    {
        if(towersonaSelected == towersona)
        {            
            return;
        }

        towersonaSelected = towersona;
        towersonaToBuild = null;
         
        nodeUI.SetTarget(towersona);
    }

    public void DeselectTowersona()
    {
        towersonaSelected = null;
        nodeUI.Hide();
    }   

    public void UpgradeTowersona()
    {
        towersonaSelected.LevelUp();
        SpawnEffect(buildEffect, towersonaSelected.place.transform.position);

        DeselectTowersona();     
    }

    public void Evolve(int evolution)
    {
        towersonaSelected.Evolve(evolution);
        SpawnEffect(buildEffect, towersonaSelected.transform.position);

        DeselectTowersona();
    }

    public void SpawnEffect(GameObject _effect, Vector3 position)
    {
        GameObject effect = Instantiate(_effect, position, Quaternion.identity);
        effect.transform.SetParent(GameObject.FindGameObjectWithTag("Effects Parent").transform, true);
        Destroy(effect, 5f);
    }

    private void OnGUI()
    {       
        if (towersonaSelected != null)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 32;
            style.fontStyle = FontStyle.Bold;

            string message = "";
            message += "Fuerza: " + towersonaSelected.stats.currentAttackStrength + "\n";
            message += "V. Ataque: " + towersonaSelected.stats.currentAttackSpeed + "\n";
            message += "Rango: " + towersonaSelected.stats.currentAttackRange + "\n";
            message += "V. Bala: " + towersonaSelected.stats.currentBulletSpeed + "\n";

            GUI.Label(new Rect(Screen.width * 0.66f + 110, 200, 120, 100), message, style);            
        }
    }
}
