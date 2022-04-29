using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private InputHandler _inputHandler;
    private Animator _animator;
    private void Start()
    {
        _inputHandler = GetComponent<InputHandler>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        _inputHandler.isInteracting = _animator.GetBool("isInteracting");
        _inputHandler.rollFlag = false;
    }
}
