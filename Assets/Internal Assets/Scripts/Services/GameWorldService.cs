using UnityEngine;
using Zenject;

public class GameWorldService
{
    private DiContainer _container;

    private GameWorldConfigScriptableObject _gameWorldConfig;

    private bool _isReady = false;

    public bool IsReady => _isReady;
    public GameWorldService(GameWorldConfigScriptableObject gameWorldConfig, DiContainer container)
    {
        _gameWorldConfig = gameWorldConfig;
        _container = container;
    }

    public void Init()
    {
        if (_isReady) return;
        FitCamera();
        _isReady = true;
    }

    private void FitCamera()
    {
        var cam = Camera.main;

        var screenRatio = (float)Screen.width / Screen.height;
        var targetRatio = _gameWorldConfig.WorldSize.x / _gameWorldConfig.WorldSize.y;

        if (screenRatio >= targetRatio)
        {
            cam.orthographicSize = _gameWorldConfig.WorldSize.y / 2f;
        }
        else
        {
            var difference = targetRatio / screenRatio;
            cam.orthographicSize = (_gameWorldConfig.WorldSize.y / 2f) * difference;
        }

        //cam.transform.position = new Vector3(
        //    _gameWorldConfig.WorldSize.x / 2f,
        //    _gameWorldConfig.WorldSize.y / 2f,
        //    cam.transform.position.z
        //);
    }

}
