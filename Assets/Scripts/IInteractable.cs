using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    void Interact(Hero hero);
    bool IsBlocking(); // 맵상에서 이동을 막는가? 
}
