using System.Windows.Forms;
using PACMAN_GAME.Interfaces;

namespace PACMAN_GAME.Models;

public class Wall : GameEntity
{
    public Wall(PictureBox view) : base(view) { }
} 