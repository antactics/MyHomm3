using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameManager.Instance.SelectHero(this);
    }

    public void MoveTo(Vector3 targetPos)
    {
        transform.position = targetPos;
        
    }
}
