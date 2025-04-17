using System.Drawing;
using System.Windows.Forms;
using PACMAN_GAME.Interfaces;

namespace PACMAN_GAME.Models;

public abstract class GameEntity : IGameEntity
{
    public PictureBox View { get; set; }
    
    public bool IsVisible 
    { 
        get => View.Visible; 
        set => View.Visible = value; 
    }
    
    public int X 
    { 
        get => View.Left; 
        set => View.Left = value; 
    }
    
    public int Y 
    { 
        get => View.Top;
        set => View.Top = value; 
    }
    
    public int Width => View.Width;
    public int Height => View.Height;
    public Rectangle Bounds => View.Bounds;

    protected GameEntity(PictureBox view)
    {
        View = view;
    }
} 