using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public bool isEnemy = false; //false아군 true적군
    public bool isPlayerControlled = true; // 아군이면 true, 적이면 false

    public float moveSpeed = 3f;

    private Coroutine moveCoroutine;

    public IInteractable currentTarget = null;
    public bool isSelected = false;

    private void OnMouseDown()
    {
        if (!isPlayerControlled) return; // 적이면 무시
        GameManager.Instance.SelectHero(this);
    }

    public void SetTarget(IInteractable target)
    {
        currentTarget = target;
        Debug.Log("상호작용 대상 : " + target);
    }

    public void ClearTarget()
    {
        currentTarget = null;
    }

    public void MoveAlongPath(List<Vector3> path, Vector3Int targetTile, bool interactOnArrival = false)
    {
        if(moveCoroutine != null)
        StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveStepByStep(path, targetTile, interactOnArrival));
    }

    private IEnumerator MoveStepByStep(List<Vector3> path, Vector3Int targetTile, bool interactOnArrival = false)
    {
        foreach (Vector3 step in path)
        {
            Debug.Log("이동 대상 타일: " + step);
            Vector3 target = step;//  + new Vector3(0, -0.5f, 0); //스프라이트 보정

            while(Vector3.Distance(transform.position, target) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                yield return null;

                // 마우스 클릭시 이동 중단
                if (Input.GetMouseButtonDown(0))
                {
                    ClearTarget();
                    yield break;
                }
            }

             
        }

        moveCoroutine = null;

        if ( interactOnArrival && currentTarget !=null)
        {
            TryInteract(targetTile);
        }
    }

    public void TryInteract(Vector3Int tilePosition)
    {
        Vector3 worldPos = GameManager.Instance.tilemap.GetCellCenterWorld(tilePosition);
        Collider2D[] hits = Physics2D.OverlapCircleAll(worldPos, 0.1f);

        foreach(var hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();
            if(interactable != null && interactable == currentTarget)
            {
                interactable.Interact(this);
                ClearTarget();
                break;
            }
        }
        
    }


    //적 영웅을 공격하기 위한 스크립트
    public InteractionType GetInteractionType()
    {
        return InteractionType.Adjacent; 
    }

     public void AttackHero(Hero hero)
    {
        //씬체인저에서 배틀씬으로 전환해줌
        if (hero == null) return;

        if(hero.isEnemy && !this.isEnemy)
        {
            GameManager.Instance.SceneChange(hero);
        }

        else
        {
            Debug.Log("공격 대상이 적이 아님.");
        }

    }


}
