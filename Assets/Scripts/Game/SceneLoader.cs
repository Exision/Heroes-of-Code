using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : SingletonMonoBehaviour<SceneLoader>
{
    Coroutine loadSceneAsyncCoroutine = null;

    public string PrevScenePath { get; private set; }
    public string CurrScenePath { get; set; }

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode loadSceneMode) =>
        {
            SceneManager.SetActiveScene(scene);
        };
    }

    public void LoadScene(string scenePath, bool async = true)
    {
        if (loadSceneAsyncCoroutine != null)
            return;

        PrevScenePath = CurrScenePath;
        CurrScenePath = scenePath;

        if (async)
            loadSceneAsyncCoroutine = StartCoroutine(LoadSceneAsync());
        else
            LoadScene();
    }

    void LoadScene()
    {
        SceneManager.LoadScene(CurrScenePath, LoadSceneMode.Additive);
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncOperation;

        PreloaderWindow _preloaderWindow = WindowManager.Instance.GetWindow<PreloaderWindow>();

        if (!string.IsNullOrEmpty(PrevScenePath))
        {
            _preloaderWindow.Show();

            yield return null;

            while (!_preloaderWindow.IsLoaded)
                yield return null;

            asyncOperation = SceneManager.UnloadSceneAsync(PrevScenePath);

            while (!asyncOperation.isDone)
                yield return null;

            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }

        yield return new WaitForSecondsRealtime(0.5f);

        asyncOperation = SceneManager.LoadSceneAsync(CurrScenePath, LoadSceneMode.Additive);

        while (!asyncOperation.isDone)
            yield return null;

        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        yield return new WaitForSecondsRealtime(0.5f);

        _preloaderWindow.Hide();

        loadSceneAsyncCoroutine = null;

        yield break;
    }
}
