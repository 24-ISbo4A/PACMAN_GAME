using System.Windows.Forms;
using PACMAN_GAME.Interfaces;
using PACMAN_GAME.Properties;

namespace PACMAN_GAME.Models;

/// <summary>
/// Фабрика для создания игровых сущностей в игре PACMAN.
/// Реализует паттерн "Фабричный метод" для инкапсуляции логики создания объектов.
/// </summary>
public class GameEntityFactory
{
    private readonly Form _parent;
    private readonly ISoundManager _soundManager;
    
    /// <summary>
    /// Инициализирует новый экземпляр фабрики игровых сущностей.
    /// </summary>
    /// <param name="parent">Родительская форма, на которой будут отображаться сущности.</param>
    /// <param name="soundManager">Менеджер звуков для воспроизведения звуковых эффектов.</param>
    public GameEntityFactory(Form parent, ISoundManager soundManager)
    {
        _parent = parent;
        _soundManager = soundManager;
    }

    /// <summary>
    /// Создает игровую сущность указанного типа.
    /// </summary>
    /// <param name="entityType">Тип создаваемой сущности (pacman, redGhost, yellowGhost, pinkGhost, coin, wall).</param>
    /// <param name="view">PictureBox, представляющий визуальное отображение сущности.</param>
    /// <param name="value">Значение для сущностей, которые его поддерживают (например, монеты). По умолчанию 1.</param>
    /// <returns>Созданная игровая сущность, реализующая интерфейс IGameEntity.</returns>
    /// <exception cref="ArgumentException">Выбрасывается, если передан неизвестный тип сущности.</exception>
    public IGameEntity CreateGameEntity(string entityType, PictureBox view, int value = 1)
    {
        return entityType switch
        {
            "pacman" => CreatePacman(view),
            "redGhost" => CreateRedGhost(view),
            "yellowGhost" => CreateYellowGhost(view),
            "pinkGhost" => CreatePinkGhost(view),
            "coin" => CreateCoin(view, value),
            "wall" => CreateWall(view),
            _ => throw new ArgumentException($"Unknown entity type: {entityType}")
        };
    }

    /// <summary>
    /// Создает экземпляр Пакмана.
    /// </summary>
    /// <param name="view">PictureBox для отображения Пакмана.</param>
    /// <returns>Новый экземпляр класса Pacman.</returns>
    private PACMAN_GAME.Pacman CreatePacman(PictureBox view)
    {
        return new PACMAN_GAME.Pacman(view, _parent, _soundManager);
    }

    /// <summary>
    /// Создает экземпляр красного призрака.
    /// </summary>
    /// <param name="view">PictureBox для отображения призрака.</param>
    /// <returns>Новый экземпляр класса RedGhost.</returns>
    private PACMAN_GAME.RedGhost CreateRedGhost(PictureBox view)
    {
        return new PACMAN_GAME.RedGhost(view, _parent);
    }

    /// <summary>
    /// Создает экземпляр желтого призрака.
    /// </summary>
    /// <param name="view">PictureBox для отображения призрака.</param>
    /// <returns>Новый экземпляр класса YellowGhost.</returns>
    private PACMAN_GAME.YellowGhost CreateYellowGhost(PictureBox view)
    {
        return new PACMAN_GAME.YellowGhost(view, _parent);
    }

    /// <summary>
    /// Создает экземпляр розового призрака.
    /// </summary>
    /// <param name="view">PictureBox для отображения призрака.</param>
    /// <returns>Новый экземпляр класса PinkGhost.</returns>
    private PACMAN_GAME.PinkGhost CreatePinkGhost(PictureBox view)
    {
        return new PACMAN_GAME.PinkGhost(view, _parent);
    }

    /// <summary>
    /// Создает экземпляр монеты.
    /// </summary>
    /// <param name="view">PictureBox для отображения монеты.</param>
    /// <param name="value">Значение монеты в очках.</param>
    /// <returns>Новый экземпляр класса Coin.</returns>
    private PACMAN_GAME.Coin CreateCoin(PictureBox view, int value)
    {
        return new PACMAN_GAME.Coin(view, value);
    }

    /// <summary>
    /// Создает экземпляр стены.
    /// </summary>
    /// <param name="view">PictureBox для отображения стены.</param>
    /// <returns>Новый экземпляр класса Wall.</returns>
    private PACMAN_GAME.Wall CreateWall(PictureBox view)
    {
        return new PACMAN_GAME.Wall(view);
    }
} 
