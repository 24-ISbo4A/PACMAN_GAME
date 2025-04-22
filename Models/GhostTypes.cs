using System.Windows.Forms;
using PACMAN_GAME.Properties;

namespace PACMAN_GAME.Models;

/// <summary>
/// Класс, представляющий красного призрака (Blinky) в игре PACMAN.
/// Наследует базовую функциональность от абстрактного класса Ghost.
/// </summary>
public class RedGhost : Ghost
{
    /// <summary>
    /// Инициализирует новый экземпляр красного призрака.
    /// </summary>
    /// <param name="view">Элемент PictureBox для отображения призрака.</param>
    /// <param name="parent">Родительская форма, содержащая игровое поле.</param>
    public RedGhost(PictureBox view, Form parent) : base(view, parent) { }
    
    /// <summary>
    /// Устанавливает стандартное изображение красного призрака.
    /// Реализация абстрактного метода базового класса Ghost.
    /// </summary>
    protected override void SetNormalImage()
    {
        View.Image = Resources.red_left;
    }
}

/// <summary>
/// Класс, представляющий желтого призрака (Clyde) в игре PACMAN.
/// Наследует базовую функциональность от абстрактного класса Ghost.
/// </summary>
public class YellowGhost : Ghost
{
    /// <summary>
    /// Инициализирует новый экземпляр желтого призрака.
    /// </summary>
    /// <param name="view">Элемент PictureBox для отображения призрака.</param>
    /// <param name="parent">Родительская форма, содержащая игровое поле.</param>
    public YellowGhost(PictureBox view, Form parent) : base(view, parent) { }
    
    /// <summary>
    /// Устанавливает стандартное изображение желтого призрака.
    /// Реализация абстрактного метода базового класса Ghost.
    /// </summary>
    protected override void SetNormalImage()
    {
        View.Image = Resources.yellow_right;
    }
}

/// <summary>
/// Класс, представляющий розового призрака (Pinky) в игре PACMAN.
/// Наследует базовую функциональность от абстрактного класса Ghost.
/// </summary>
public class PinkGhost : Ghost
{
    /// <summary>
    /// Инициализирует новый экземпляр розового призрака.
    /// </summary>
    /// <param name="view">Элемент PictureBox для отображения призрака.</param>
    /// <param name="parent">Родительская форма, содержащая игровое поле.</param>
    public PinkGhost(PictureBox view, Form parent) : base(view, parent) { }
    
    /// <summary>
    /// Устанавливает стандартное изображение розового призрака.
    /// Реализация абстрактного метода базового класса Ghost.
    /// </summary>
    protected override void SetNormalImage()
    {
        View.Image = Resources.pink_left;
    }
}
