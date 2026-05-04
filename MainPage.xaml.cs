using EvolutionPlayMobile.Models;
using EvolutionPlayMobile.Services;

namespace EvolutionPlayMobile;

public partial class MainPage : ContentPage
{
    private EvolutionEngine _engine;

    public MainPage()
    {
        InitializeComponent();
        ConnectionStatusLabel.Text = "Carregando jogo...";
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await InitializeGameAsync();
    }

    private async Task InitializeGameAsync()
    {
        try
        {
            _engine = new EvolutionEngine();
            var dashboard = _engine.BuildDashboard();
            BindingContext = dashboard;
            ConnectionStatusLabel.Text = "Jogo carregado com sucesso!";

            // Initialize navigation - show home section
            ShowSection("Home");
        }
        catch (Exception ex)
        {
            ConnectionStatusLabel.Text = "Erro ao carregar jogo.";
            await DisplayAlertAsync("Erro", $"Falha ao carregar o jogo: {ex.Message}", "OK");
        }
    }

    private async void OnWorkoutLightClicked(object sender, EventArgs e)
    {
        try
        {
            _engine.CompleteWorkout(WorkoutIntensity.Light);
            var dashboard = _engine.BuildDashboard("Treino leve concluído!");
            BindingContext = dashboard;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", ex.Message, "OK");
        }
    }

    private async void OnWorkoutMediumClicked(object sender, EventArgs e)
    {
        try
        {
            _engine.CompleteWorkout(WorkoutIntensity.Medium);
            var dashboard = _engine.BuildDashboard("Treino médio concluído!");
            BindingContext = dashboard;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", ex.Message, "OK");
        }
    }

    private async void OnWorkoutHeavyClicked(object sender, EventArgs e)
    {
        try
        {
            _engine.CompleteWorkout(WorkoutIntensity.Heavy);
            var dashboard = _engine.BuildDashboard("Treino pesado concluído!");
            BindingContext = dashboard;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", ex.Message, "OK");
        }
    }

    private async void OnSaveMoneyClicked(object sender, EventArgs e)
    {
        var result = await DisplayPromptAsync("Registrar Economia", "Digite o valor economizado (R$):", keyboard: Keyboard.Numeric);
        if (string.IsNullOrEmpty(result) || !decimal.TryParse(result, out var amount))
        {
            return;
        }

        try
        {
            _engine.SaveMoney(amount);
            var dashboard = _engine.BuildDashboard($"Economia de R$ {amount:N2} registrada!");
            BindingContext = dashboard;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", ex.Message, "OK");
        }
    }

