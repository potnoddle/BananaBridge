# Build, Deployment, and Verification Guide

This document describes how to compile, deploy, run, and troubleshoot the Gemini Banana Bridge suite of plugins inside Paint.NET.

---

## 1. Prerequisites

* **Development Environment:**
  * [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
  * C# Compiler / MSBuild (via Visual Studio 2022 v17.12+ or the dotnet CLI)
* **Application Host:**
  * [Paint.NET v5.1 or later](https://www.getpaint.net/download.html)
  * Location: Typically installed at `C:\Program Files\paint.net\`

---

## 2. Compiling the Plugins

The solution compiles into two key libraries:
1. `BananaBridge.dll` (Plugin Core / Paint.NET Integration)
2. `BananaBridge.UI.dll` (User Interface / Form Dialogs / Interactive Canvas)

1. Open your terminal at the project root directory:
   `c:\Work\Projects\Paint.Net-Plugings\`
2. Run the build command:
   ```powershell
   dotnet build -c Debug
   ```
   Or for production release:
   ```powershell
   dotnet build -c Release
   ```
3. The build output DLLs will be located at:
   - `BananaBridge\bin\Debug\net9.0-windows\win-x64\BananaBridge.dll`
   - `BananaBridge.UI\bin\Debug\net9.0-windows\BananaBridge.UI.dll`

---

## 3. Deploying the Plugins

### 3.1 Automated Deployment (`install.ps1`)
The easiest method is using the included `install.ps1` script, which automatically checks for configuration overrides and deploys to your Paint.NET Effects directory:
```powershell
.\install.ps1
```
* **Configuration Override Detection:**
  * Checks for `JsonConfigs.Override/` first. If detected, its custom JSON configurations are deployed.
  * If `JsonConfigs.Override/` is not present, the installer falls back to `JsonConfigs/` (the default content).
  * Automatically creates and updates `Effects\JsonConfigs\` in your Paint.NET installation.
* **Target Directory Resolution:**
  * Detects Paint.NET user effects directory: `%USERPROFILE%\Documents\paint.net App Files\Effects\` (standard on modern Windows and ARM64 systems; no administrator elevation required).
  * Detects system directory: `C:\Program Files\paint.net\Effects\` (automatically requests elevation if needed).

### 3.2 Manual Elevated Copy Deployment
If copying files manually:
1. Close Paint.NET.
2. Open PowerShell and copy the build assemblies and configurations:
```powershell
Stop-Process -Name "paintdotnet" -Force -ErrorAction SilentlyContinue
$target = "$env:USERPROFILE\Documents\paint.net App Files\Effects"
Copy-Item "BananaBridge\bin\Release\net9.0-windows\win-x64\BananaBridge.dll" $target -Force
Copy-Item "BananaBridge.UI\bin\Release\net9.0-windows\BananaBridge.UI.dll" $target -Force
Copy-Item "JsonConfigs\*" (Join-Path $target "JsonConfigs") -Recurse -Force
```

### 3.3 Configuration Overrides (`JsonConfigs.Override`)
* **Default Content:** `JsonConfigs/` contains clean default demo configurations tracked by source control.
* **Custom Override:** `JsonConfigs.Override/` is ignored by Git (`.gitignore`) and contains full, extended, or customized JSON presets.
* **Runtime Resolution:** `ConfigService` checks for files in `Effects\JsonConfigs\` and `JsonConfigs.Override\` before falling back to assembly embedded resources.

---

## 4. Verifying the Installation

1. Launch **Paint.NET**.
2. Open or create an image.
3. Select an area using the Rectangle Select, Lasso Select, or Magic Wand tool.
4. Verify the suite of plugins appears under the menu: **Effects > AI Tools**:
   * **AI Style Applier**
   * **AI Image Editor**
   * **AI Add Frame**
   * **AI Asset Creator**
   * **AI Image Creator**
   * **AI Outline Selector & Isolator**
   * **Banana AI Editor**
5. Open any plugin, input your **API Key** in Settings (stored securely using Windows DPAPI), select a model, click **Generate**, and check the preview.
6. Verify that the **[Commit Layer]** button remains disabled until the image has finished generating.
7. Click **Commit Layer** to write the pixels back to your canvas layer.

---

## 5. Troubleshooting & FAQs

### 5.1 The plugins do not appear in the Effects menu
* Ensure you copied *both* `BananaBridge.dll` and `BananaBridge.UI.dll` to `C:\Program Files\paint.net\Effects\`.
* Ensure you are running Paint.NET v5.1 or later. Older versions do not support .NET 9 plugins.
* **Unblock DLLs:** If Windows blocks the files, right-click each DLL in `C:\Program Files\paint.net\Effects\`, select **Properties**, check **Unblock** at the bottom, and click **Apply**.

### 5.2 "Access Denied" when copying files
* You must run your terminal as an Administrator to write files into `C:\Program Files\`. Refer to Section 3.1.
* Ensure Paint.NET is closed so the active DLLs are unlocked.

### 5.3 Commit button is disabled
* This is by design. The **Commit Layer** button is disabled until the generation finishes successfully to prevent blank or empty layer commits.

