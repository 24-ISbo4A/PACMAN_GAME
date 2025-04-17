using System.Windows.Forms;
using PACMAN_GAME.Interfaces;
using PACMAN_GAME.Models;

namespace PACMAN_GAME.Managers;

public class InputHandler : IInputHandler
{
    private readonly GameManager _gameManager;
    private readonly Pacman _pacman;
    
    public InputHandler(GameManager gameManager, Pacman pacman)
    {
        _gameManager = gameManager;
        _pacman = pacman;
    }
    
    public void HandleKeyDown(KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            Application.Exit();
            return;
        }
        
        if (_gameManager.IsInMenu)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    _gameManager.SelectMenuOption(-1);
                    break;
                case Keys.Down:
                    _gameManager.SelectMenuOption(1);
                    break;
                case Keys.Enter:
                    _gameManager.ExecuteSelectedMenuOption();
                    break;
            }
            return;
        }
        
        if (e.KeyCode == Keys.R)
        {
            _gameManager.ResetGame();
            return;
        }
        
        if (_gameManager.IsGameOver) return;
        
        switch (e.KeyCode)
        {
            case Keys.Up:
                _pacman.SetDirection(0);
                break;
            case Keys.Down:
                _pacman.SetDirection(2);
                break;
            case Keys.Left:
                _pacman.SetDirection(3);
                break;
            case Keys.Right:
                _pacman.SetDirection(1);
                break;
        }
    }
    
    public void HandleKeyUp(KeyEventArgs e)
    {
        // No specific actions on key up in this game
    }
} 