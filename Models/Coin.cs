using System.Windows.Forms;
using PACMAN_GAME.Interfaces;

namespace PACMAN_GAME.Models;

public class Coin : GameEntity
{
    public int Value { get; }
    
    public Coin(PictureBox view, int value = 1) : base(view)
    {
        Value = value;
    }
} 