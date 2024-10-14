using Game;
using Game.Alien;
using Godot;
using Game.Bus;
using System;
using Util.ExtensionMethods;

public class MothershipManager : Node, IGameManager
{
    private IMothershipControl _mothershipController;
    private Timer _mothershipTimer;

    private const float _minimumSpawnTime = 20.0f;
    private const float _maximumSpawnTime = 30.0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetNodeReferences();
        CheckNodeReferences();
        SetNodeConnections();
    }

    private void SetNodeReferences()
    {
        _mothershipController = GetNode<IMothershipControl>("Mothership");
        _mothershipTimer = GetNode<Timer>("Timer");
    }

    private void CheckNodeReferences()
    {
        if (!(_mothershipController is Node mothershipNode))
        {
            GD.PrintErr("ERROR: Mothership is not a node!");
        }
        else if (!mothershipNode.IsValid())
        {
            GD.PrintErr("ERROR: Mothership node is not valid!");
        }

        if (!_mothershipTimer.IsValid())
        {
            GD.PrintErr("ERROR: Mothership timer node is not valid!");
        }
    }

    private void SetNodeConnections()
    {
        PlayerEventBus.Instance.Connect("PlayerShot", this, "OnPlayerShot");
        PlayerEventBus.Instance.Connect("PlayerRespawn", this, "OnPlayerRespawn");
    }

    public bool StartGame()
    {
        if (!(_mothershipController is Node mothershipNode) || !mothershipNode.IsValid()) { return false; }
        if (!_mothershipTimer.IsValid()) { return false; }

        StartSpawnTimer();

        return true;
    }

    public bool EndGame()
    {
        if (!(_mothershipController is Node mothershipNode) || !mothershipNode.IsValid()) { return false; }
        if (!_mothershipTimer.IsValid()) { return false; }

        _mothershipController.ResetShip();
        _mothershipTimer.Stop();

        return true;
    }

    private void StartSpawnTimer()
    {
        float spawnTime = (float)GD.RandRange(_minimumSpawnTime, _maximumSpawnTime);

        if (_mothershipTimer.IsValid())
        {
            _mothershipTimer.Start(spawnTime);
        }
    }

    public void OnMothershipTimerTimeout()
    {
        if (_mothershipController is Node mothershipNode && mothershipNode.IsValid())
        {
            _mothershipController.StartShip();
        }
    }

    public void OnPlayerShot()
    {
        if (_mothershipController is Node mothershipNode && mothershipNode.IsValid())
        {
            _mothershipController.ResetShip();
        }
        if (_mothershipTimer.IsValid())
        {
            _mothershipTimer.Stop();
        }
    }

    public void OnPlayerRespawn()
    {
        StartSpawnTimer();
    }
}
