using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapper : MonoBehaviour
{
    public bool isManager = false;

    public string SwapToScene;

    public string[] LoadScenes;
    public string[] UnloadScenes;

    public LoadSceneMode loadMode = LoadSceneMode.Additive;

    public string MySceneName;

    private void Start()
    {
        MySceneName = gameObject.scene.name;
        if(isManager)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(MySceneName));
            SceneManager.LoadSceneAsync("PersistantObjects", LoadSceneMode.Additive);
        }
    }

    public void SwapObjects(GameObject triggerObject)
    {
        if (MySceneName == SceneManager.GetActiveScene().name)
        {
            Debug.Log("Swapping");
            AsyncOperation op = SceneManager.LoadSceneAsync(SwapToScene, loadMode);
            op.completed += Op_completed;
            UpdateAdjacentScenes();
        }
    }
    void Op_completed(AsyncOperation obj)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(SwapToScene));
    }

    private void UpdateAdjacentScenes()
    {
        {
            foreach (string s in LoadScenes)
            {
                if (s != SwapToScene)
                    SceneManager.LoadSceneAsync(s, loadMode);
            }

            foreach (string s in UnloadScenes)
            {
                SceneManager.UnloadSceneAsync(s);
            }
        }
    }
}
