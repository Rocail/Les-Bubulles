using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
   public void doQuit()
    {
        print("Button Quit pressed, Game quitting");
        Application.Quit();
    }
}
