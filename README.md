# BananaBridge: Gemini AI Plugin Suite for Paint.NET

BananaBridge is an AI-powered plugin suite for [Paint.NET](https://www.getpaint.net/) that bridges your canvas directly to Google's Gemini GenAI vision and image models.

All plugins appear conveniently under the **Effects > AI Tools** menu in Paint.NET.

---

## 🎨 Suite of 7 AI Plugins

1. **AI Style Applier (`AiStyleEffect`)**
   * One-shot artistic style transfer using curated style databases (Digital, Global & Indigenous, Pop Art, Photography & Realism, etc.).
   * Fine-tune transformations with multi-characteristic checkboxes (Substrate, Rendering, Pigmentation, Line Quality, Anatomy) and a gradation strength slider.

2. **AI Image Editor (`AiEditEffect`)**
   * Multi-turn conversational image inpainting and editing.
   * Features an interactive pan/zoom canvas (`InteractiveCanvas`) with customizable brush and eraser tools for mask overlays.

3. **AI Add Frame (`AiAddFrameEffect`)**
   * Generates style-matched ornate border frames around selections or entire images.
   * Adjustable frame weights (2% to 30%) with optional center transparency for overlaying on separate layers.

4. **AI Asset Creator (`AiAssetCreatorEffect`)**
   * Multi-tab generative sprite and concept asset creation tool for characters, locations, and standalone props.
   * Granular building blocks (Species, Fashion, Personalities, Visual States, Atmospheres, Materials) with JSON preset saving/loading.

5. **AI Image Creator (`AiImageCreatorEffect`)**
   * Generates complete illustrations and scenes from scratch or selection bounds.
   * Injects classical compositional armatures (Rule of Thirds, Triangular Composition, Dynamic Symmetry, Central Focus) from compositional schemas.

6. **AI Outline Selector & Isolator (`AiIsolatorEffect`)**
   * Extracts foreground subjects onto transparent backgrounds.
   * Automatically traces alpha boundaries and renders customizable contour outline strokes (ideal for sticker cutouts and isolated assets).

7. **Banana AI Editor (`BananaEffect`)**
   * Unified conversational editing and style transformation workspace.

---

## ⚙️ Configuration & Override System

BananaBridge utilizes a prioritized configuration system managed by `ConfigService`:

* **Default Configurations (`JsonConfigs/`):** Clean, minimal demo samples tracked in the public repository for rapid evaluation.
* **Custom Overrides (`JsonConfigs.Override/`):** Local, private configuration presets (ignored by Git via `.gitignore`).
* **Resolution Priority:**
  1. `Effects\JsonConfigs.Override\` (local custom override next to plugin DLL)
  2. `Effects\JsonConfigs\` (installed configuration folder)
  3. `%APPDATA%\BananaBridge\JsonConfigs\` (roaming user configuration)
  4. Embedded DLL resources within `BananaBridge.UI.dll` (fallback)

---

## 🚀 Installation & Deployment

### Automated Installation (`install.ps1`)
Run the included PowerShell installer from the repository root:
```powershell
.\install.ps1
```
* Automatically detects your Paint.NET Effects directory:
  * User Effects: `%USERPROFILE%\Documents\paint.net App Files\Effects\` (standard on modern Windows and ARM64 systems; no admin privileges required).
  * System Effects: `C:\Program Files\paint.net\Effects\` (automatically elevates if necessary).
* Automatically checks for `JsonConfigs.Override/` and deploys your custom configurations.

### Manual Installation
Copy the compiled assemblies and configurations to your Paint.NET `Effects` folder:
```powershell
$target = "$env:USERPROFILE\Documents\paint.net App Files\Effects"
Copy-Item "BananaBridge\bin\Release\net9.0-windows\win-x64\BananaBridge.dll" $target -Force
Copy-Item "BananaBridge.UI\bin\Release\net9.0-windows\BananaBridge.UI.dll" $target -Force
Copy-Item "JsonConfigs\*" (Join-Path $target "JsonConfigs") -Recurse -Force
```

---

## 🔑 Setup & API Key Security

1. In Paint.NET, open any plugin under **Effects > AI Tools**.
2. Click **Settings...** and enter your Google Gemini API key.
3. API keys are encrypted locally using Windows Data Protection API (DPAPI) and stored in `%APPDATA%\BananaBridge\`.

---

## 📋 Requirements

* Windows 10/11 (x64 or ARM64)
* [Paint.NET v5.1+](https://www.getpaint.net/)
* [.NET 9.0 Runtime](https://dotnet.microsoft.com/download/dotnet/9.0)
* Google Gemini API Key

---

## 📄 License

MIT License. See [LICENSE](LICENSE) for details.
