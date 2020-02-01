using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public Action onEndShowAnimation;
    public Action onEndHideAnimation;
    public Action onClickCloseButton;

    [SerializeField] private Animator _animator;

    int showHash = Animator.StringToHash("ShowAnimation");
    int hideHash = Animator.StringToHash("HideAnimation");

    public virtual bool IsShow()
    {
        if (gameObject.activeSelf)
            return true;

        return false;
    }

    public virtual void Show(bool animation = true)
    {
        gameObject.SetActive(true);

        if (_animator != null)
            _animator.enabled = false;
        else
            animation = false;

        if (animation)
        {
            _animator.enabled = true;
            _animator.Play(showHash, -1, 0.0f);
            _animator.Update(0.0f);
        }
        else
            OnEndShowAnimation();
    }

    public virtual void Hide(bool animation = true)
    {
        if (_animator == null)
            animation = false;

        if (animation)
        {
            _animator.enabled = true;
            _animator.Play(hideHash, -1, 0.0f);
            _animator.Update(0.0f);
        }
        else
            OnEndHideAnimation();

        onEndShowAnimation = null;

        Destroy(gameObject);
    }

    public virtual void OnEndShowAnimation()
    {
        if (_animator != null)
            _animator.enabled = false;

        onEndShowAnimation?.Invoke();
    }

    public virtual void OnEndHideAnimation()
    {
        if (_animator != null)
            _animator.enabled = false;

        gameObject.SetActive(false);

        onEndHideAnimation?.Invoke();

        onEndHideAnimation = null;
    }

    public virtual void OnClickCloseButton()
    {
        onClickCloseButton?.Invoke();
    }
}
