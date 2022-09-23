using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void JOGAR (){
      LevelManager.LoadLevel(0);
    }

    public void SAIR (){
      //Debug.Log("Sair");
      Application.Quit();
    }
}
