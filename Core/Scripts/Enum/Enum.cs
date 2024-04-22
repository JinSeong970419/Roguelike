namespace Roguelike.Core
{
    public enum ImageExtension
    {
        PNG,
        JPG,
    }

    public enum GameWorldDimension
    {
        Two,
        Three,
    }
    public enum DefaultDirection2D
    {
        Left,
        Right,
    }

    public enum Team
    {
        Neutral,
        Alliance,
        Enemy,
    }

    public enum WeaponType
    {
        Normal,
        Projectile,
        Bomb,
        Close,
    }

    public enum Level
    {
        _01,
        _02,
        _03,
        _04,
        _05,
        _06,
        _07,
        _08,
        _09,
        _10,
        End,
    }

    public enum Aim
    {
        Normal,
        Auto,
        Random,
    }

    public enum Grade
    {
        Normal,
        Rare,
        Epic,
        Unique,
        Legendary,
    }

}
