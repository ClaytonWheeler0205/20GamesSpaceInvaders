using Game.Bus;
using Godot;
using System;
using Util.ExtensionMethods;

public class ScoreManager : Control
{
    private Label _scoreLabelReference;

    private int _score = 0;
    private int _pointsToNextLife = 1500;
    private const int POINTS_TO_NEXT_LIFE_INCREMENT = 1500;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetNodeReferences();
        CheckNodeReferences();
        SetNodeConnections();
    }

    private void SetNodeReferences()
    {
        _scoreLabelReference = GetNode<Label>("ScoreLabel");
    }

    private void CheckNodeReferences()
    {
        if (!_scoreLabelReference.IsValid())
        {
            GD.PrintErr("ERROR: Score label reference is not valid!");
        }
    }

    private void SetNodeConnections()
    {
        ScoreEventBus.Instance.Connect("AwardPoints", this, "OnAwardPoints");
    }

    private void UpdateScore()
    {
        if (_scoreLabelReference.IsValid())
        {
            if (_score < 10)
            {
                _scoreLabelReference.Text = $"Score: 000{_score}";
            }
            else if (_score < 100)
            {
                _scoreLabelReference.Text = $"Score: 00{_score}";
            }
            else if (_score < 1000)
            {
                _scoreLabelReference.Text = $"Score: 0{_score}";
            }
            else
            {
                _scoreLabelReference.Text = $"Score: {_score}";
            }
        }
    }

    public void OnAwardPoints(int pointsToGive)
    {
        _score += pointsToGive;
        UpdateScore();
        if (_score >= _pointsToNextLife)
        {
            LivesEventBus.Instance.EmitSignal("GainLife");
            // Play a life up sound effect
            _pointsToNextLife += POINTS_TO_NEXT_LIFE_INCREMENT;
        }
    }
}
