using System.Windows.Forms;

namespace PACMAN_GAME.Interfaces;

public interface IInputHandler
{
    void HandleKeyDown(KeyEventArgs e);
    void HandleKeyUp(KeyEventArgs e);
} 