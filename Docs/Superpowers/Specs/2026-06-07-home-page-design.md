# Home Page — Implementation Design

- **Date Written:** 2026-06-07
- **Status:** Approved

## Overview

Implement the Zoo Tycoon Nexus marketing landing page (`Zoo Tycoon Nexus.html` from the Claude Design project) as a Blazor WebAssembly Standalone application. The design uses a
warm-paper / retro Win95 aesthetic. All CSS is already present in `wwwroot/css/app.css`. Only Hero B is implemented; Hero A is out of scope.

Root namespace: `Erdmier.ZooTycoonNexus.Presentation.Web`

---

## Architecture

`Home.razor` (at route `/`) composes all section components in document order, wrapped in `<div class="nexus">`. `MainLayout.razor` remains a pass-through (`@Body` only). No
routing changes are needed.

---

## File Layout

```
Source/Presentation.Web/
├── Common/
│   ├── Constants/
│   │   ├── IniEntryKinds.cs
│   │   └── RoadmapStatuses.cs
│   └── Extensions/
│       └── IniEntryKindsExtensions.cs
├── Components/
│   ├── BetaCallToActionSection.razor
│   ├── ContributeSection.razor
│   ├── EmailSignupForm.razor
│   ├── FeaturesSection.razor
│   ├── Hero.razor
│   ├── HowItWorksSection.razor
│   ├── ProblemsSection.razor
│   ├── RoadmapSection.razor
│   ├── SiteFooter.razor
│   └── SiteNav.razor
├── Models/
│   ├── FeatureItem.cs
│   ├── HelpWayItem.cs
│   ├── IniEntry.cs
│   ├── ProblemItem.cs
│   ├── RoadmapItem.cs
│   └── StepItem.cs
├── Pages/
│   └── Home.razor              ← updated
└── wwwroot/js/
    └── site.js                 ← new
```

`_Imports.razor` gains `@using` directives for `...Components` and `...Models`.
`GlobalUsings.cs` gains the same two namespaces for `.cs` files.

---

## Data Models

All records use positional parameters. Collections are never nullable; an empty list signals absence.

| File             | Type     | Members                                                                                        |
|------------------|----------|------------------------------------------------------------------------------------------------|
| `ProblemItem.cs` | `record` | `Title`, `Description`, `Fix`                                                                  |
| `FeatureItem.cs` | `record` | `Icon`, `Title`, `Description`, `IReadOnlyList<IniEntry> Entries` (defaults to `[]`)           |
| `IniEntry.cs`    | `record` | `Key`, `Value`, `IniEntryKinds Kind`                                                           |
| `StepItem.cs`    | `record` | `Title`, `Description`                                                                         |
| `HelpWayItem.cs` | `record` | `Icon`, `Title`                                                                                |
| `RoadmapItem.cs` | `record` | `Icon`, `RoadmapStatuses Status`, `Title`, `Description`, `IReadOnlyList<string> BulletPoints` |

### Enums (in `Common/Constants/`)

**`IniEntryKinds`** — `Key`, `Ok`, `Value`

**`RoadmapStatuses`** — `Planned`, `NextUp`

### Extensions (in `Common/Extensions/`)

**`IniEntryKindsExtensions`** — `MapToCssToken(this IniEntryKinds kind) : string` returns `"k"` for `Key`, `"ok"` for `Ok`, and `"v"` for `Value`.

---

## Components

### `EmailSignupForm.razor`

**Parameters:** `string CallToAction = "Request beta access"`, `bool Compact = false`

**State:** A `private enum SubmissionStates { Idle, ValidationError, Submitted }` defined in the same file. Email validated with a regex on submit. On success, state transitions to
`Submitted` and field values are discarded (no persistence). The `Compact` parameter suppresses the name field.

CSS class on the `<form>` element is computed from state: base `signup`, plus `done` when `Submitted`, `err` when `ValidationError`.

### `SiteNav.razor`

Sticky header. Injects `IJSRuntime`. All anchor `<a>` elements use `@onclick` to call `scrollToSection` via JS interop instead of default browser anchor navigation. "Join the beta"
button scrolls to `#beta`.

### `Hero.razor`

Two-column grid (`hero-grid`): copy column and showcase column. Renders `<EmailSignupForm />` inline. No parameters. Static markup only.

### `ProblemsSection.razor`

`private static readonly ProblemItem[] Problems` defined in `@code`. Loops to render problem cards. Each card: tag pair (Issue → Fixed), heading, description, fix row with check
icon.

