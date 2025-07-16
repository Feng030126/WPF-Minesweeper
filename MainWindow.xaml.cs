using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    IRandom rng;

    int mf_row = 8;
    int mf_col = 8;

    int singleGridSize = 32;

    int mineCount = 8;

    bool fieldInitialized = false;

    Dictionary<Vector2, MineButton> minefieldMap = new();
    List<MineButton> mineButtons = new();

    readonly HashSet<Vector2> revealed = new();

    readonly List<Vector2> neighbouringVectors = new()
    {
        new Vector2(-1, -1),
        new Vector2( 0, -1),
        new Vector2( 1, -1),
        new Vector2(-1,  0),
        new Vector2( 1,  0),
        new Vector2(-1,  1),
        new Vector2( 0,  1),
        new Vector2( 1,  1),
    };

    public MainWindow()
    {
        InitializeComponent();
        Mode.SelectedIndex = 0;
        InitializeEmptyMineField();
    }

    private void InitializeEmptyMineField()
    {
        minefieldMap = new();
        mineButtons = new();
        MineField.RowDefinitions.Clear();
        MineField.ColumnDefinitions.Clear();
        MineField.Children.Clear();

        for (int row = 0; row < mf_row; row++)
        {
            // Add row definition
            MineField.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(singleGridSize, GridUnitType.Pixel)
            });

            for (int col = 0; col < mf_col; col++)
            {
                // Only add column definitions once, in the first row loop
                if (row == 0)
                {
                    MineField.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = new GridLength(singleGridSize, GridUnitType.Pixel)
                    });
                }

                // Add button
                var btn = new Button();
                Grid.SetRow(btn, row);
                Grid.SetColumn(btn, col);

                Vector2 pos = new(row, col);

                btn.Click += (s, e) => MineFieldButton_Click(pos);

                minefieldMap.Add(pos, new MineButton(btn, pos));
                mineButtons.Add(minefieldMap[pos]);

                MineField.Children.Add(btn);
            }
        }

    }

    private void Difficulty_Changed(object sender, SelectionChangedEventArgs e)
    {
        var diff = ((sender as ComboBox)?.SelectedItem as ComboBoxItem)?.Content;

        if (diff.ToString() == "Easy")
        {
            mf_col = 8;
            mf_row = 8;
            singleGridSize = 32;
            mineCount = 10;
        }
        else if (diff.ToString() == "Medium")
        {
            mf_col = 16;
            mf_row = 16;
            singleGridSize = 18;
            mineCount = 40;
        }
        else if (diff.ToString() == "Hard")
        {
            mf_col = 16;
            mf_row = 16;
            singleGridSize = 18;
            mineCount = 60;
        }

        InitializeEmptyMineField();
        fieldInitialized = false;
    }

    private void MineFieldButton_Click(Vector2 pos)
    {
        if (!fieldInitialized)
        {
            InitializeMineField(pos);
        }

        if (!minefieldMap.TryGetValue(pos, out var mb))
        {
            return;
        }

        MineButton mineButton = minefieldMap[pos];

        if (mineButton.isMine)
        {
            mineButton.buttonRef.Content = "*";

            RevealAll();

            MessageBox.Show("YOU LOSE");

            InitializeEmptyMineField();
            fieldInitialized = false;
        }
        else
        {
            mineButton.buttonRef.Content = mineButton.neigbouringMine;

            if (mineButton.neigbouringMine == 0)
            {
                RevealNeighbour(mineButton);
            }
        }
    }

    private void RevealAll()
    {
        foreach (var tile in mineButtons)
        {
            tile.buttonRef.Content = tile.isMine ? "*" : tile.neigbouringMine;
        }
    }

    private void RevealNeighbour(MineButton mb)
    {
        if (revealed.Contains(mb.pos)) return;

        revealed.Add(mb.pos);
        mb.buttonRef.Content = mb.neigbouringMine;

        if (mb.neigbouringMine != 0)
            return;

        foreach (var offset in neighbouringVectors)
        {
            Vector2 neighPos = mb.pos + offset;

            if (minefieldMap.TryGetValue(neighPos, out var t))
            {
                t.buttonRef.Content = t.neigbouringMine;

                if (t.neigbouringMine == 0)
                {
                    RevealNeighbour(t);
                }
            }
        }
    }

    private void InitializeMineField(Vector2 firstClick)
    {
        List<MineButton> mines = mineButtons.Sample(mineCount, new List<MineButton> { minefieldMap[firstClick] });

        foreach (var mine in mines)
        {
            mine.isMine = true;
        }

        foreach (var tile in mineButtons)
        {
            foreach (var offset in neighbouringVectors)
            {
                Vector2 neighPos = tile.pos + offset;

                if (minefieldMap.TryGetValue(neighPos, out var t))
                {
                    if (t.isMine) tile.neigbouringMine++;
                }
            }
        }

        fieldInitialized = true;
    }

    private void OnRestartButton_Click(object sender, RoutedEventArgs e)
    {
        InitializeEmptyMineField();
        fieldInitialized = false;
    }
}