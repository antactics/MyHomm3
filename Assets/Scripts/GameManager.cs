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
            Vector2 mouseWorld2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y); // 2D월드에서 좌표값을 받을 변수

            RaycastHit2D hit = Physics2D.Raycast(mouseWorld2D, Vector2.zero);

            if(hit.collider != null && hit.collider.GetComponent<Hero>() != null)
            {
                return; // Hero스크립트의 OnMouseDown에서 처리하고 여기에선 무시함
            }

            if(selectedHero != null)
            {
                Vector3Int cellpos = tilemap.WorldToCell(mouseWorldPos);
                Vector3 cellCenterWorld = tilemap.GetCellCenterWorld(cellpos);

                //스프라이트를 셀 중앙으로 맞추기 위해 보정
                cellCenterWorld.y -= 0.5f;

                selectedHero.MoveTo(cellCenterWorld);
            }
        }
    }

    public void SelectHero(Hero hero)
    {
        selectedHero = hero;
        Debug.Log($"선택된 영웅: {hero.name}");
    }

    
}
