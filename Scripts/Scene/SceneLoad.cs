using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SceneLoad : MonoBehaviour
{
    public void TitleToNewGame()
    {
        LoadingSceneController.LoadScene("Tutorial_MWJ");
    }
}
