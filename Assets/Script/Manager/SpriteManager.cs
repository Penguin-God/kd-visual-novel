using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteManager : MonoBehaviour
{
    [SerializeField] DialogueCannel dialogueCannel = null;
    
    private void Start()
    {
        dialogueCannel.StartInteractionEvent += (_target, _container) => ChangeSprite(characterUI_Images, _target.gameObject);

        dialogueCannel.StartTalkEvent += (_con) => characterImageField.SetActive(true);
        dialogueCannel.EndTalkEvent += () => characterImageField.SetActive(false);

        dialogueCannel.ChangeContextEvent += ChangeSprite_byTalk;
    }

    [SerializeField] GameObject characterImageField = null;
    [SerializeField] Image[] characterUI_Images = null;
    void ChangeSprite_byTalk(DialogueData _data, int _contextCount)
    {
        if (_data.spriteNames[_contextCount] == "") return;

        ChangeSprite(characterUI_Images, _data.spriteNames[_contextCount]);
    }


    void ChangeSprite(Image[] _images, string _spriteName)
    {
        Sprite _sprite = GetSprite(_spriteName);
        ChangeSprite(_images, _sprite);
    }
    void ChangeSprite(SpriteRenderer[] _srs, string _spriteName)
    {
        Sprite _sprite = GetSprite(_spriteName);
        ChangeSprite(_srs, _sprite);
    }

    void ChangeSprite(Image[] _images, GameObject _object)
    {
        Sprite _sprite = GetSprite(_object);
        ChangeSprite(_images, _sprite);
    }
    void ChangeSprite(SpriteRenderer[] _srs, GameObject _object)
    {
        Sprite _sprite = GetSprite(_object);
        ChangeSprite(_srs, _sprite);
    }

    // 모든 ChangeSprite의 종점
    void ChangeSprite(Image[] _images, Sprite _newsprite)
    {
        StopAllCoroutines();
        StartCoroutine(Co_ChangeSprite(_images, _newsprite));
    }
    void ChangeSprite(SpriteRenderer[] _srs, Sprite _newsprite)
    {
        StopAllCoroutines();
        StartCoroutine(Co_ChangeSprite(_srs, _newsprite));
    }

    [SerializeField] float fadeSpeed;
    IEnumerator Co_ChangeSprite(Image[] _images,  Sprite _newsprite)
    {

        if (_newsprite != null && !Check_SameSprite(_images[0].sprite, _newsprite))
        {
            Color _front_color = _images[0].color;
            _front_color.a = 0;
            _images[0].color = _front_color;
            _images[0].sprite = _newsprite;

            Color shadowColor = _images[1].color;
            shadowColor.a = 0;
            _images[1].color = shadowColor;
            _images[1].sprite = _newsprite;

            while (_front_color.a < 1f)
            {
                _front_color.a += fadeSpeed;
                _images[0].color = _front_color;

                shadowColor.a += fadeSpeed;
                _images[1].color = shadowColor;
                yield return null;
            }
        }
    }
    IEnumerator Co_ChangeSprite(SpriteRenderer[] _srs, Sprite _newsprite)
    {

        if (_newsprite != null && !Check_SameSprite(_srs[0].sprite, _newsprite))
        {
            Color _front_color = _srs[0].color;
            _front_color.a = 0;
            _srs[0].color = _front_color;
            _srs[0].sprite = _newsprite;

            Color shadowColor = _srs[1].color;
            shadowColor.a = 0;
            _srs[1].color = shadowColor;
            _srs[1].sprite = _newsprite;

            while (_front_color.a < 1f)
            {
                _front_color.a += fadeSpeed;
                _srs[0].color = _front_color;

                shadowColor.a += fadeSpeed;
                _srs[1].color = shadowColor;
                yield return null;
            }
        }
    }

    bool Check_SameSprite(Sprite _chageSprite, Sprite p_Sprite)
    {
        if (_chageSprite == p_Sprite) return true;
        else return false;
    }


    SpriteRenderer[] GetSpriteRenderers(GameObject _object) => _object.GetComponentsInChildren<SpriteRenderer>();
    Sprite GetSprite(string spriteName) => Resources.Load("Characters/" + spriteName, typeof(Sprite)) as Sprite;
    Sprite GetSprite(GameObject _obj) => _obj.GetComponentInChildren<SpriteRenderer>().sprite;
}