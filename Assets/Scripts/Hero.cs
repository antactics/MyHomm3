using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    private bool isMoving = false;

    private void OnMouseDown()
    {
        if(!isMoving)
        {
        GameManager.Instance.SelectHero(this);
        }
    }

    public void MoveTo(Vector3 targetPos)
    {
        if (!isMoving)
        {
            StartCoroutine(MoveToPosition(targetPos));
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPos)
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        float distance = Vector3.Distance(startPos, targetPos);
        float speed = 10f;

        float travelTime = distance / speed;
        float elapsed = 0f;

        while (elapsed < travelTime)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / travelTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
        Debug.Log("영웅이 이동함" + targetPos);

    }

}
