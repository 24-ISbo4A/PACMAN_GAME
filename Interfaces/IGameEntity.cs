using System.Drawing;
using System.Windows.Forms;

namespace PACMAN_GAME.Interfaces;

public interface IGameEntity
{
    PictureBox View { get; set; }
    bool IsVisible { get; set; }
    int X { get; set; }
    int Y { get; set; }
    int Width { get; }
    int Height { get; }
    Rectangle Bounds { get; }
} 