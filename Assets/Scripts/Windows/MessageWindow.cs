using System;
using UnityEngine;
using UnityEngine.UI;

public class MessageWindow : Window
{
    public Action onClickOkButton;

    [SerializeField] private Text _messageText;

    public void Show(string messageText)
    {
        base.Show();

        _messageText.text = messageText;
    }

    public void OnClickOkButton()
    {
        onClickOkButton?.Invoke();
    }
}
