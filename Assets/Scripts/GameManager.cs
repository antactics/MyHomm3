using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    
    public int gold = 0;
    public TextMeshProUGUI goldText; 


    public static GameManager Instance { get; private set; }

    public Hero selectedHero;
    public Tilemap tilemap; //에디터에서 그리드의 타일맵을 할당
    public Pathfinder pathfinder;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void SelectHero(Hero hero)
    {
        selectedHero = hero;
        Debug.Log($"선택된 영웅: {hero.name}");
    }

    private void Update()
    {
        if (selectedHero != null && Input.GetMouseButton(0)) // 좌클릭을 하면
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition); //화면의 마우스클릭위치를 벡터3로 받아옴
            mouseWorld.z = 0;

            if(tilemap.HasTile(tilemap.WorldToCell(mouseWorld)))
            {
                List<Vector3> path = pathfinder.FindPath(selectedHero.transform.position, mouseWorld);
                if (path != null)
                    selectedHero.MoveAlongPath(path);
            }
        }
    }

    public void AddGold(int amount)
    {
        gold += amount;
        UpdateGoldUI();
    }

    void UpdateGoldUI()
    {
        if (goldText != null)
            goldText.text = gold.ToString();
    }


}
