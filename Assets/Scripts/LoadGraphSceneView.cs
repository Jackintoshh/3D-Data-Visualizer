using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadGraphSceneView : MonoBehaviour
{
    GameObject button;
    // Start is called before the first frame update
    void Start()
    {
        button = this.gameObject;
        Button buttonOb = button.GetComponent<Button>();
        buttonOb.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        //PreviewData.dropdown
        SceneManager.LoadScene("test2");
    }
}
