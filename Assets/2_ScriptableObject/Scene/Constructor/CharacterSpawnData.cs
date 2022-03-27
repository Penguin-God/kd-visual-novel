using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new parser", menuName = "Scriptable Object / Scenes / CharacterSpawnData")]
public class CharacterSpawnData : SpawnInteractionObjectData
{
    [SerializeField] GameObject characterContainer = null;
    [SerializeField] Sprite spawnSprite = null;

    public override GameObject GetInteractionObject()
    {
        GameObject _my = Instantiate(characterContainer, spawnPos, Quaternion.Euler(spawnEulerAngles));
        _my.GetComponent<InteractionName>().SetName(interactionName);
        _my.GetComponent<InteractionEvent>().SetMC(dialogueMC);

        SpriteRenderer[] _srs = _my.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < _srs.Length; i++) _srs[i].sprite = spawnSprite;

        return _my;
    }
}