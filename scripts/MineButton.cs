using System.Numerics;
using System.Windows.Controls;

public class MineButton
{
    public Button buttonRef;
    public Vector2 pos;

    public bool isMine = false;
    public int neigbouringMine = 0;

    public MineButton(Button button, Vector2 pos)
    {
        buttonRef = button;
        this.pos = pos;
    }
}