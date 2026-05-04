namespace EvolutionPlayMobile.Models;

public static class GameCatalog
{
    public static IReadOnlyList<PurchaseOption> PurchaseOptions { get; } =
    [
        new() { Id = PurchaseType.Treino, Label = "Treino pago" },
        new() { Id = PurchaseType.Suco, Label = "Suco natural" },
        new() { Id = PurchaseType.Vitamina, Label = "Vitaminas" },
        new() { Id = PurchaseType.Suplemento, Label = "Suplemento" },
        new() { Id = PurchaseType.RefeicaoFit, Label = "Refeicao fit" },
        new() { Id = PurchaseType.Agua, Label = "Agua / hidratacao" }
    ];

    public static IReadOnlyList<DailyMissionState> DailyMissionsTemplate { get; } =
    [
        new()
        {
            Id = "daily-workout",
            Title = "Finalizar 2 treinos ou sessoes fisicas",
            Kind = DailyMissionKind.Workout,
            Target = 2,
            Reward = new Reward { Xp = 45, Gold = 30, SkillPoints = 1 }
        },
        new()
        {
            Id = "daily-save",
            Title = "Economizar pelo menos R$ 20 no dia",
            Kind = DailyMissionKind.Save,
            Target = 20,
            Reward = new Reward { Xp = 25, Gold = 40, SkillPoints = 1 }
        },
        new()
        {
            Id = "daily-purchase",
            Title = "Registrar 2 compras reais focadas na evolucao",
            Kind = DailyMissionKind.Purchase,
            Target = 2,
            Reward = new Reward { Xp = 35, Gold = 25, SkillPoints = 1 }
        },
        new()
        {
            Id = "daily-hydration",
            Title = "Fazer 3 acoes de hidratacao",
            Kind = DailyMissionKind.Hydration,
            Target = 3,
            Reward = new Reward { Xp = 20, Gold = 20, SkillPoints = 0 }
        }
    ];

    public static IReadOnlyList<PotionRecipe> Potions { get; } =
    [
        new()
        {
            Id = "potion-bronze",
            Name = "Pocao Bronzestrike",
            Tier = 1,
            GoldCost = 25,
            Effect = "+2 strength e +15 XP",
            Requirements = [new() { Type = PurchaseType.Suco, Count = 1 }, new() { Type = PurchaseType.Agua, Count = 1 }],
            Reward = new Reward { Xp = 15 }
        },
        new()
        {
            Id = "potion-silver",
            Name = "Pocao Silver Pulse",
            Tier = 2,
            GoldCost = 45,
            Effect = "+1 strength, +1 vitality e +25 XP",
            Requirements = [new() { Type = PurchaseType.Treino, Count = 1 }, new() { Type = PurchaseType.Vitamina, Count = 1 }],
            Reward = new Reward { Xp = 25, SkillPoints = 1 }
        },
        new()
        {
            Id = "potion-gold",
            Name = "Pocao Gold Surge",
            Tier = 3,
            GoldCost = 70,
            Effect = "+2 strength, +1 agility e +40 XP",
            Requirements =
            [
                new() { Type = PurchaseType.Treino, Count = 1 },
                new() { Type = PurchaseType.Suplemento, Count = 1 },
                new() { Type = PurchaseType.RefeicaoFit, Count = 1 }
            ],
            Reward = new Reward { Xp = 40, SkillPoints = 1 }
        },
        new()
        {
            Id = "potion-shadow",
            Name = "Pocao Shadow Monarch",
            Tier = 4,
            GoldCost = 120,
            Effect = "+3 strength, +2 vitality, +2 intelligence e +60 XP",
            Requirements =
            [
                new() { Type = PurchaseType.Treino, Count = 2 },
                new() { Type = PurchaseType.Suco, Count = 1 },
                new() { Type = PurchaseType.Vitamina, Count = 1 },
                new() { Type = PurchaseType.Suplemento, Count = 1 }
            ],
            Reward = new Reward { Xp = 60, SkillPoints = 2 }
        }
    ];