    private async void OnPurchaseClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is PurchaseType purchaseType)
        {
            try
            {
                _engine.LogPurchase(purchaseType);
                var dashboard = _engine.BuildDashboard($"Compra de {purchaseType} registrada!");
                BindingContext = dashboard;
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Erro", ex.Message, "OK");
            }
        }
    }

    private async void OnBuyItemClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is ShopItem item)
        {
            try
            {
                _engine.BuyItem(item.Id);
                var dashboard = _engine.BuildDashboard($"{item.Name} comprado!");
                BindingContext = dashboard;
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Erro", ex.Message, "OK");
            }
        }
    }

    private async void OnCraftItemClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is CraftRecipe recipe)
        {
            try
            {
                _engine.CraftItem(recipe.Id);
                var dashboard = _engine.BuildDashboard($"{recipe.Name} criado com sucesso!");
                BindingContext = dashboard;
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Erro", ex.Message, "OK");
            }
        }
    }

    private async void OnCreatePotionClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is PotionRecipe potion)
        {
            try
            {
                _engine.CraftPotion(potion.Id);
                var dashboard = _engine.BuildDashboard($"{potion.Name} criada!");
                BindingContext = dashboard;
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Erro", ex.Message, "OK");
            }
        }
    }

    private void OnHomeClicked(object sender, EventArgs e)
    {
        ShowSection("Home");
    }

    private void OnShopClicked(object sender, EventArgs e)
    {
        ShowSection("Shop");
    }

    private void OnCraftClicked(object sender, EventArgs e)
    {
        ShowSection("Craft");
    }

    private void OnPotionsClicked(object sender, EventArgs e)
    {
        ShowSection("Potions");
    }

    private void OnInventoryClicked(object sender, EventArgs e)
    {
        ShowSection("Inventory");
    }

    private void OnChatbotClicked(object sender, EventArgs e)
    {
        ShowSection("Chatbot");
        InitializeChatbot();
    }

    private void ShowSection(string sectionName)
    {
        // Hide all sections first
        HomeSection.IsVisible = false;
        ShopSection.IsVisible = false;
        CraftSection.IsVisible = false;
        PotionsSection.IsVisible = false;
        InventorySection.IsVisible = false;
        ChatbotSection.IsVisible = false;

        // Show the selected section
        switch (sectionName)
        {
            case "Home":
                HomeSection.IsVisible = true;
                break;
            case "Shop":
                ShopSection.IsVisible = true;
                break;
            case "Craft":
                CraftSection.IsVisible = true;
                break;
            case "Potions":
                PotionsSection.IsVisible = true;
                break;
            case "Inventory":
                InventorySection.IsVisible = true;
                break;
            case "Chatbot":
                ChatbotSection.IsVisible = true;
                break;
        }
    }

    private async void OnAllocateStrengthClicked(object sender, EventArgs e)
    {
        try
        {
            _engine.AllocatePoint("strength");
            var dashboard = _engine.BuildDashboard("Ponto alocado em Força!");
            BindingContext = dashboard;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", ex.Message, "OK");
        }
    }

    private async void OnAllocateAgilityClicked(object sender, EventArgs e)
    {
        try
        {
            _engine.AllocatePoint("agility");
            var dashboard = _engine.BuildDashboard("Ponto alocado em Agilidade!");
            BindingContext = dashboard;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", ex.Message, "OK");
        }
    }

    private async void OnAllocateVitalityClicked(object sender, EventArgs e)
    {
        try
        {
            _engine.AllocatePoint("vitality");
            var dashboard = _engine.BuildDashboard("Ponto alocado em Vitalidade!");
            BindingContext = dashboard;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", ex.Message, "OK");
        }
    }

    private async void OnAllocateIntelligenceClicked(object sender, EventArgs e)
    {
        try
        {
            _engine.AllocatePoint("intelligence");
            var dashboard = _engine.BuildDashboard("Ponto alocado em Inteligência!");
            BindingContext = dashboard;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", ex.Message, "OK");
        }
    }

    private async void OnAllocateSpeedClicked(object sender, EventArgs e)
    {
        try
        {
            _engine.AllocatePoint("speed");
            var dashboard = _engine.BuildDashboard("Ponto alocado em Velocidade!");
            BindingContext = dashboard;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", ex.Message, "OK");
        }
    }

    private async void OnAllocateResistanceClicked(object sender, EventArgs e)
    {
        try
        {
            _engine.AllocatePoint("resistance");
            var dashboard = _engine.BuildDashboard("Ponto alocado em Resistência!");
            BindingContext = dashboard;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", ex.Message, "OK");
        }
    }

    private void InitializeChatbot()
    {
        if (ChatMessages.Children.Count == 0)
        {
            AddChatMessage("🤖 Assistente", "Olá! Sou seu assistente de evolução. Como posso ajudar você hoje? Posso explicar mecânicas do jogo, dar dicas ou responder perguntas sobre seu progresso.", true);
        }
    }

    private async void OnSendMessageClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ChatInput.Text))
            return;

        var userMessage = ChatInput.Text.Trim();
        AddChatMessage("👤 Você", userMessage, false);
        ChatInput.Text = "";

        // Simple chatbot responses
        var response = GenerateChatbotResponse(userMessage);
        await Task.Delay(500); // Small delay for natural feel
        AddChatMessage("🤖 Assistente", response, true);
    }

    private void AddChatMessage(string sender, string message, bool isBot)
    {
        var messageFrame = new Frame
        {
            Padding = 12,
            CornerRadius = 12,
            BackgroundColor = isBot ? Color.FromHex("#2D5A8C") : Color.FromHex("#38B26D"),
            Margin = new Thickness(0, 0, 0, 8)
        };

        var layout = new VerticalStackLayout { Spacing = 4 };
        
        var senderLabel = new Label
        {
            Text = sender,
            FontSize = 11,
            TextColor = Color.FromHex("#A0AEC0"),
            FontAttributes = FontAttributes.Bold
        };
        
        var messageLabel = new Label
        {
            Text = message,
            FontSize = 13,
            TextColor = Color.FromHex("#E2E8F0"),
            LineBreakMode = LineBreakMode.WordWrap
        };

        layout.Children.Add(senderLabel);
        layout.Children.Add(messageLabel);
        messageFrame.Content = layout;

        ChatMessages.Children.Add(messageFrame);

        // Auto scroll to bottom
        var scrollView = ChatMessages.Parent as ScrollView;
        if (scrollView != null)
        {
            scrollView.ScrollToAsync(0, scrollView.Content.Height, false);
        }
    }

    private string GenerateChatbotResponse(string userMessage)
    {
        var message = userMessage.ToLowerInvariant();

        if (message.Contains("treino") || message.Contains("workout"))
        {
            return "💪 Os treinos aumentam seus atributos! Há 3 intensidades: Leve (+1 força, +1 velocidade), Médio (+2 força, +1 resistência) e Pesado (+4 força, +1 velocidade, +2 resistência). Você pode fazer apenas 1 treino por dia!";
        }

        if (message.Contains("economia") || message.Contains("dinheiro") || message.Contains("save"))
        {
            return "💰 Economizar dinheiro dá ouro e XP! Você pode economizar até 3 vezes por dia. Quanto mais economizar, mais ouro ganha!";
        }

        if (message.Contains("loja") || message.Contains("shop") || message.Contains("comprar"))
        {
            return "🛒 Na loja você compra itens únicos! Alguns têm restrições de nível ou classe. Itens comprados vão para seu inventário e desaparecem da loja.";
        }

        if (message.Contains("craft") || message.Contains("criar") || message.Contains("receita"))
        {
            return "⚒️ Crafting cria itens usando compras reais como ingredientes. Cada receita dá atributos e XP. Verifique os requisitos antes de craftar!";
        }

        if (message.Contains("pocao") || message.Contains("potion"))
        {
            return "🧪 Poções são feitas com compras reais. Elas aumentam atributos temporariamente e dão XP. Há 4 tiers com efeitos crescentes!";
        }

        if (message.Contains("atributo") || message.Contains("attribute") || message.Contains("ponto"))
        {
            return "📊 Você ganha pontos de habilidade ao subir de nível. Distribua-os em: Força, Agilidade, Vitalidade, Inteligência, Velocidade, Resistência. Cada um tem cores diferentes na interface!";
        }

        if (message.Contains("nivel") || message.Contains("level") || message.Contains("xp"))
        {
            return "⭐ XP vem de ações diárias. A cada nível você ganha 1 ponto de habilidade. Veja sua barra de progresso XP no perfil!";
        }

        if (message.Contains("missao") || message.Contains("mission") || message.Contains("daily"))
        {
            return "🎯 Missões diárias dão recompensas extras! Complete treinos, economias e compras para ganhar ouro e pontos de habilidade.";
        }

        if (message.Contains("ouro") || message.Contains("gold") || message.Contains("moeda"))
        {
            return "💰 Ouro é a moeda principal. Ganhe com treinos, economias, missões e vendas. Use para comprar itens e craftar.";
        }

        if (message.Contains("inventario") || message.Contains("inventory"))
        {
            return "🎒 Seu inventário mostra itens comprados. Eles dão bônus passivos e mostram seu progresso na jornada de evolução!";
        }

        if (message.Contains("olá") || message.Contains("oi") || message.Contains("hello") || message.Contains("hi"))
        {
            return "Olá! 👋 Estou aqui para ajudar com qualquer dúvida sobre o jogo. Pergunte sobre mecânicas, estratégias ou progresso!";
        }

        if (message.Contains("obrigado") || message.Contains("thanks"))
        {
            return "De nada! 😊 Continue sua jornada de evolução. Se precisar de mais ajuda, é só perguntar!";
        }

        return "🤔 Hmm, não entendi bem sua pergunta. Tente perguntar sobre treinos, economia, loja, crafting, poções, atributos, níveis, missões ou ouro!";
    }
}
