using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class DreamMaster : MonoBehaviour
{
    public Animator sunAnimator;
    private event Action decideNum;
    private float timer;
    
    private int effectChoice;

    void Start()
    {
     
        timer = Random.Range(10, 30);
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            decideNum?.Invoke();
        }
    }

  

    void OnEnable()
    {
        decideNum += RandomNum;
        decideNum += EffectChooser;
    }

    void OnDisable()
    {
        decideNum -= RandomNum;
        decideNum -= EffectChooser;
    }

    void RandomNum() => timer = Random.Range(10, 30);

    void EffectChooser()
    {
        effectChoice = Random.Range(1, 5);
     //   if (effectChoice == 2) ToggleRandomSway();
    }
}
