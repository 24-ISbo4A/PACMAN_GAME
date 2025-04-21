using System.Windows.Forms;
using PACMAN_GAME.Interfaces;

namespace PACMAN_GAME.Models;
/// <summary>
/// Представляет монету, которую может собирать игрок в игре PACMAN.
/// Наследуется от базового класса GameEntity.
/// </summary>
public class Coin : GameEntity
{
     /// <summary>
    /// Получает значение монеты в очках.
    /// </summary>
    /// <value>Количество очков, которое дает монета при сборе. По умолчанию 1.</value>
    public int Value { get; }
    /// <summary>
    /// Инициализирует новый экземпляр класса Coin с указанным изображением и значением.
    /// </summary>
    /// <param name="view">Элемент PictureBox, представляющий визуальное отображение монеты.</param>
    /// <param name="value">Значение монеты в очках. По умолчанию равно 1.</param>
    public Coin(PictureBox view, int value = 1) : base(view)
    {
        Value = value;
    }
} 
