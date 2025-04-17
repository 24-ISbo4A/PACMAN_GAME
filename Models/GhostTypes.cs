using System.Windows.Forms;
using PACMAN_GAME.Properties;

namespace PACMAN_GAME.Models;

public class RedGhost : Ghost
{
    public RedGhost(PictureBox view, Form parent) : base(view, parent) { }
    
    protected override void SetNormalImage()
    {
        View.Image = Resources.red_left;
    }
}

public class YellowGhost : Ghost
{
    public YellowGhost(PictureBox view, Form parent) : base(view, parent) { }
    
    protected override void SetNormalImage()
    {
        View.Image = Resources.yellow_right;
    }
}

public class PinkGhost : Ghost
{
    public PinkGhost(PictureBox view, Form parent) : base(view, parent) { }
    
    protected override void SetNormalImage()
    {
        View.Image = Resources.pink_left;
    }
} 