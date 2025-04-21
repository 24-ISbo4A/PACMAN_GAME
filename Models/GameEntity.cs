using System.Drawing;
using System.Windows.Forms;
using PACMAN_GAME.Interfaces;

namespace PACMAN_GAME.Models;

/// <summary>
/// Абстрактный базовый класс для всех игровых сущностей в PACMAN.
/// Реализует интерфейс <see cref="IGameEntity"/>.
/// </summary>
public abstract class GameEntity : IGameEntity
{

    /// <summary>
    /// Получает или задает PictureBox, связанный с игровой сущностью.
    /// </summary>
    /// <value>Элемент управления, отображающий сущность на игровом поле.</value>
    public PictureBox View { get; set; }
    /// <summary>
    /// Получает или задает видимость игровой сущности.
    /// </summary>
    /// <value>true, если сущность видима; иначе false.</value>
    public bool IsVisible 
    { 
        get => View.Visible; 
        set => View.Visible = value; 
    }
    
    /// <summary>
    /// Получает или задает X-координату сущности на игровом поле.
    /// </summary>
    /// <value>Координата по горизонтали (Left свойство PictureBox).</value>
    public int X 
    { 
        get => View.Left; 
        set => View.Left = value; 
    }
    
    /// <summary>
    /// Получает или задает Y-координату сущности на игровом поле.
    /// </summary>
    /// <value>Координата по вертикали (Top свойство PictureBox).</value>
    public int Y 
    { 
        get => View.Top;
        set => View.Top = value; 
    }
    
    /// <summary>
    /// Получает ширину сущности.
    /// </summary>
    /// <value>Ширина связанного PictureBox.</value>
    public int Width => View.Width;
    
    /// <summary>
    /// Получает высоту сущности.
    /// </summary>
    /// <value>Высота связанного PictureBox.</value>
    public int Height => View.Height;
    
    /// <summary>
    /// Получает ограничивающий прямоугольник сущности.
    /// </summary>
    /// <value>Прямоугольник, описывающий границы PictureBox.</value>
    public Rectangle Bounds => View.Bounds;
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="GameEntity"/> с указанным PictureBox.
    /// </summary>
    /// <param name="view">Элемент PictureBox, представляющий визуальное отображение сущности.</param>
    protected GameEntity(PictureBox view)
    {
        View = view;
    }
} 
