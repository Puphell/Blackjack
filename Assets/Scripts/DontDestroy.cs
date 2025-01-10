using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static DontDestroy instance;

    private void Awake()
    {
        // Eðer baþka bir instance varsa bu objeyi yok et, yoksa bu objeyi instance yap.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Objeyi sahneler arasýnda yok etme.
        }
        else
        {
            Destroy(gameObject); // Bu objeden baþka bir tane varsa onu yok et.
        }
    }
}