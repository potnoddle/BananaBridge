# Image Restyling & Selection Tracking Specification

This document details the preset style databases, the prompt assembly mechanism, and the selection tracking used by the Gemini Banana Bridge suite.

---

## 1. Category-Specific Style Databases

Instead of a single flat file, the suite embeds multiple category JSON style databases from `StoryGenerator`:
- `styles-digital.json`
- `styles-global.json`
- `styles-other.json`
- `styles-photography.json`

### 1.1 Rich Style Schema
Each style entry maps to a detailed schema including specific visual parameters:
- `base_style.substrate`: Canvas texture, newsprint, cardstock.
- `base_style.rendering`: Pen hatching, swept-gold leaf gilding, digital shading.
- `base_style.pigmentation`: CMYK process, jewel oil-ink, charcoal wash.
- `characteristics.line_quality`: Varying brush tapering, fixed-width ink.
- `characteristics.anatomy`: V-taper silhouettes, realistic musculature.

### 1.2 Multi-Characteristic Prompt Synthesis
When you choose a style, you can select which characteristics (Substrate, Rendering, Coloring, Line Quality, Anatomy) to apply.
The dialog synthesizes a prompt based on:
1. **Gradation Strength Slider:** Determines the intensity guide (subtle hint vs. full transformation).
2. **Selected Visual Checkboxes:** Pulls exact visual descriptions from the JSON.
3. **Overall Style ImagePrompt:** Appends the overall style guide prompt.

---

## 2. Selection-Change Detection via Token Hash

Because Paint.NET does not permit effects to modify document metadata, the plugin tracks edits using local pixel hashing.

### 2.1 The Selection Hash Reset Mechanism
To prevent a previous result from bleeding into a new selection or layer when re-opening the dialog:
1. When the dialog launches, it crops the original pixels of the selection bounding box.
2. It generates a **SHA-256** checksum hash of this byte stream.
3. It compares this hash with `token.SelectionHash`.
4. If they do not match, the plugin realizes you are working on a new selection. It clears `token.PixelColors`, `token.ChatHistory`, and other cached states, forcing the interface to start fresh with the new selection.
5. On OK / **Commit Layer**, the new hash is written back to `token.SelectionHash`.


