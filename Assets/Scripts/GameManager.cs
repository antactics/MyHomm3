using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public int gold = 0;
    public TextMeshProUGUI goldText; 


    public static GameManager Instance { get; private set; }

    public Tilemap tilemap; //에디터에서 그리드의 타일맵을 할당
    public Hero selectedHero;
    public Pathfinder pathfinder;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void SelectHero(Hero hero)
    {
        selectedHero = hero;
        selectedHero.isSelected = true;
        Debug.Log($"선택된 영웅: {hero.name}");
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = tilemap.WorldToCell(worldPos);
            Collider2D hit = Physics2D.OverlapPoint(worldPos);

            if (selectedHero == null)
                return;

            if (hit != null)
            {
                // 먼저 적 영웅인지 확인
                Hero clickedHero = hit.GetComponent<Hero>();
                if (clickedHero != null && clickedHero.isEnemy && selectedHero != null && !selectedHero.isEnemy)
                {
                    // 적 영웅을 클릭했고, 선택된 영웅은 아군일 때 공격 시도
                    if (IsAdjacent(tilemap.WorldToCell(selectedHero.transform.position), tilemap.WorldToCell(clickedHero.transform.position)))
                    {
                        selectedHero.AttackHero(clickedHero);
                    }
                    else
                    {
                        Debug.Log("적과 인접하지 않아 공격할 수 없습니다.");
                        // 필요하면 인접 타일까지 이동 후 공격하도록 구현 가능
                    }
                    return; // 여기서 처리 완료했으니 아래 상호작용 처리 안함
                }


                IInteractable interactable = hit.GetComponent<IInteractable>();

                if (interactable != null)
                {
                    if(selectedHero.currentTarget == interactable)
                    {
                        //상호작용 
                        Vector3Int herotile = tilemap.WorldToCell(selectedHero.transform.position);
                        if(IsAdjacent(herotile, cellPos))
                        {
                            selectedHero.TryInteract(cellPos);
                        }

                        else
                        {
                            //인접하지 않으면 인접타일로 이동 후 상호작용
                            Vector3Int neighborTile = FindAdjacentMovableTile(herotile, cellPos);
                            if(neighborTile != Vector3Int.zero)
                            {
                                var path = Pathfinder.Instance.FindPath(herotile, neighborTile);
                                selectedHero.MoveAlongPath(path, cellPos, true);
                            }
                        }
                    }

                    else
                    {
                        selectedHero.SetTarget(interactable);
                    }

                    return;
                }
            }

            // 클릭한 곳이 타일이면 일반이동
            if(tilemap.HasTile(cellPos))
            {
                selectedHero.ClearTarget();
                Vector3Int start = tilemap.WorldToCell(selectedHero.transform.position);
                var path = Pathfinder.Instance.FindPath(start, cellPos);
                selectedHero.MoveAlongPath(path, cellPos, false);
            }
        }
    }

    //인접여부 판단
    private bool IsAdjacent(Vector3Int a, Vector3Int b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        return (dx + dy == 1);
    }

    private Vector3Int FindAdjacentMovableTile(Vector3Int from, Vector3Int to)
    {
        Vector3Int[] directions = new Vector3Int[]
        {
            new Vector3Int(1,0,0), new Vector3Int(-1,0,0),
            new Vector3Int(0,1,0), new Vector3Int(0,-1,0),
        };

        foreach (var dir in directions)
        {
            Vector3Int neighbor = to + dir;
            if (Pathfinder.Instance.IsWalkable(neighbor))
                return neighbor;
        }

        return Vector3Int.zero;

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



    public void SceneChange(Hero enemyHero)
    {
        Debug.Log($"전투씬으로 전환. 적 영웅 : {enemyHero.name}");

        //SceneManager.LoadScene("BattleScene");
        SceneManager.LoadScene(1);
    }
}
