using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Hero selectedHero;
    public Tilemap tilemap; //에디터에서 그리드의 타일맵을 할당

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)) // 좌클릭을 하면
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //화면의 마우스클릭위치를 벡터3로 받아옴
            Vector3Int cellPos = tilemap.WorldToCell(mouseWorldPos); //마우스클릭위치값을 cell위치로 받아옴
            Vector3 cellCenterWorld = tilemap.GetCellCenterWorld(cellPos); //타일 셀의 중앙위치
            
            if(selectedHero != null)
            {
                selectedHero.MoveTo(cellCenterWorld);
                Debug.Log($"영웅이 {cellCenterWorld}로 이동함");
            }
        }
    }

    public void SelectHero(Hero hero)
    {
        selectedHero = hero;
        Debug.Log($"선택된 영웅: {hero.name}");
    }

    
}
