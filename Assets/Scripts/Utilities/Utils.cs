using System;

public enum GameLayer
{
    Default,
    Destructible,
    Player,
    Enemy,
    Ground
}

public class Utils
{
    public static GameLayer LayerNameToEnum(string layer) =>
        Enum.TryParse<GameLayer>(layer, out var layerEnum) ? layerEnum : GameLayer.Default;

    public static string EnumToLayerName(GameLayer layer) => layer.ToString();
}