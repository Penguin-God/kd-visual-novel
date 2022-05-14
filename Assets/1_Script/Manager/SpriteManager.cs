using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteManager : MonoBehaviour
{
    [SerializeField] DialogueChannel dialogueChannel = null;
    [SerializeField] CameraRotateTalkDirector rotateTalkDirector = null;
    Image[] CurrentChacterImages => rotateTalkDirector.CurrentImageField.GetComponentsInChildren<Image>();

    SpriteFadeManager spriteFadeManager;
    private void Awake() => spriteFadeManager = gameObject.AddComponent<SpriteFadeManager>();
    private void Start()
    {
        dialogueChannel.ChangeContextEvent += ChangeSprite_byTalk;

        dialogueChannel.StartTalkEvent += (_con) => FadeOut_AllSceneCharacters();
        dialogueChannel.EndTalkEvent += (_con) => FadeIn_AllSceneCharacters();

        MySceneManager.Instance.OnSceneLoadComplete += (_data) => FadeIn_AllSceneCharacters();
    }

    void ChangeSprite_byTalk(DialogueData _data, int _contextCount)
    {
        if (_data.spriteNames[_contextCount] == "") return;
        ChangeSprite(CurrentChacterImages, _data.spriteNames[_contextCount]);
    }

    void ChangeSprite(Image[] _images, GameObject _target)
    {
        Sprite _sprite = GetSprite(_target);
        ChangeSprite(_images, _sprite);
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

    // 모든 ChangeSprite의 종점
    void ChangeSprite(Image[] _images, Sprite _newSprite)
    {
        StopAllCoroutines();
        spriteFadeManager.ChangeSprite(_images, _newSprite);
    }
    void ChangeSprite(SpriteRenderer[] _srs, Sprite _newSprite)
    {
        StopAllCoroutines();
        spriteFadeManager.ChangeSprite(_srs, _newSprite);
    }


    Sprite GetSprite(string spriteName) => Resources.Load("Characters/" + spriteName, typeof(Sprite)) as Sprite;
    Sprite GetSprite(GameObject _obj) => _obj.GetComponentInChildren<SpriteRenderer>().sprite;
    SpriteRenderer[] GetSpriteRenderers(GameObject _object) => _object.GetComponentsInChildren<SpriteRenderer>();


    IReadOnlyList<GameObject> CurrentSceneCharacters => DialogueSystem.Instance.DynamicDialogueObjects;
    void FadeOut_AllSceneCharacters()
    {
        if (CurrentSceneCharacters == null) return;
        for (int i = 0; i < CurrentSceneCharacters.Count; i++)
            spriteFadeManager.SpriteFadeOut(GetSpriteRenderers(CurrentSceneCharacters[i]));
    }
    void FadeIn_AllSceneCharacters()
    {
        if (CurrentSceneCharacters == null) return;
        for (int i = 0; i < CurrentSceneCharacters.Count; i++)
            spriteFadeManager.SpriteFadeIn(GetSpriteRenderers(CurrentSceneCharacters[i]));
    }
}



public class SpriteFadeManager : MonoBehaviour
{
    [SerializeField] float fadeSpeed = 0.1f;

    // 스프라이트 변환
    public void ChangeSprite(Image[] _images, Sprite _newSprite) => StartCoroutine(Co_ChangeSprite(_images, _newSprite));
    IEnumerator Co_ChangeSprite(Image[] _images, Sprite _newSprite)
    {
        if (_newSprite != null && !Check_SameSprite(_images[0].sprite, _newSprite))
        {
            Color _front_color = _images[0].color;
            _front_color.a = 0;
            _images[0].color = _front_color;
            _images[0].sprite = _newSprite;

            Color shadowColor = _images[1].color;
            shadowColor.a = 0;
            _images[1].color = shadowColor;
            _images[1].sprite = _newSprite;

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

    public void ChangeSprite(SpriteRenderer[] _srs, Sprite _newSprite) => StartCoroutine(Co_ChangeSprite(_srs, _newSprite));
    IEnumerator Co_ChangeSprite(SpriteRenderer[] _srs, Sprite _newSprite)
    {

        if (_newSprite != null && !Check_SameSprite(_srs[0].sprite, _newSprite))
        {
            Color _front_color = _srs[0].color;
            _front_color.a = 0;
            _srs[0].color = _front_color;
            _srs[0].sprite = _newSprite;

            Color shadowColor = _srs[1].color;
            shadowColor.a = 0;
            _srs[1].color = shadowColor;
            _srs[1].sprite = _newSprite;

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


    // 스프라이트 투명화, 불투명화
    public void SpriteFadeOut(SpriteRenderer[] _srs) => StartCoroutine(Co_SpriteFadeOut(_srs));
    IEnumerator Co_SpriteFadeOut(SpriteRenderer[] _srs)
    {
        Color _front_color = _srs[0].color;
        Color shadowColor = _srs[1].color;

        while (_front_color.a > 0f)
        {
            _front_color.a -= fadeSpeed;
            _srs[0].color = _front_color;

            shadowColor.a -= fadeSpeed;
            _srs[1].color = shadowColor;
            yield return null;
        }
    }

    public void SpriteFadeIn(SpriteRenderer[] _srs) => StartCoroutine(Co_SpriteFadeIn(_srs));
    IEnumerator Co_SpriteFadeIn(SpriteRenderer[] _srs)
    {
        Color _front_color = _srs[0].color;
        Color shadowColor = _srs[1].color;

        while (_front_color.a < 1f)
        {
            _front_color.a += fadeSpeed;
            _srs[0].color = _front_color;

            shadowColor.a += fadeSpeed;
            _srs[1].color = shadowColor;
            yield return null;
        }
    }


    bool Check_SameSprite(Sprite _chageSprite, Sprite p_Sprite)
    {
        if (_chageSprite == p_Sprite) return true;
        else return false;
    }
}