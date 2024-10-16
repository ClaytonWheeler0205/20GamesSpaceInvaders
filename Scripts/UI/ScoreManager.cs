using Game.Bus;
using Godot;
using System;
using Util.ExtensionMethods;

public class ScoreManager : Control
{
    private Label _scoreLabelReference;
    private Label _highScoreReference;

    private int _score = 0;
    private int _highScore;
    private int _pointsToNextLife = 1500;
    private const int POINTS_TO_NEXT_LIFE_INCREMENT = 1500;

    private ConfigFile _config = new ConfigFile();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetNodeReferences();
        CheckNodeReferences();
        SetNodeConnections();
        LoadHighScore();
    }

    private void SetNodeReferences()
    {
        _scoreLabelReference = GetNode<Label>("ScoreLabel");
        _highScoreReference = GetNode<Label>("HighScoreLabel");
    }

    private void CheckNodeReferences()
    {
        if (!_scoreLabelReference.IsValid())
        {
            GD.PrintErr("ERROR: Score label reference is not valid!");
        }
        if (!_highScoreReference.IsValid())
        {
            GD.PrintErr("ERROR: High score label reference is not valid!");
        }
    }

    private void SetNodeConnections()
    {
        ScoreEventBus.Instance.Connect("AwardPoints", this, "OnAwardPoints");
    }

    private void LoadHighScore()
    {
        Error err = _config.Load("user://highscore.cfg");

        if (err != Error.Ok)
        {
            _highScore = 0;
            return;
        }

        _highScore = (int)_config.GetValue("PlayerScore", "high_score");
        UpdateHighScore();
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

    private void UpdateHighScore()
    {
        if (_highScoreReference.IsValid())
        {
            if (_highScore < 10)
            {
                _highScoreReference.Text = $"High Score: 000{_highScore}";
            }
            else if (_highScore < 100)
            {
                _highScoreReference.Text = $"High Score: 00{_highScore}";
            }
            else if (_highScore < 1000)
            {
                _highScoreReference.Text = $"High Score: 0{_highScore}";
            }
            else
            {
                _highScoreReference.Text = $"High Score: {_highScore}";
            }
        }
        SaveHighScore();
    }

    private void SaveHighScore()
    {
        _config.SetValue("PlayerScore", "high_score", _highScore);

        _config.Save("user://highscore.cfg");
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
        if (_score > _highScore)
        {
            _highScore = _score;
            UpdateHighScore();
        }
    }
}
