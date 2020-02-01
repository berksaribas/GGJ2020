using System;
using UnityEngine;

public enum Timespace
{
    PAST, FUTURE, SIMULATION
}
public class TimeManager : MonoBehaviour
{
    public GameObject PastContainer;
    public Camera MainCamera;
    
    private GameObject _futureContainer;
    private ActionCommand[] _futureActionCommands;
    private Timespace _currentSpace;
    private int _futureLayer, _pastLayer, _playerLayer;
    private float _simulationStartTime = 0f;
    
    private void Awake()
    {
        _futureLayer = LayerMask.NameToLayer("FutureLayer");
        _pastLayer = LayerMask.NameToLayer("PastLayer");
        _playerLayer = LayerMask.NameToLayer("PlayerLayer");
        
        _futureContainer = Instantiate(PastContainer);
        _futureActionCommands = _futureContainer.GetComponentsInChildren<ActionCommand>();
        
        _futureContainer.name = "FutureContainer";
        Utils.SetLayerRecursively(_futureContainer, _futureLayer);
        ChangeState(Timespace.FUTURE);
    }

    public void ChangeState(Timespace state)
    {
        switch (state)
        {
            case Timespace.FUTURE:
                Simulate();
                break;
            case Timespace.PAST:
                MakeUserInteractWithPastLayer();
                Time.timeScale = 1;
                MainCamera.cullingMask &=  ~(1 << _futureLayer); //hide past layer
                _currentSpace = state;
                break;
        }
    }

    private void Update()
    {
        if (_currentSpace == Timespace.SIMULATION)
        {
            if (Time.realtimeSinceStartup > _simulationStartTime + 1f)
            {
                OnSimulationConcluded();
            }
        }
    }

    private void Simulate()
    {
        _simulationStartTime = Time.realtimeSinceStartup;
        _currentSpace = Timespace.SIMULATION;
        Time.timeScale = 100f;
        foreach (var futureActionCommand in _futureActionCommands)
        {
            futureActionCommand.OnCommand();
        }
    }

    private void OnSimulationConcluded()
    {
        Time.timeScale = 1f;
        _currentSpace = Timespace.FUTURE;
        MakeUserInteractWithFutureLayer();
        MainCamera.cullingMask &=  ~(1 << _pastLayer); //hide past layer
    }

    private void MakeUserInteractWithPastLayer()
    {
        Physics.IgnoreLayerCollision(_playerLayer, _futureLayer, true);
        Physics.IgnoreLayerCollision(_playerLayer, _pastLayer, false);
    }
    
    private void MakeUserInteractWithFutureLayer()
    {
        Physics.IgnoreLayerCollision(_playerLayer, _futureLayer, false);
        Physics.IgnoreLayerCollision(_playerLayer, _pastLayer, true);
    }
}