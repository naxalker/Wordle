using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour
{
    [SerializeField]
    private AssetReference _mainSceneReference;

    private AsyncOperationHandle<SceneInstance> _loadOperation;

    void Start()
    {
        LoadMainScene();
    }

    private async void LoadMainScene()
    {
        _loadOperation = Addressables.LoadSceneAsync(_mainSceneReference, LoadSceneMode.Single);
        await _loadOperation.Task;
    }
}
