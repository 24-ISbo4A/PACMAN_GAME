using System.Drawing;
using System.Windows.Forms;
using PACMAN_GAME.Interfaces;

namespace PACMAN_GAME.Managers;

public class UIManager : IUIManager
{
    private readonly Form _parent;
    private readonly PictureBox _menuBackground;
    private readonly PictureBox _menuArrow;
    private readonly Label _gameOverLabel;
    private readonly Label _restartLabel;
    private readonly Label _scoreLabel;
    
    public UIManager(Form parent, PictureBox menuBackground, PictureBox menuArrow, Label gameOverLabel, Label restartLabel, Label scoreLabel)
    {
        _parent = parent;
        _menuBackground = menuBackground;
        _menuArrow = menuArrow;
        _gameOverLabel = gameOverLabel;
        _restartLabel = restartLabel;
        _scoreLabel = scoreLabel;
    }
    
    public void ShowMainMenu()
    {
        _menuBackground.Visible = true;
        _menuArrow.Visible = true;
        
        foreach (Control x in _parent.Controls)
        {
            if (x is PictureBox && x != _menuBackground && x != _menuArrow)
            {
                x.Visible = false;
            }
            
            if (x is Label && (x.Text == "PLAY" || x.Text == "QUIT"))
            {
                x.Visible = true;
            }
        }
        
        _scoreLabel.Visible = false;
    }
    
    public void HideMainMenu()
    {
        _menuBackground.Visible = false;
        _menuArrow.Visible = false;
        
        foreach (Control x in _parent.Controls)
        {
            if (x.Parent == _menuBackground)
            {
                x.Visible = false;
            }
            
            if (x is Label && (x.Text == "PLAY" || x.Text == "QUIT"))
            {
                x.Visible = false;
            }
        }
        
        _scoreLabel.Visible = true;
    }
    
    public void ShowGameOverScreen(string message)
    {
        if (message == "You Win!")
        {
            MessageBox.Show("You Win! Press R to restart or ESC to quit.");
            return;
        }
        
        foreach (Control x in _parent.Controls)
        {
            if (x is PictureBox && x != _menuBackground && x != _menuArrow)
            {
                x.Visible = false;
            }
        }
        
        _scoreLabel.Visible = false;
        
        _gameOverLabel.Location = new Point(
            (_parent.ClientSize.Width - _gameOverLabel.Width) / 2,
            (_parent.ClientSize.Height - _gameOverLabel.Height) / 2 - 100
        );
        _gameOverLabel.Visible = true;
        _gameOverLabel.BringToFront();
        
        _restartLabel.Location = new Point(
            (_parent.ClientSize.Width - _restartLabel.Width) / 2,
            _gameOverLabel.Bottom + 50
        );
        _restartLabel.Visible = true;
        _restartLabel.BringToFront();
    }
    
    public void UpdateScore(int score)
    {
        _scoreLabel.Text = "Score: " + score;
    }
    
    public void UpdateArrowPosition(int selectedOption)
    {
        int baseX = _menuBackground.Width / 2 + 250;
        int baseY = _menuBackground.Bottom - 290;
        int spacing = 100;
        
        _menuArrow.Location = new Point(baseX, baseY + selectedOption * spacing);
        _menuArrow.BringToFront();
    }
    
    public void HideGameOverScreen()
    {
        _gameOverLabel.Visible = false;
        _restartLabel.Visible = false;
    }
} 