using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class PopUpLabel : MonoBehaviour
{
    [SerializeField]
    private Text BackLabel;
    [SerializeField]
    private Text FrontLabel;

    private Animator _animator;
    
    private string _text;
    public string Text
    {
        get { return _text; }
        set
        {
            _text = value;
            BackLabel.text = _text;
            FrontLabel.text = _text;
        }
    }
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Show()
    {
        _animator.Play("Show");
    }
}
