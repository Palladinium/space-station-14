using System.Linq;
using System.Numerics;
using Content.Client.Stylesheets;
using Content.Client.UserInterface.Controls;
using Robust.Client.AutoGenerated;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;

using Robust.Shared.Utility;

namespace Content.Client.Dice;

/// <summary>
///     UI to set the roll of a loaded die.
/// </summary>
[GenerateTypedNameReferences]
public sealed partial class LoadedDiceMenu : FancyWindow
{
    public event Action<int?>? OnSideSelected;

    private IEnumerable<DiceSide> _possibleSides = Enumerable.Empty<DiceSide>();
    private int? _selectedSide;

    public LoadedDiceMenu()
    {
        RobustXamlLoader.Load(this);

        UnsetButton.OnPressed += _ => OnSideSelected?.Invoke(null);
    }

    public void UpdateState(IEnumerable<DiceSide> possibleSides, int? selectedSide)
    {
        _possibleSides = possibleSides;
        _selectedSide = selectedSide;

        UnsetButton.Pressed = _selectedSide == null;

        UpdateGrid();
    }

    private void UpdateGrid()
    {
        ClearGrid();

        var group = new ButtonGroup();

        foreach (var side in _possibleSides)
        {
            var button = new Button
            {
                HorizontalExpand = true,
                Group = group,
                ToggleMode = true,
                Pressed = _selectedSide == side.Index,
                Text = side.Value
            };
            button.OnPressed += _ => OnSideSelected?.Invoke(side.Index);
            Grid.AddChild(button);
        }
    }

    private void ClearGrid()
    {
        Grid.RemoveAllChildren();
    }
}

/// <summary>
///     Helper struct to avoid computing the value of a side in the UI.
///     For example, side 1 of a percentile die has the value "10".
/// </summary>
public struct DiceSide
{
    public int Index;
    public string Value;

    public DiceSide(int index, string value)
    {
        Index = index;
        Value = value;
    }
}