    public static IReadOnlyList<ShopItem> ShopItems { get; } =
    [
        // Armas
        new() { Id = "blade-obsidian", Name = "Lâmina Obsidiana", Category = "Arma", Price = 90, Description = "Presença de caçador.", MinLevel = 5 },
        new() { Id = "sword-dragon", Name = "Espada do Dragão", Category = "Arma", Price = 135, Description = "Poder mítico despertado.", MinLevel = 15, RequiredClass = "Warrior" },
        new() { Id = "blade-twilight", Name = "Lâmina Crepuscular", Category = "Arma", Price = 110, Description = "Corta entre mundos.", MinLevel = 10, RequiredClass = "Rogue" },
        new() { Id = "lance-sky", Name = "Lança Celestial", Category = "Arma", Price = 125, Description = "Toca o infinito.", MinLevel = 12, RequiredClass = "Warrior" },
        
        // Equipamento
        new() { Id = "gloves-titan", Name = "Luvas do Titã", Category = "Equipamento", Price = 70, Description = "Força bruta.", MinLevel = 4 },
        new() { Id = "boots-wind", Name = "Botas do Vendaval", Category = "Equipamento", Price = 60, Description = "Agilidade para rotina.", MinLevel = 3, RequiredClass = "Rogue" },
        new() { Id = "chestplate-steel", Name = "Peitoral de Aço", Category = "Equipamento", Price = 85, Description = "Proteção absoluta.", MinLevel = 8 },
        new() { Id = "helm-crown", Name = "Elmo Real", Category = "Equipamento", Price = 95, Description = "Liderança visual.", MinLevel = 10 },
        new() { Id = "gauntlets-fire", Name = "Manoplas de Fogo", Category = "Equipamento", Price = 75, Description = "Queima com força." },
        new() { Id = "leggings-shadow", Name = "Calças da Sombra", Category = "Equipamento", Price = 65, Description = "Mobilidade absoluta." },
        
        // Acessórios
        new() { Id = "ring-focus", Name = "Anel do Foco", Category = "Acessório", Price = 55, Description = "Disciplina constante." },
        new() { Id = "amulet-vigor", Name = "Amuleto do Vigor", Category = "Acessório", Price = 65, Description = "Consistência pura." },
        new() { Id = "ring-luck", Name = "Anel da Sorte", Category = "Acessório", Price = 72, Description = "Fortuna aos bravos." },
        new() { Id = "bracelet-hunter", Name = "Pulseira do Caçador", Category = "Acessório", Price = 68, Description = "Instinto aguçado." },
        new() { Id = "necklace-power", Name = "Colar de Poder", Category = "Acessório", Price = 88, Description = "Energia constante." },
        
        // Relíquias
        new() { Id = "core-bronze", Name = "Núcleo Bronze", Category = "Relíquia", Price = 30, Description = "Primeiro despertar." },
        new() { Id = "core-silver", Name = "Núcleo Prata", Category = "Relíquia", Price = 50, Description = "Progresso sustentável." },
        new() { Id = "core-gold", Name = "Núcleo Ouro", Category = "Relíquia", Price = 95, Description = "Salto de evolução." },
        new() { Id = "core-diamond", Name = "Núcleo Diamante", Category = "Relíquia", Price = 155, Description = "Imortalidade tangível." },
        new() { Id = "sigil-power", Name = "Sigilo de Poder", Category = "Relíquia", Price = 115, Description = "Você não para." },
        new() { Id = "sigil-wealth", Name = "Sigilo da Riqueza", Category = "Relíquia", Price = 105, Description = "Disciplina econômica." },
        new() { Id = "sigil-mind", Name = "Sigilo da Mente", Category = "Relíquia", Price = 98, Description = "Builds equilibradas." },
        
        // Companheiros
        new() { Id = "pet-slime", Name = "Slime Azul", Category = "Companheiro", Price = 28, Description = "Colecionável divertido." },
        new() { Id = "drone-scout", Name = "Drone Scout", Category = "Companheiro", Price = 110, Description = "Exploração virtual." },
        new() { Id = "wolf-spirit", Name = "Lobo Espiritual", Category = "Companheiro", Price = 140, Description = "Presença feroz.", MinLevel = 14 },
        new() { Id = "panther-spirit", Name = "Pantera Sombria", Category = "Companheiro", Price = 160, Description = "Raro e ágil.", MinLevel = 16, RequiredClass = "Rogue" },
        new() { Id = "phoenix-mini", Name = "Fênix de Bolso", Category = "Companheiro", Price = 170, Description = "Lendário e eterno.", MinLevel = 18 },
        new() { Id = "golem-guardian", Name = "Golem Guardião", Category = "Companheiro", Price = 150, Description = "Protetor absoluto.", MinLevel = 18, RequiredClass = "Warrior" },
        
        // Cosméticos
        new() { Id = "cape-shadow", Name = "Capa da Sombra", Category = "Cosmético", Price = 80, Description = "Aura intimidadora." },
        new() { Id = "skin-noir", Name = "Skin Noir Runner", Category = "Cosmético", Price = 77, Description = "Urbano e agressivo." },
        new() { Id = "skin-solar", Name = "Skin Solar Flame", Category = "Cosmético", Price = 99, Description = "Brilho dourado." },
        new() { Id = "skin-abyss", Name = "Skin Abyss Core", Category = "Cosmético", Price = 125, Description = "Tema sombrio raro." },
        new() { Id = "skin-frost", Name = "Skin Frost Nova", Category = "Cosmético", Price = 88, Description = "Gelo e elegância." },
        
        // Títulos
        new() { Id = "badge-rookie", Name = "Insígnia Rookie", Category = "Título", Price = 25, Description = "Seu começo." },
        new() { Id = "badge-hunter", Name = "Insígnia Hunter", Category = "Título", Price = 45, Description = "Missões completas." },
        new() { Id = "badge-elite", Name = "Insígnia Elite", Category = "Título", Price = 85, Description = "Respeito absoluto." },
        new() { Id = "badge-legend", Name = "Insígnia Lendária", Category = "Título", Price = 145, Description = "Nome na história." },
        
        // Base/Hub
        new() { Id = "banner-red", Name = "Banner Carmesim", Category = "Base", Price = 35, Description = "Quartel virtual." },
        new() { Id = "banner-iron", Name = "Banner de Ferro", Category = "Base", Price = 48, Description = "Visual industrial." },
        new() { Id = "banner-celestial", Name = "Banner Celestial", Category = "Base", Price = 92, Description = "Premium veterano.", MinLevel = 12 },
        new() { Id = "desk-war", Name = "Mesa Tática", Category = "Base", Price = 75, Description = "Comando da evolução.", MinLevel = 6 },
        new() { Id = "forge-mini", Name = "Mini Forja", Category = "Base", Price = 88, Description = "RPG elevado.", MinLevel = 8 },
        new() { Id = "forge-royal", Name = "Forja Real", Category = "Base", Price = 130, Description = "Laboratório pessoal.", MinLevel = 18 },
        new() { Id = "room-gym", Name = "Quarto Gym Mode", Category = "Base", Price = 145, Description = "Dashboard completo." },
        new() { Id = "room-alchemy", Name = "Quarto Alquimia", Category = "Base", Price = 155, Description = "Lab visual poções." },
        
        // Molduras de Perfil
        new() { Id = "frame-basic", Name = "Moldura Básica", Category = "Moldura", Price = 20, Description = "Seu inicio." },
        new() { Id = "frame-ember", Name = "Moldura Ember", Category = "Moldura", Price = 42, Description = "Reflexos quentes." },
        new() { Id = "frame-frost", Name = "Moldura Frost", Category = "Moldura", Price = 58, Description = "Limpo e frio." },
        new() { Id = "frame-onyx", Name = "Moldura Onyx", Category = "Moldura", Price = 86, Description = "Premium escuro." },
        new() { Id = "frame-royal", Name = "Moldura Real", Category = "Moldura", Price = 115, Description = "Poder absoluto." }
    ];

