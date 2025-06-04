using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldObject : MonoBehaviour, IInteractable
{
    public int goldAmout = 100;

    public bool IsBlocking() => true;

    public InteractionType GetInteractionType()
    {
        return InteractionType.Adjacent; //°ñµå´Â ÀÎÁ¢½Ã¿¡¸¸ È¹µæ°¡´É
    }

    public void Interact(Hero hero)
    {
        GameManager.Instance.AddGold(goldAmout);
        Debug.Log($"{goldAmout} ¸¸Å­ °ñµå È¹µæ");
        Destroy(gameObject);
    }
}
