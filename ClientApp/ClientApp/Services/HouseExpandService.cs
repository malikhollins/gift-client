using ClientApp.Models;

namespace ClientApp.Services;

public class HouseExpandService
{
    private readonly HashSet<int> _expanded = new();

    public bool IsExpanded(int houseId) => _expanded.Contains(houseId);

    public void Set(int houseId, bool expanded)
    {
        if (expanded) _expanded.Add(houseId);
        else _expanded.Remove(houseId);
    }

    public void ApplyTo(House house) => house.IsExpanded = IsExpanded(house.Id);
}
