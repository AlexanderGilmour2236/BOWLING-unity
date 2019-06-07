﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public Menu prevMenu;

    public virtual void Show(bool showParameter)
    {
        Debug.Log("Show");
        gameObject.SetActive(showParameter);
    }
}
