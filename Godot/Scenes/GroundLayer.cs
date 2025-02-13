using Godot;
using System;

public partial class GroundLayer : TileMapLayer
{

	[Export]
	public TileMapLayer Obstacles
	{
		get; set;
	}

	public override bool _UseTileDataRuntimeUpdate(Vector2I coords)
	{
		return Obstacles.GetCellTileData(coords) is not null;
	}

	public override void _TileDataRuntimeUpdate(Vector2I coords, TileData tileData)
	{
		if(Obstacles.GetCellTileData(coords) is not null)
		{
			tileData.SetNavigationPolygon(0, null);
		}
	}
}
