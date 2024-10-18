using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreenController : MonoBehaviour
{
    public void RestartGame()
    {
        SceneController.LoadScene(0);
    }
}