### `FeaturesSection.razor`

`private static readonly FeatureItem[] Features` in `@code`. Loops to render feature cards. When `Entries` is non-empty, renders the INI snippet block; CSS class on the value
`<span>` is provided by `kind.MapToCssToken()`.

### `HowItWorksSection.razor`

`private static readonly StepItem[] Steps` in `@code`. Three-column step grid.

### `RoadmapSection.razor`

`private static readonly RoadmapItem[] RoadmapItems` in `@code`. Two items (Elephant/Save management, Gate/Mod management). Loop renders cards. The status pill's CSS class and
label text are derived from `Status`:

- `Planned` → class `"planned"`, label `"Planned"`
- `NextUp` → class `"beta"`, label `"Next up"`

`BulletPoints` that contain inline HTML are rendered as `MarkupString` (safe: all content is static, developer-authored).

### `ContributeSection.razor`

`private static readonly HelpWayItem[] HelpWays` in `@code`. Loops to render help-chips. Repo URL is a `private const string` set to the placeholder
`https://github.com/zoo-tycoon-nexus/nexus`. Win95 terminal mock is a static markup. Two action buttons link to `RepoUrl` and `RepoUrl + "/contribute"`.

### `BetaCallToActionSection.razor`

Dark CTA section. Wraps `<EmailSignupForm CallToAction="Get my key" />` inside a Win95 dialogue chrome (`.beta-dialog`). No parameters.

### `SiteFooter.razor`

Static footer. Injects `IJSRuntime` for the Product and Community link columns (which smooth-scroll to sections). Ko-fi image link. Disclaimer text.

---

## JS Interop

**`wwwroot/js/site.js`**

```js
window.scrollToSection = function (id) {
    const element = document.getElementById(id);
    if (element) {
        window.scrollTo({ top: element.getBoundingClientRect().top + window.scrollY - 54, behavior: "smooth" });
    }
};
```

Referenced via `<script src="js/site.js"></script>` in `wwwroot/index.html` (before the Blazor bootstrap script). Called from `SiteNav` and `SiteFooter` as
`await JsRuntime.InvokeVoidAsync("scrollToSection", sectionId)`.

---

## Icon / Asset Path Reference

All image paths are resolved through the `Images` static constants class (`Common/Constants/Images.cs`) rather than being hardcoded as strings. Components reference paths via `Images.Icons.Brand.*`, `Images.Icons.Win95.*`, `Images.KofiBanner`, and `Images.LauncherScreenshot`.

| Design mapping key    | `Images.*` constant                                    |
|-----------------------|--------------------------------------------------------|
| `A.logo`              | `Images.Icons.Brand.Monogram`                          |
| `A.lion`              | `Images.Icons.Brand.Lion`                              |
| `A.eleph`             | `Images.Icons.Brand.Elephant`                          |
| `A.rhino`             | `Images.Icons.Brand.Puma`                              |
| `A.gate`              | `Images.Icons.Brand.Gate`                              |
| `A.shot`              | `Images.LauncherScreenshot`                            |
| `A.kofi`              | `Images.KofiBanner`                                    |
| `info.ico`            | `Images.Icons.Win95.Information32Gif`                  |
| `mail-check.ico`      | `Images.Icons.Win95.Mail24`                            |
| `my-computer.ico`     | `Images.Icons.Win95.MyComputer16`                      |
| `install-discs.ico`   | `Images.Icons.Win95.InstallationOnComputer16`          |
| `check.ico`           | `Images.Icons.Win95.Ok32`                              |
| `search.ico`          | `Images.Icons.Win95.SearchComputer16`                  |
| `install-stopped.ico` | `Images.Icons.Win95.NoInstallationOnComputer32`        |
| `controls.ico`        | `Images.Icons.Win95.Controls32`                        |
| `config-sheet.ico`    | `Images.Icons.Win95.ConfigSheet16`                     |
| `folder-catalog.ico`  | `Images.Icons.Win95.FolderCatalog16`                   |
| `exe.ico`             | `Images.Icons.Win95.Window16`                          |
| `mystify.ico`         | `Images.Icons.Win95.Mystify16`                         |
| `sheets-docs.ico`     | `Images.Icons.Win95.SheetsAndDocuments32`              |
| `help-page.ico`       | `Images.Icons.Win95.TextDocument32`                    |

---

## Out of Scope

- Hero A (`HeroA` / `.heroA` styles) — ignored
- `localStorage` persistence for the email form — form state is in-memory only
- Any backend or API integration
