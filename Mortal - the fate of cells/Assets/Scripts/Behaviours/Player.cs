using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player PL { get; private set; }

    private void Awake()
    {
        PL = this;
    }
}
