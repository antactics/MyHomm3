using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public float moveSpeed = 3f;

    private Coroutine moveCoroutine;

    private void OnMouseDown()
    {
        GameManager.Instance.SelectHero(this);
    }

    public void MoveAlongPath(List<Vector3> path)
    {
        if(moveCoroutine != null)
        StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveStepByStep(path));
        
    }

    private IEnumerator MoveStepByStep(List<Vector3> path)
    {
        for (int i = 0; i<path.Count; i++)
        {
            Vector3 step = path[i];
            Vector3 target = step + new Vector3(0, -0.5f, 0); 

            while(Vector3.Distance(transform.position, target) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                yield return null;

                // 마우스 클릭시 이동 중단
                if (Input.GetMouseButtonDown(0))
                    yield break;
            }

            if (i == path.Count - 1)
            {
                Vector3Int tilePos = GameManager.Instance.tilemap.WorldToCell(step);
                TryInteract(tilePos);
            }
        }

        moveCoroutine = null;
    }

    public void TryInteract(Vector3Int tilePosition)
    {
        Vector3 worldPos = GameManager.Instance.tilemap.GetCellCenterWorld(tilePosition);
        Collider2D[] hits = Physics2D.OverlapCircleAll(worldPos, 0.1f);
        foreach(var hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();
            if(interactable != null)
            {
                interactable.Interact(this);
            }
        }
    }

    
}
