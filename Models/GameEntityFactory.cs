using System.Windows.Forms;
using PACMAN_GAME.Interfaces;
using PACMAN_GAME.Properties;

namespace PACMAN_GAME.Models;

public class GameEntityFactory
{
    private readonly Form _parent;
    private readonly ISoundManager _soundManager;

    public GameEntityFactory(Form parent, ISoundManager soundManager)
    {
        _parent = parent;
        _soundManager = soundManager;
    }

    public IGameEntity CreateGameEntity(string entityType, PictureBox view, int value = 1)
    {
        return entityType switch
        {
            "pacman" => CreatePacman(view),
            "redGhost" => CreateRedGhost(view),
            "yellowGhost" => CreateYellowGhost(view),
            "pinkGhost" => CreatePinkGhost(view),
            "coin" => CreateCoin(view, value),
            "wall" => CreateWall(view),
            _ => throw new ArgumentException($"Unknown entity type: {entityType}")
        };
    }

    private PACMAN_GAME.Pacman CreatePacman(PictureBox view)
    {
        return new PACMAN_GAME.Pacman(view, _parent, _soundManager);
    }

    private PACMAN_GAME.RedGhost CreateRedGhost(PictureBox view)
    {
        return new PACMAN_GAME.RedGhost(view, _parent);
    }

    private PACMAN_GAME.YellowGhost CreateYellowGhost(PictureBox view)
    {
        return new PACMAN_GAME.YellowGhost(view, _parent);
    }

    private PACMAN_GAME.PinkGhost CreatePinkGhost(PictureBox view)
    {
        return new PACMAN_GAME.PinkGhost(view, _parent);
    }

    private PACMAN_GAME.Coin CreateCoin(PictureBox view, int value)
    {
        return new PACMAN_GAME.Coin(view, value);
    }

    private PACMAN_GAME.Wall CreateWall(PictureBox view)
    {
        return new PACMAN_GAME.Wall(view);
    }
} 