using System.Text.Json;
using System.Text.Json.Serialization;
using EvolutionPlayMobile.Models;

namespace EvolutionPlayMobile.Services;

public sealed class EvolutionEngine
{
    private readonly object _sync = new();
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly string _statePath;

    public EvolutionEngine()
    {
        var appDataPath = FileSystem.AppDataDirectory;
        _statePath = Path.Combine(appDataPath, "evolution-state.json");
    }

    public DashboardViewModel BuildDashboard(string? message = null, string? error = null)
    {
        lock (_sync)
        {
            var state = LoadState();
            return CreateViewModel(state, message, error);
        }
    }

    public void CompleteWorkout(WorkoutIntensity intensity)
    {
        lock (_sync)
        {
            var state = LoadState();
            if (state.DailyQuickActions >= 1)
            {
                throw new InvalidOperationException("Voce ja realizou uma acao rapida hoje. Tente de novo amanha.");
            }

            var reward = intensity switch
            {
                WorkoutIntensity.Light => (Strength: 1, Speed: 1, Resistance: 0, Xp: 18, Gold: 12),
                WorkoutIntensity.Medium => (Strength: 2, Speed: 0, Resistance: 1, Xp: 28, Gold: 18),
                _ => (Strength: 4, Speed: 1, Resistance: 2, Xp: 45, Gold: 26)
            };

            state.DailyQuickActions += 1;
            state.Attributes.Strength += reward.Strength;
            state.Attributes.Vitality += 1;
            state.Attributes.Speed += reward.Speed;
            state.Attributes.Resistance += reward.Resistance;
            state.Resources.Gold += reward.Gold;
            ApplyXp(state, reward.Xp);
            AdvanceMission(state, DailyMissionKind.Workout, 1);
            ApplyHiddenEffect(state, HiddenAction.Workout);
            AddLog(state, "Treino concluido", $"{intensity} -> +{reward.Strength} força, +{reward.Gold} ouro, +{reward.Xp} XP");
            SaveState(state);
        }
    }

    public void SaveMoney(decimal amount)
    {
        if (amount <= 0)
        {
            throw new InvalidOperationException("Informe um valor valido para economizar.");
        }

        lock (_sync)
        {
            var state = LoadState();
            if (state.DailySaveActions >= 3)
            {
                throw new InvalidOperationException("Voce ja economizou 3 vezes hoje. Aguarde amanha.");
            }

            var goldEarned = Math.Max(1, (int)Math.Floor(amount * 1.6m));

            state.DailySaveActions += 1;
            state.Resources.SavedMoney += amount;
            state.Resources.Gold += goldEarned;
            var xpGain = Math.Max(5, (int)Math.Floor(amount * 0.7m));
            ApplyXp(state, xpGain);
            AdvanceMission(state, DailyMissionKind.Save, amount);
            ApplyHiddenEffect(state, HiddenAction.Save);
            AddLog(state, "Economia registrada", $"R$ {amount:N2} -> +{goldEarned} gold, +{xpGain} XP");
            SaveState(state);
        }
    }

    public void LogPurchase(PurchaseType purchaseType)
    {
        lock (_sync)
        {
            var state = LoadState();
            state.PurchaseInventory[purchaseType] = state.PurchaseInventory.GetValueOrDefault(purchaseType) + 1;
            state.Resources.Gold += 4;
            ApplyXp(state, purchaseType == PurchaseType.Agua ? 5 : 8);
            AdvanceMission(state, purchaseType == PurchaseType.Agua ? DailyMissionKind.Hydration : DailyMissionKind.Purchase, 1);
            ApplyHiddenEffect(state, HiddenAction.Purchase);
            AddLog(state, "Compra real registrada", $"{purchaseType} -> +4 gold");
            SaveState(state);
        }
    }

    public void CraftPotion(string potionId)
    {
        lock (_sync)
        {
            var state = LoadState();
            var potion = GameCatalog.Potions.FirstOrDefault(item => item.Id == potionId)
                ?? throw new InvalidOperationException("Pocao nao encontrada.");

            if (state.Resources.Gold < potion.GoldCost)
            {
                throw new InvalidOperationException("Gold insuficiente para fabricar essa pocao.");
            }

            foreach (var requirement in potion.Requirements)
            {
                if (state.PurchaseInventory.GetValueOrDefault(requirement.Type) < requirement.Count)
                {
                    throw new InvalidOperationException("Faltam compras reais registradas para essa receita.");
                }
            }

            foreach (var requirement in potion.Requirements)
            {
                state.PurchaseInventory[requirement.Type] -= requirement.Count;
            }

            state.Resources.Gold -= potion.GoldCost;
            state.Resources.SkillPoints += potion.Reward.SkillPoints;
            state.PotionInventory[potion.Id] = state.PotionInventory.GetValueOrDefault(potion.Id) + 1;
            state.Attributes.Strength += potion.Tier;
            state.Attributes.Vitality += Math.Max(1, potion.Tier - 1);
            if (potion.Tier >= 3)
            {
                state.Attributes.Agility += 1;
            }

            if (potion.Tier >= 4)
            {
                state.Attributes.Intelligence += 2;
            }

            ApplyXp(state, potion.Reward.Xp);
            ApplyHiddenEffect(state, HiddenAction.Craft);
            AddLog(state, "Pocao criada", $"{potion.Name} -> custo {potion.GoldCost} gold");
            SaveState(state);
        }
    }