    public static IReadOnlyList<CraftRecipe> CraftRecipes { get; } =
    [
        new()
        {
            Id = "craft-aegis-vest",
            Name = "Veste Aegis",
            Category = "Armadura",
            GoldCost = 95,
            Effect = "+2 vitality e +1 intelligence",
            Requirements =
            [
                new() { Type = PurchaseType.Treino, Count = 1 },
                new() { Type = PurchaseType.Suplemento, Count = 1 },
                new() { Type = PurchaseType.RefeicaoFit, Count = 1 }
            ],
            Reward = new Reward { Xp = 32, SkillPoints = 1 }
        },
        new()
        {
            Id = "craft-rune-band",
            Name = "Bracelete Runa",
            Category = "Acessorio",
            GoldCost = 68,
            Effect = "+1 agility e +1 intelligence",
            Requirements =
            [
                new() { Type = PurchaseType.Vitamina, Count = 1 },
                new() { Type = PurchaseType.Suco, Count = 1 }
            ],
            Reward = new Reward { Xp = 22 }
        },
        new()
        {
            Id = "craft-shadow-mail",
            Name = "Malha Umbra",
            Category = "Armadura",
            GoldCost = 140,
            Effect = "+3 vitality e +2 strength",
            Requirements =
            [
                new() { Type = PurchaseType.Treino, Count = 2 },
                new() { Type = PurchaseType.Suplemento, Count = 1 },
                new() { Type = PurchaseType.Vitamina, Count = 1 }
            ],
            Reward = new Reward { Xp = 48, SkillPoints = 1 }
        },
        new()
        {
            Id = "craft-aurora-lantern",
            Name = "Lanterna Aurora",
            Category = "Reliquia",
            GoldCost = 88,
            Effect = "+1 vitality, +1 intelligence e +28 XP",
            Requirements =
            [
                new() { Type = PurchaseType.Agua, Count = 2 },
                new() { Type = PurchaseType.Suco, Count = 1 },
                new() { Type = PurchaseType.RefeicaoFit, Count = 1 }
            ],
            Reward = new Reward { Xp = 28 }
        },
        new()
        {
            Id = "craft-war-sigil",
            Name = "Sigilo Belico",
            Category = "Reliquia",
            GoldCost = 120,
            Effect = "+2 strength, +1 agility e +40 XP",
            Requirements =
            [
                new() { Type = PurchaseType.Treino, Count = 2 },
                new() { Type = PurchaseType.Suco, Count = 1 },
                new() { Type = PurchaseType.Suplemento, Count = 1 }
            ],
            Reward = new Reward { Xp = 40, SkillPoints = 1 }
        },
        new()
        {
            Id = "craft-crown-echo",
            Name = "Coroa Echo",
            Category = "Artefato",
            GoldCost = 165,
            Effect = "+2 intelligence, +2 vitality e +55 XP",
            Requirements =
            [
                new() { Type = PurchaseType.Vitamina, Count = 2 },
                new() { Type = PurchaseType.RefeicaoFit, Count = 1 },
                new() { Type = PurchaseType.Suplemento, Count = 1 }
            ],
            Reward = new Reward { Xp = 55, SkillPoints = 2 }
        }
    ];
}