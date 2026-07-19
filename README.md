# 🧠 AOGAiAnalizer

[![GitHub stars](https://img.shields.io/github/stars/Artemon0/AOGAiAnalizer?style=flat-square&logo=github&label=Stars)](https://github.com/Artemon0/AOGAiAnalizer/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/Artemon0/AOGAiAnalizer?style=flat-square&logo=github&label=Forks)](https://github.com/Artemon0/AOGAiAnalizer/network/members)
[![GitHub last commit](https://img.shields.io/github/last-commit/Artemon0/AOGAiAnalizer?style=flat-square&logo=github&label=Last%20commit)](https://github.com/Artemon0/AOGAiAnalizer/commits/main)
[![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=csharp&label=Language)](https://github.com/Artemon0/AOGAiAnalizer/search?l=c%23)

[![CounterStrikeSharp](https://img.shields.io/badge/CounterStrikeSharp-important?style=flat-square&label=Framework)](https://github.com/roflmuffin/CounterStrikeSharp)
[![Gemini](https://img.shields.io/badge/Gemini-4285F4?style=flat-square&logo=google&label=Powered%20by)](https://gemini.google.com/)
[![Serilog](https://img.shields.io/badge/Serilog-important?style=flat-square&label=Logging)](https://serilog.net/)
[![Build status](https://github.com/Artemon0/AOGAiAnalizer/actions/workflows/build.yml/badge.svg)](https://github.com/Artemon0/AOGAiAnalizer/actions)

---

## 📖 About

**AOGAiAnalizer** is a CounterStrikeSharp plugin for CS2 that uses the Google Gemini API to provide instant, sarcastic coaching feedback when a player dies. It analyzes death logs and gives short, constructive criticism directly in the game chat — just like a real coach, but with a bit of attitude.

### ✨ Features

- **🧠 AI-Powered Analysis** — Uses Google Gemini to analyze player deaths and suggest improvements
- **⚡ Instant Feedback** — Delivers coaching tips directly in the CS2 chat
- **🎯 Customizable** — Easily change the model, language, and chat prefix
- **🔍 Detailed Logging** — Logs every event for debugging and analysis
- **⚠️ API Key Validation** — Prevents startup if the API key is invalid
- **🌍 Multi-language Support** — Comes with English and Russian localization

---

## 🚀 Installation

### 1️⃣ Prerequisites
- A CS2 server with [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp) installed
- A valid [Google Gemini API key](https://ai.google.dev/)

### 2️⃣ Download & Install
1. Download the latest release from the [Releases page](https://github.com/Artemon0/AOGAiAnalizer/releases) (or clone this repo).
2. Place the `AOGAiAnalizer.dll` file into your server's `csgo/addons/counterstrikesharp/plugins/` directory.
3. (Optional) Create a `lang/` folder and add language files if you want to customize localization.

### 3️⃣ Configuration
The plugin will generate a config file at `csgo/addons/counterstrikesharp/configs/plugins/AOGAiAnalizer/config.json`. Edit it to your liking:

```json
{
  "Enabled": true,
  "Model": "gemini-2.5-flash",
  "APIKey": "your-google-gemini-api-key",
  "Prefix": "[AOG Ai Analyzer]",
  "Language": "English"
}
```

4️⃣ Restart the Server

Restart your CS2 server or use the css_plugins reload command to load the plugin.

---

🎮 Usage

When a player dies, the plugin will:

1. Collect all relevant data (killer, weapon, headshot, distance, etc.)
2. Send a formatted prompt to the Google Gemini API
3. Display the AI's response as a coaching tip in the dead player's chat

Example Output

```
[AOG Ai Analyzer] You were caught wide-peeking on A site. Try using utility to isolate the fight next time. Still far from a NAVI invite, buddy!
```

---

⚙️ Configuration Reference

Key Type Default Description
Enabled bool true Enable or disable the plugin
Model string "gemini-2.5-flash" Google Gemini model to use
APIKey string "" Your Google Gemini API key
Prefix string "[AOG Ai Analyzer]" Prefix displayed before each message
Language string "English" Language for the AI response

---

🛠️ Building from Source

1. Clone the repository:
   ```bash
   git clone https://github.com/Artemon0/AOGAiAnalizer.git
   cd AOGAiAnalizer
   ```
2. Open the solution in Visual Studio or Rider.
3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```
4. Build the project:
   ```bash
   dotnet build -c Release
   ```
5. The compiled .dll will be located in AOGAiAnalizer/bin/Release/net8.0/.

---

📦 Dependencies

· CounterStrikeSharp — The framework for CS2 plugin development
· Google.GenAI — Google Gemini API client
· Serilog — Structured logging library

---

📸 Screenshots

Coming soon!

---

🤝 Contributing

Contributions are welcome! Feel free to:

· 🐛 Report bugs
· 💡 Suggest new features
· 🔧 Submit pull requests

---

📄 License

This project is licensed under the MIT License — see the LICENSE file for details.

---

💬 Connect with the Author

[![Telegram](https://img.shields.io/badge/Telegram-@Artemon0000-26A5E4?style=for-the-badge&logo=telegram)](https://t.me/Artemon0000)
[![GitHub](https://img.shields.io/badge/GitHub-Artemon0-181717?style=for-the-badge&logo=github)](https://github.com/Artemon0)

---

⭐ If you find this plugin useful, please consider giving it a star on GitHub! It helps others discover the project.