    public void CraftItem(string recipeId)
    {
        lock (_sync)
        {
            var state = LoadState();
            var recipe = GameCatalog.CraftRecipes.FirstOrDefault(item => item.Id == recipeId)
                ?? throw new InvalidOperationException("Receita nao encontrada.");

            if (state.CraftedItems.Contains(recipeId))
            {
                throw new InvalidOperationException("Esse item craftavel ja foi criado.");
            }

            if (state.Resources.Gold < recipe.GoldCost)
            {
                throw new InvalidOperationException("Gold insuficiente para criar esse item.");
            }

            foreach (var requirement in recipe.Requirements)
            {
                if (state.PurchaseInventory.GetValueOrDefault(requirement.Type) < requirement.Count)
                {
                    throw new InvalidOperationException("Faltam ingredientes registrados para essa criacao.");
                }
            }

            foreach (var requirement in recipe.Requirements)
            {
                state.PurchaseInventory[requirement.Type] -= requirement.Count;
            }

            state.Resources.Gold -= recipe.GoldCost;
            state.Resources.SkillPoints += recipe.Reward.SkillPoints;
            state.CraftedItems.Add(recipeId);
            state.Attributes.Strength += recipe.Category is "Armadura" ? 1 : 0;
            state.Attributes.Vitality += recipe.Category is "Armadura" or "Reliquia" ? 2 : 1;
            state.Attributes.Agility += recipe.Category is "Acessorio" ? 1 : 0;
            state.Attributes.Intelligence += recipe.Category is "Artefato" or "Reliquia" ? 1 : 0;
            ApplyXp(state, recipe.Reward.Xp);
            ApplyHiddenEffect(state, HiddenAction.Craft);
            AddLog(state, "Item criado", $"{recipe.Name} -> custo {recipe.GoldCost} gold");
            SaveState(state);
        }
    }

    public void BuyItem(string itemId)
    {
        lock (_sync)
        {
            var state = LoadState();
            var item = GameCatalog.ShopItems.FirstOrDefault(entry => entry.Id == itemId)
                ?? throw new InvalidOperationException("Item nao encontrado.");

            if (!IsItemAccessible(item, state))
            {
                throw new InvalidOperationException("Voce ainda nao esta apto a comprar esse item.");
            }

            if (state.OwnedItems.Contains(itemId))
            {
                throw new InvalidOperationException("Esse item ja foi comprado.");
            }

            if (state.Resources.Gold < item.Price)
            {
                throw new InvalidOperationException("Gold insuficiente para comprar esse item.");
            }

            state.Resources.Gold -= item.Price;
            state.OwnedItems.Add(itemId);
            var xpGain = 10;
            ApplyXp(state, xpGain);
            ApplyHiddenEffect(state, HiddenAction.Shop);
            AddLog(state, "Item comprado", $"{item.Name} -> {item.Price} gold, +{xpGain} XP");
            SaveState(state);
        }
    }

    public void AllocatePoint(string attributeName)
    {
        lock (_sync)
        {
            var state = LoadState();
            if (state.Resources.SkillPoints <= 0)
            {
                throw new InvalidOperationException("Voce nao tem pontos de habilidade para distribuir.");
            }

            switch (attributeName.Trim().ToLowerInvariant())
            {
                case "strength":
                    state.Attributes.Strength += 1;
                    break;
                case "agility":
                    state.Attributes.Agility += 1;
                    break;
                case "vitality":
                    state.Attributes.Vitality += 1;
                    break;
                case "intelligence":
                    state.Attributes.Intelligence += 1;
                    break;
                case "speed":
                    state.Attributes.Speed += 1;
                    break;
                case "resistance":
                    state.Attributes.Resistance += 1;
                    break;
                default:
                    throw new InvalidOperationException("Atributo invalido.");
            }

            state.Resources.SkillPoints -= 1;
            AddLog(state, "Ponto distribuido", $"+1 em {attributeName}");
            SaveState(state);
        }
    }

    public void SetAlerts(bool enabled)
    {
        lock (_sync)
        {
            var state = LoadState();
            state.AlertsEnabled = enabled;
            AddLog(state, enabled ? "Alertas ativados" : "Alertas desativados", "Lembretes do navegador");
            SaveState(state);
        }
    }

    private GameState LoadState()
    {
        if (!File.Exists(_statePath))
        {
            var initial = CreateInitialState();
            SaveState(initial);
            return initial;
        }

        var json = File.ReadAllText(_statePath);
        var state = JsonSerializer.Deserialize<GameState>(json, _jsonOptions) ?? CreateInitialState();
        ResetLoopIfNeeded(state);
        state.PurchaseInventory ??= Enum.GetValues<PurchaseType>().ToDictionary(type => type, _ => 0);
        state.CraftedItems ??= [];
        state.HiddenProgress ??= new HiddenProgress();
        return state;
    }

