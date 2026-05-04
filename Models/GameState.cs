using System.Text.Json.Serialization;

namespace EvolutionPlayMobile.Models;

public enum WorkoutIntensity
{
    Light,
    Medium,
    Heavy
}

public enum PurchaseType
{
    Treino,
    Suco,
    Vitamina,
    Suplemento,
    RefeicaoFit,
    Agua
}

public enum HiddenAction
{
    Workout,
    Save,
    Purchase,
    Craft,
    Shop
}

public enum DailyMissionKind
{
    Workout,
    Save,
    Purchase,
    Hydration
}

public sealed class GameState
{
    public PlayerProfile Profile { get; set; } = new();
    public AttributeSheet Attributes { get; set; } = new();
    public ResourceWallet Resources { get; set; } = new();
    public List<DailyMissionState> Missions { get; set; } = [];
    public string LastDailyReset { get; set; } = "";
    public bool AlertsEnabled { get; set; }
    public List<string> OwnedItems { get; set; } = [];
    public List<string> CraftedItems { get; set; } = [];
    public Dictionary<string, int> PotionInventory { get; set; } = [];
    public Dictionary<PurchaseType, int> PurchaseInventory { get; set; } = [];
    public List<ActivityLogEntry> ActivityLog { get; set; } = [];
    public HiddenProgress HiddenProgress { get; set; } = new();
    public int DailyQuickActions { get; set; }
    public int DailySaveActions { get; set; }
}

public sealed class PlayerProfile
{
    public string Name { get; set; } = "Jogador";
    public string Class { get; set; } = "Universal";
    public int Level { get; set; } = 1;
    public string Rank { get; set; } = "E";
    public int Xp { get; set; }
    public int XpToNext { get; set; } = 120;
    public int DayStreak { get; set; } = 1;
    public string LastActiveDate { get; set; } = "";
}

public sealed class AttributeSheet
{
    public int Strength { get; set; } = 5;
    public int Agility { get; set; } = 4;
    public int Vitality { get; set; } = 4;
    public int Intelligence { get; set; } = 3;
    public int Resistance { get; set; } = 3;
    public int Speed { get; set; } = 3;
}

public sealed class ResourceWallet
{
    public int Gold { get; set; } = 120;
    public decimal SavedMoney { get; set; }
    public int SkillPoints { get; set; } = 3;
}

public sealed class DailyMissionState
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DailyMissionKind Kind { get; set; }
    public decimal Progress { get; set; }
    public decimal Target { get; set; }
    public bool Completed { get; set; }
    public Reward Reward { get; set; } = new();
}

public sealed class Reward
{
    public int Xp { get; set; }
    public int Gold { get; set; }
    public int SkillPoints { get; set; }
}

public sealed class ShopItem
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public int Price { get; set; }
    public string Description { get; set; } = "";
    public int MinLevel { get; set; }
    public string RequiredClass { get; set; } = "";
    public string RequirementText =>
        string.IsNullOrWhiteSpace(RequiredClass)
            ? MinLevel > 0 ? $"Requer nível {MinLevel}" : string.Empty
            : MinLevel > 0 ? $"Requer nível {MinLevel} e classe {RequiredClass}" : $"Requer classe {RequiredClass}";
}

public sealed class CraftRecipe
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public int GoldCost { get; set; }
    public string Effect { get; set; } = "";
    public List<CraftRequirement> Requirements { get; set; } = [];
    public Reward Reward { get; set; } = new();
}

public sealed class PotionRequirement
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PurchaseType Type { get; set; }
    public int Count { get; set; }
}

public sealed class PotionRecipe
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public int Tier { get; set; }
    public int GoldCost { get; set; }
    public string Effect { get; set; } = "";
    public List<PotionRequirement> Requirements { get; set; } = [];
    public Reward Reward { get; set; } = new();
}

public sealed class CraftRequirement
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PurchaseType Type { get; set; }
    public int Count { get; set; }
}

public sealed class PurchaseOption
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PurchaseType Id { get; set; }
    public string Label { get; set; } = "";
}

public sealed class ActivityLogEntry
{
    public DateTime Timestamp { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
}

public sealed class HiddenProgress
{
    public int WorkoutCount { get; set; }
    public int SaveCount { get; set; }
    public int PurchaseCount { get; set; }
    public int CraftCount { get; set; }
    public int ShopCount { get; set; }
    public bool UnlockedSecret { get; set; }
}