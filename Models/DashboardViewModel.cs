namespace EvolutionPlayMobile.Models;

public sealed class DashboardViewModel
{
    public required GameState State { get; init; }
    public required IReadOnlyList<ShopItem> ShopItems { get; init; }
    public required IReadOnlyList<ShopItem> OwnedShopItems { get; init; }
    public required IReadOnlyList<PotionRecipe> Potions { get; init; }
    public required IReadOnlyList<CraftRecipe> CraftRecipes { get; init; }
    public required IReadOnlyList<PurchaseOption> PurchaseOptions { get; init; }
    public string? Message { get; init; }
    public string? Error { get; init; }
    public bool HasOwnedItems => OwnedShopItems?.Count > 0;
    public double XpProgress => State.Profile.XpToNext > 0 ? (double)State.Profile.Xp / State.Profile.XpToNext : 0;
    public string XpDisplay => $"{State.Profile.Xp}/{State.Profile.XpToNext}";
}