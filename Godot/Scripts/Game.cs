using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Game: Node
{
	private GameLevel? _activeLevel;
	private bool _isGameOver;

	public event Action<GameLevel?>? LevelChanging;
	public event Action<GameLevel?>? LevelChanged;

	public event Action<bool>? GameOverChanged;

	[Export]
	public GameLevel? ActiveLevel
	{
		get => _activeLevel;
		set
		{
			LevelChanging?.Invoke(value);
			_activeLevel = value;
			LevelChanged?.Invoke(_activeLevel);
		}
	}

	[Export]
	public bool IsGameOver
	{
		get => _isGameOver;
		set
		{
			_isGameOver = value;
			GameOverChanged?.Invoke(_isGameOver);
		}
	}

}
