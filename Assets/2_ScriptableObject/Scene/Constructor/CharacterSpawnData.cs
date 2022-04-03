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
        GameObject _obj = Instantiate(characterContainer, spawnPos, Quaternion.Euler(spawnEulerAngles));
        _obj.GetComponent<InteractionName>().SetName(interactionName);
        _obj.GetComponent<InteractionEvent>().SetMC(dialogueMC);
        

        SpriteRenderer[] _srs = _obj.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < _srs.Length; i++)
        {
            _srs[i].color = new Color(1, 1, 1, 0);
            _srs[i].sprite = spawnSprite;
        }

        return _obj;
    }
}