using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldObject : MonoBehaviour, IInteractable
{
    public int goldAmout = 100;

    public bool IsBlocking() => true;

    public void Interact(Hero hero)
    {
        GameManager.Instance.AddGold(goldAmout);
        Debug.Log($"{goldAmout} ¸¸Å­ °ñµå È¹µæ");
        Destroy(gameObject);
    }
}
