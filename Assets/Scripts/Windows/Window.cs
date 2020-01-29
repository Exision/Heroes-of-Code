using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window : MonoBehaviour
{
    public System.Action onEndShowAnimation;
    public System.Action onEndHideAnimation;

    [SerializeField] Animator animator;

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
        if (gameObject.activeSelf)
            return;

        gameObject.SetActive(true);

        if (animator != null)
            animator.enabled = false;
        else
            animation = false;

        if (animation)
        {
            animator.enabled = true;
            animator.Play(showHash, -1, 0.0f);
            animator.Update(0.0f);
        }
        else
            OnEndShowAnimation();
    }

    public virtual void Hide(bool animation = true)
    {
        if (animator == null)
            animation = false;

        if (animation)
        {
            animator.enabled = true;
            animator.Play(hideHash, -1, 0.0f);
            animator.Update(0.0f);
        }
        else
            OnEndHideAnimation();

        onEndShowAnimation = null;
    }

    public virtual void OnEndShowAnimation()
    {
        if (animator != null)
            animator.enabled = false;

        onEndShowAnimation?.Invoke();
    }

    public virtual void OnEndHideAnimation()
    {
        if (animator != null)
            animator.enabled = false;

        gameObject.SetActive(false);

        onEndHideAnimation?.Invoke();

        onEndHideAnimation = null;
    }
}
