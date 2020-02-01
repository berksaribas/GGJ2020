using System;
using UnityEngine;

public enum Timespace
{
    PAST, FUTURE, SIMULATION, GOBACK
}
public class TimeManager : MonoBehaviour
{
    public GameObject PastContainer;
    public Rigidbody[] PastRigidbodies;
    public Camera MainCamera;
    
    private GameObject _futureContainer;
    private ActionCommand[] _futureActionCommands;
    
    private Timespace _currentSpace;
    private int _futureLayer, _pastLayer, _playerLayer;
    private float _stateChangeTime = 0f;
    
    private void Awake()
    {
        _futureLayer = LayerMask.NameToLayer("FutureLayer");
        _pastLayer = LayerMask.NameToLayer("PastLayer");
        _playerLayer = LayerMask.NameToLayer("PlayerLayer");
    }

    private void Start()
    {
        OnGoBackConcluded();
        
        TransitionController.Instance.FadeToTransition(1f, 1f, 1f, 0f, () =>
        {
            ChangeState(Timespace.FUTURE, true);
            TransitionController.Instance.FadeToTransition(1f, 1f, 1f, 1f);
        });
    }

    public void ChangeState(Timespace state, bool playTransition = true)
    {
        if(playTransition)
        {
            TransitionController.Instance.StartTimeTransition();
        }
        
        switch (state)
        {
            case Timespace.FUTURE:
                Simulate();
                break;
            case Timespace.PAST:
                _currentSpace = Timespace.GOBACK;
                break;
        }

        _stateChangeTime = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        if (_currentSpace == Timespace.SIMULATION)
        {
            for(var i = 0; i < 50; i++)
            {
                Physics.Simulate(Time.fixedDeltaTime);
            }
        }
        
        if (Time.realtimeSinceStartup > _stateChangeTime + 1f)
        {
            if (_currentSpace == Timespace.SIMULATION)
            {
                OnSimulationConcluded();
            }
            else if(_currentSpace == Timespace.GOBACK)
            {
                OnGoBackConcluded();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(_currentSpace == Timespace.FUTURE)
            {
                ChangeState(Timespace.PAST);
            }
            else if (_currentSpace == Timespace.PAST)
            {
                ChangeState(Timespace.FUTURE);
            }
        }
    }

    private void Simulate()
    {
        PastRigidbodies = PastContainer.GetComponentsInChildren<Rigidbody>();

        foreach (var pastRigidbody in PastRigidbodies)
        {
            pastRigidbody.velocity = Vector3.zero;
            pastRigidbody.angularVelocity = Vector3.zero;
        }
        
        _futureContainer = Instantiate(PastContainer);
        _futureActionCommands = _futureContainer.GetComponentsInChildren<ActionCommand>();
        _futureContainer.name = "FutureContainer";
        Utils.SetLayerRecursively(_futureContainer, _futureLayer);
        
        _currentSpace = Timespace.SIMULATION;
        foreach (var futureActionCommand in _futureActionCommands)
        {
            futureActionCommand.OnCommand();
        }
        
        Physics.autoSimulation = false;
        SetSimulationInteractions();
    }

    private void OnSimulationConcluded()
    {
        Physics.autoSimulation = true;
        _currentSpace = Timespace.FUTURE;
        SetFutureInteractions();
        ToggleCulling();
    }

    private void OnGoBackConcluded()
    {
        if (_futureContainer != null)
        {
            Destroy(_futureContainer);
        }
        SetPastInteractions();
        _currentSpace = Timespace.PAST;
        ToggleCulling();
    }

    private void ToggleCulling()
    {
        switch (_currentSpace)
        {
            case Timespace.FUTURE:
                MainCamera.cullingMask &=  ~(1 << _pastLayer); //hide past layer
                MainCamera.cullingMask |= 1 << _futureLayer; //show future layer
                break;
            case Timespace.PAST:
                MainCamera.cullingMask &=  ~(1 << _futureLayer); //hide future layer
                MainCamera.cullingMask |= 1 << _pastLayer; //show past layer
                break;
        }
    }

    private void SetPastInteractions()
    {
        Physics.IgnoreLayerCollision(_playerLayer, _futureLayer, false);
        Physics.IgnoreLayerCollision(_playerLayer, _pastLayer, false);
    }

    private void SetSimulationInteractions()
    {
        Physics.IgnoreLayerCollision(_playerLayer, _futureLayer, true);
        Physics.IgnoreLayerCollision(_playerLayer, _pastLayer, true);
    }
    
    private void SetFutureInteractions()
    {
        Physics.IgnoreLayerCollision(_playerLayer, _futureLayer, false);
        Physics.IgnoreLayerCollision(_playerLayer, _pastLayer, true);
    }
}