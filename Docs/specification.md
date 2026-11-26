# Technical Specification: Gemini Banana Bridge Suite

This document outlines the architectural specifications, class hierarchy, interfaces, and metadata strategies for the Gemini Banana Bridge suite of Paint.NET plugins.

---

## 1. Environment & Dependencies

* **Target Host:** Paint.NET v5.1+
* **Target Runtime:** .NET 9.0 (Windows-compatible)
* **Framework:** Windows Forms (`net9.0-windows`)
* **Libraries:**
  - `PaintDotNet.Base.dll`
  - `PaintDotNet.Core.dll`
  - `PaintDotNet.Data.dll`
  - `PaintDotNet.Effects.dll`
  - `PaintDotNet.Primitives.dll`
  - `PaintDotNet.PropertySystem.dll`
  - `System.Drawing.Common` (v9.0.1)
  - `System.Security.Cryptography.ProtectedData` (v9.0.0)

---

## 2. Core Architecture

The suite consists of seven specialized plugins that interact with the Google GenAI Gateway:

```
        +-------------------------------------------------------+
        |                  Paint.NET Interface                  |
        +-------------------------------------------------------+
            | (Active Selection / Mask)     ^ (Updated Layer)
            v                               |
+-----------------------------------------------------------------+
|               BananaBridge.dll (Plugin Core)                    |
|                                                                 |
|  - AI Style Applier            (Style preset transformer)       |
|  - AI Image Editor             (Conversational inpainting)      |
|  - AI Add Frame                (Scene border frame tool)        |
|  - AI Asset Creator            (Characters, locations, props)   |
|  - AI Image Creator            (Scenic & composition generator) |
|  - AI Outline Selector/Isolator (Contour cutout & sticker maker)|
|  - Banana AI Editor            (Unified editing workspace)      |
+-----------------------------------------------------------------+
               | HTTP POST                           ^
               | (Image + Mask + Settings)           | JSON Response
               v                                     | (Base64 Image)
        +-------------------------------------------------------+
        |                 Google GenAI Gateway                  |
        +-------------------------------------------------------+
```

---

## 3. Plugin Architecture

### 3.1 Class `AiStyleEffect`
* **Menu Name:** `"AI Style Applier"`
* **Purpose:** Applies specialized art styles (Digital, Global, Pop Art, Vintage Film) loaded from embedded JSON files.
* **Token:** `AiStyleToken` (Category, StyleId, Strength 10%-100%, PixelColors array).
* **Strength Gradation:** Adjusts the prompt structure from a subtle hint to a full rewrite depending on the slider.

### 3.2 Class `AiEditEffect`
* **Menu Name:** `"AI Image Editor"`
* **Purpose:** Enables conversational canvas editing with an interactive masking control.
* **Token:** `AiEditToken` (ChatHistory, MaskPixelColors, PixelColors).
* **Control:** `InteractiveCanvas` allows drawing/erasing white/black pixels on the mask overlay while supporting canvas panning and zooming.

### 3.3 Class `AiAddFrameEffect`
* **Menu Name:** `"AI Add Frame"`
* **Purpose:** Places style-specific borders matching the frames configuration.
* **Token:** `AiAddFrameToken` (StyleId, FrameWeight 2%-30%, TransparentCenter).
* **Center Transparency:** If enabled, clears all pixels in the center region of the selection bounds, allowing the border to stand alone on a transparent layer.

### 3.4 Class `AiAssetCreatorEffect`
* **Menu Name:** `"AI Asset Creator"`
* **Purpose:** Multi-tab generator creating standalone asset sprites, characters, locations, and props.
* **Token:** `AiAssetCreatorToken` (SelectedTab, StyleCategory, StyleName, SpeciesName, GenderName, PersonalityName, FashionName, AccessoryName, PaletteName, PoseName, LocationTypeName, StructureName, VisualStateName, AtmosphereName, PropName, MaterialName, ConditionName, PixelColors).
* **Features:** Supports saving and loading character/environment configuration presets (`.json`).

### 3.5 Class `AiImageCreatorEffect`
* **Menu Name:** `"AI Image Creator"`
* **Purpose:** Generates full illustrations from scratch or selection bounds using classical composition armatures.
* **Token:** `AiImageCreatorToken` (Prompt, StyleCategory, StyleName, CompositionLayoutName, ScaleToFit, CopyToClipboard, PixelColors).
* **Composition Armatures:** Injects classical composition frameworks (Rule of Thirds, Triangular Composition, Central Symmetry) loaded from `layout.json`.

### 3.6 Class `AiIsolatorEffect`
* **Menu Name:** `"AI Outline Selector & Isolator"`
* **Purpose:** Segments and extracts foreground subjects onto transparent backgrounds with optional boundary contour strokes.
* **Token:** `AiIsolatorToken` (Prompt, DrawOutline, OutlineColor, OutlineWidth 1-10px, ScaleToFit, CopyToClipboard, PixelColors).
* **Contour Outlining:** Automatically traces alpha boundaries and renders custom color outlines (ideal for sticker cutouts and asset isolation).

### 3.7 Class `BananaEffect`
* **Menu Name:** `"Banana AI Editor"`
* **Purpose:** Legacy unified conversational editing and style transformation environment.
* **Token:** `BananaToken` (ApiKey, Model, ChatHistory, PixelColors, AppliedStyles, ScaleToFit, GenerateFromScratch).

---

## 4. Configuration Resolution & Override System

To provide clean demo defaults while supporting full customized presets, configurations follow a prioritized resolution hierarchy orchestrated by `ConfigService`:

### 4.1 Configuration Directories
1. **`JsonConfigs/` (Defaults Content):**
   * Default repository configurations tracked in source control.
   * Curated to minimal, representative samples for rapid demonstration.
2. **`JsonConfigs.Override/` (Custom Overrides):**
   * Private, local directory ignored by source control (`.gitignore`).
   * When present, `install.ps1` and build routines detect this folder and prioritize its files over `JsonConfigs/`.

### 4.2 Resolution Hierarchy (`ConfigService`)
When any dialog requests a configuration file (e.g. `comicbook-styles.json` or `styles-digital.json`), `ConfigService` resolves the stream in order:
1. `Effects\JsonConfigs.Override\<filename>` (next to running assembly)
2. `Effects\JsonConfigs\<filename>` (installed configuration folder)
3. `%APPDATA%\BananaBridge\JsonConfigs\<filename>` (user roaming application data)
4. Assembly Embedded Resource: `BananaBridge.UI.<filename>` within `BananaBridge.UI.dll`