    private void SaveState(GameState state)
    {
        state.ActivityLog = state.ActivityLog
            .OrderByDescending(entry => entry.Timestamp)
            .Take(12)
            .ToList();

        File.WriteAllText(_statePath, JsonSerializer.Serialize(state, _jsonOptions));
    }

    private static GameState CreateInitialState()
    {
        return new GameState
        {
            Missions = GameCatalog.DailyMissionsTemplate.Select(m => new DailyMissionState
            {
                Id = m.Id,
                Title = m.Title,
                Kind = m.Kind,
                Target = m.Target,
                Reward = m.Reward
            }).ToList()
        };
    }

    private static void ResetLoopIfNeeded(GameState state)
    {
        var today = DateTime.Now.ToString("yyyy-MM-dd");
        if (state.LastDailyReset != today)
        {
            state.LastDailyReset = today;
            foreach (var mission in state.Missions)
            {
                mission.Progress = 0;
                mission.Completed = false;
            }

            state.DailyQuickActions = 0;
            state.DailySaveActions = 0;
        }
    }

    private static void ApplyXp(GameState state, int xp)
    {
        state.Profile.Xp += xp;
        while (state.Profile.Xp >= state.Profile.XpToNext)
        {
            state.Profile.Level += 1;
            state.Profile.Xp -= state.Profile.XpToNext;
            state.Profile.XpToNext = (int)(state.Profile.XpToNext * 1.2);
            state.Resources.SkillPoints += 1;
            state.Profile.Rank = GetRankForLevel(state.Profile.Level);
        }
    }

    private static string GetRankForLevel(int level)
    {
        return level switch
        {
            <= 5 => "E",
            <= 10 => "D",
            <= 15 => "C",
            <= 20 => "B",
            <= 25 => "A",
            <= 30 => "S",
            _ => "SS"
        };
    }

    private static void AdvanceMission(GameState state, DailyMissionKind kind, decimal progress)
    {
        var mission = state.Missions.FirstOrDefault(m => m.Kind == kind);
        if (mission is not null && !mission.Completed)
        {
            mission.Progress += progress;
            if (mission.Progress >= mission.Target)
            {
                mission.Completed = true;
                state.Resources.Gold += mission.Reward.Gold;
                state.Resources.SkillPoints += mission.Reward.SkillPoints;
                ApplyXp(state, mission.Reward.Xp);
            }
        }
    }

    private static void ApplyHiddenEffect(GameState state, HiddenAction action)
    {
        switch (action)
        {
            case HiddenAction.Workout:
                state.HiddenProgress.WorkoutCount++;
                break;
            case HiddenAction.Save:
                state.HiddenProgress.SaveCount++;
                break;
            case HiddenAction.Purchase:
                state.HiddenProgress.PurchaseCount++;
                break;
            case HiddenAction.Craft:
                state.HiddenProgress.CraftCount++;
                break;
            case HiddenAction.Shop:
                state.HiddenProgress.ShopCount++;
                break;
        }

        if (!state.HiddenProgress.UnlockedSecret &&
            state.HiddenProgress.WorkoutCount >= 10 &&
            state.HiddenProgress.SaveCount >= 5 &&
            state.HiddenProgress.PurchaseCount >= 15 &&
            state.HiddenProgress.CraftCount >= 3 &&
            state.HiddenProgress.ShopCount >= 2)
        {
            state.HiddenProgress.UnlockedSecret = true;
            state.Resources.Gold += 500;
            state.Resources.SkillPoints += 5;
            AddLog(state, "Segredo desbloqueado!", "Parabens! Voce encontrou o caminho secreto da evolucao.");
        }
    }

    private static void AddLog(GameState state, string title, string description)
    {
        state.ActivityLog.Insert(0, new ActivityLogEntry
        {
            Timestamp = DateTime.Now,
            Title = title,
            Description = description
        });
    }

    private static DashboardViewModel CreateViewModel(GameState state, string? message, string? error)
    {
        var availableShopItems = GameCatalog.ShopItems
            .Where(item => !state.OwnedItems.Contains(item.Id) && IsItemAccessible(item, state))
            .ToList();

        var ownedShopItems = GameCatalog.ShopItems
            .Where(item => state.OwnedItems.Contains(item.Id))
            .ToList();

        return new DashboardViewModel
        {
            State = state,
            ShopItems = availableShopItems,
            OwnedShopItems = ownedShopItems,
            Potions = GameCatalog.Potions,
            CraftRecipes = GameCatalog.CraftRecipes,
            PurchaseOptions = GameCatalog.PurchaseOptions,
            Message = message,
            Error = error
        };
    }

    private static bool IsItemAccessible(ShopItem item, GameState state)
    {
        if (item.MinLevel > 0 && state.Profile.Level < item.MinLevel)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(item.RequiredClass) &&
            !string.Equals(state.Profile.Class, item.RequiredClass, StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(item.RequiredClass, "Universal", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return true;
    }
}
