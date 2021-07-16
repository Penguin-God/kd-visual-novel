﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionEffect : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] ParticleSystem ps_HitEffect;

    public void Throw_QuestionMark(Vector3 _target)
    {
        StartCoroutine(Throw_Coroutine(_target));
    }

    IEnumerator Throw_Coroutine(Vector3 _target)
    {
        float distance_byTarget = Mathf.Infinity;
        while (distance_byTarget > 0.3f)
        {
            transform.position =  Vector3.Lerp(transform.position, _target, moveSpeed);
            distance_byTarget = Vector3.Distance(transform.position, _target);
            yield return null;
        }
        Play_HitEffect();
        gameObject.SetActive(false);
    }

    void Play_HitEffect()
    {
        ps_HitEffect.gameObject.SetActive(true);
        ps_HitEffect.transform.position = transform.position;
        ps_HitEffect.Play();
        Invoke("Hide_HitEffect", ps_HitEffect.startLifetime);

        DialogueManager.instance.ShowDialogue();
    }

    void Hide_HitEffect()
    {
        ps_HitEffect.gameObject.SetActive(false);
    }
}
