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
        foreach(Vector3 step in path)
        {
            Vector3 target = step + new Vector3(0, -0.5f, 0); 

            while(Vector3.Distance(transform.position, target) > 0.01f)
            {
                
                transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                yield return null;
                Debug.Log("영웅이 이동함" + target);
                // 마우스 클릭시 이동 중단
                if (Input.GetMouseButtonDown(0))
                    yield break;
            }
        }
        moveCoroutine = null;
    }
}
