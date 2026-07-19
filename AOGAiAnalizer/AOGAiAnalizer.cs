using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using Google.GenAI;
using Serilog;
using Serilog.Core;


namespace AOGAiAnalyzer;

public class AiAnalyzerConfig : BasePluginConfig
{
    [JsonPropertyName("Enabled")] public bool Enabled { get; set; } = true;
    [JsonPropertyName("Model")] public string Model { get; set; } = "gemini-2.5-flash";
    [JsonPropertyName("APIKey")] public string APIKey { get; set; } = "";
    [JsonPropertyName("Prefix")] public string Prefix { get; set; } = "[AOG Ai Analyzer]";
    [JsonPropertyName("Language")] public string Language { get; set; } = "English";
}

public class AOGAiAnalyzer : BasePlugin, IPluginConfig<AiAnalyzerConfig>
{
    public override string ModuleName { get; } = "AOGAiAnalizer";
    public override string ModuleVersion { get; } = "0.2";
    public required AiAnalyzerConfig Config { get; set; }


    public void OnConfigParsed(AiAnalyzerConfig config)
    {
        Config = config;
    }

    private ILogger _logger = null!;

    public override void Load(bool hotReload)
    {
        if (!Config.Enabled)
            throw new Exception("AiAnalyzer is disabled.");

        if (!CheckApiKeyAsync(Config).Result || string.IsNullOrWhiteSpace(Config.Model))
        {
            _logger.Fatal("API Key is invalid.");
            throw new Exception("API Key is invalid.");
        }

        _logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
            )
            .WriteTo.File(
                "logs/AOGAiAnalyzer.log",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
            )
            .CreateLogger();

        _logger.Information("Plugin loaded. Version: {Version}", ModuleVersion);
    }

    [GameEventHandler]
    private HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
    {
        CCSPlayerController? attacker = @event.Attacker;
        CCSPlayerController? player = @event.Userid;

        if (attacker == null || player == null || attacker == player) return HookResult.Continue;

        string weapon = @event.Weapon;
        bool isAttackerBlind = @event.Attackerblind;
        bool assistedflash = @event.Assistedflash;
        bool thrusmoke = @event.Thrusmoke;
        bool noscope = @event.Noscope;
        bool attackerInAir = @event.Attackerinair;
        bool headshot = @event.Headshot;
        string? assisterPlayerName = @event.Assister?.PlayerName;
        float distance = @event.Distance;
        int dominated = @event.Dominated;
        int penetrated = @event.Penetrated;
        int damage = @event.DmgHealth;

        string log = $"======LOG OF PLAYER {player} DEATH======(it is CS2 game)\n" +
                     $"{attacker} kills {player} by {weapon}. Is attacker blind: {isAttackerBlind}, is assisted flash:" +
                     $" {assistedflash}, trough smoke: {thrusmoke}, noscope: {noscope}, attacker in air: {attackerInAir}" +
                     $", headshot: {headshot}, assisted player name (if assisted): {assisterPlayerName}, distance(float)" +
                     $": {distance}, dominated: {dominated}, penetrated: {penetrated}, damage: {damage}.\n" +
                     $"===END OF {player} DEATH LOG===";

        string prompt =
            "You are a professional CS2 coach, known for sharp analytical feedback with a touch of sarcasm (e.g., 'You're still far from a NAVI invite'). " +
            "Analyze the provided player death log. Provide a concise, constructive critique focusing on the player's actions, positioning, and decisions. " +
            "Keep the tone encouraging but honest, with a slight teasing undertone. " +
            "IMPORTANT: " +
            "1. Do NOT use any Markdown formatting (e.g., **bold**, ## headings, bullet points, etc.). " +
            @"2. Replace any Cyrillic (Russian) characters in your response with Unicode escape sequences (e.g., \u0410 for 'А', \u0430 for 'а'). " +
            "The game chat does not display Cyrillic letters, so only use Latin letters or escapes. " +
            "3. Keep the response short and to the point (max 3-4 sentences). " +
            "4. Base your feedback solely on the data in the log." +
            $"5. Language your speak: {Config.Language}.\n" +
            @"Example of correct output for Russian: \u041f\u0440\u0438\u0432\u0435\u0442";

        Task.Run(async () =>
        {
            try
            {
                _logger.Information("Sending log for player: {string}\n", player.PlayerName);
                string? response = await SendLogAsync(Config, log, [prompt]);
                if (!string.IsNullOrWhiteSpace(response))
                    player.PrintToChat($" {Config.Prefix} {response}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error");
            }
        });

        return HookResult.Continue;
    }

    private static async Task<bool> CheckApiKeyAsync(
        AiAnalyzerConfig? cfg = null
    )
    {
        if (cfg == null) return false;

        string apiKey = cfg.APIKey;
        string model = cfg.Model;


        using HttpClient client = new();

        HttpResponseMessage response = await client.GetAsync(
            $"https://generativelanguage.googleapis.com/v1beta/models/{model}?key={apiKey}");

        return response.StatusCode == HttpStatusCode.OK;
    }

    private static async Task<string?> SendLogAsync(
        AiAnalyzerConfig? cfg, string log, string[]? args
    )
    {
        if (cfg == null) return null;

        string apiKey = cfg.APIKey;
        string model = cfg.Model;

        if (string.IsNullOrWhiteSpace(log))
            return null;

        Client client = new Client(apiKey: apiKey);

        if (args != null && args.Length != 0)
            log = args.Aggregate(log, (current, arg) => current + arg);


        var response = await client.Models.GenerateContentAsync(
            model: model,
            contents: log
        );

        return response?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;
    }
}