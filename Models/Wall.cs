using System.Windows.Forms;
using PACMAN_GAME.Interfaces;

namespace PACMAN_GAME.Models;

/// <summary>
/// Класс, представляющий стену (препятствие) в игре PACMAN.
/// Наследует функциональность от базового класса GameEntity.
/// </summary>
/// <remarks>
/// Стены являются непроходимыми препятствиями для Pacman и призраков.
/// При столкновении с ними движение становится невозможным.
/// </remarks>
public class Wall : GameEntity
{
    /// <summary>
    /// Инициализирует новый экземпляр стены.
    /// </summary>
    /// <param name="view">Элемент PictureBox, представляющий визуальное отображение стены.</param>
    /// <remarks>
    /// Конструктор просто передает PictureBox в базовый класс GameEntity.
    /// Для корректной работы стены, связанный PictureBox должен иметь Tag со значением "wall".
    /// </remarks>
    public Wall(PictureBox view) : base(view) { }
}
