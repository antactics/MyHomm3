using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    bool IsBlocking(); // 맵상에서 이동을 막는가? 
    InteractionType GetInteractionType();
    void Interact(Hero hero);
}

public enum InteractionType
{
    Adjacent, // 인접한 타일에 상호작용
    Touch // 겹친 타일에 상호작용
}
