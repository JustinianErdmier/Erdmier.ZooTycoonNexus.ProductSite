# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Zoo Tycoon Nexus Product Site — a Blazor WebAssembly (.NET 10) marketing/product site for the Zoo Tycoon Nexus launcher app. The design direction is a Windows 95 / retro
aesthetic (dark background `#14130c`, classic Win95 UI elements), as established in the Zoo Tycoon Nexus Claude Design project.

## Common Commands

> **Always run shell commands via PowerShell — never Bash.**

```powershell
# Run dev server (hot reload)
dotnet watch --project Source/Presentation.Web/Presentation.Web.csproj

# Run without watch
dotnet run --project Source/Presentation.Web/Presentation.Web.csproj
```

**Build:** prefer `mcp__rider__build_solution`. Fallback: `dotnet build Erdmier.ZooTycoonNexus.ProductSite.slnx`.

Dev server listens on `http://localhost:5241` (HTTP) and `https://localhost:7156` (HTTPS). `launchBrowser` is `false` in `launchSettings.json`, so open the URL manually.

## Git commits

Use the **conventional commit** format with a **gitmoji** in the type scope: `type(emoji): subject`.

Type→gitmoji map: `feat`→✨, `fix`→🐛, `refactor`→♻️, `style`→🎨, `docs`→📝, `chore`→🔧, `perf`→⚡, `test`→✅, `build`→📦, `ci`→👷.

Commit per logical task, not per file.

**After each completed task, automatically ask the user whether to commit** (via `AskUserQuestion` or an inline yes/no). Propose the conventional-commit message you would use, list
the files that would be staged, and wait for explicit approval before running `git commit`. Never commit silently and never commit on the user's behalf without that per-task
confirmation.

## Architecture

Single-project Blazor WASM solution (`Microsoft.NET.Sdk.BlazorWebAssembly`), no backend. All code lives in `Source/Presentation.Web/`.

- `Program.cs` — WASM host bootstrapping; registers a scoped `HttpClient` pointed at the base address.
- `App.razor` — root router; uses `MainLayout` as the default layout and `NotFound` as the 404 page.
- `Layout/MainLayout.razor` — currently a pass-through (`@Body` only); add nav/chrome here.
- `Pages/` — route-decorated Razor components (`Home.razor` at `/`, `NotFound.razor` at `/not-found`).
- `Components/` — section components composed by `Home.razor` (nav, hero, features, etc.).
- `Models/` — positional sealed records used as component data models.
- `Common/Constants/` — static constants classes (`Images`, `Links`; public enums: `IniEntryKinds`, `RoadmapStatuses`).
- `Common/Extensions/` — extension methods (`IniEntryKindsExtensions`).
- `wwwroot/index.html` — shell HTML; fingerprinted `blazor.webassembly.js` script tag uses `#[.{fingerprint}]` placeholder handled by the SDK.
- `wwwroot/css/app.css` — global styles (Blazor loading progress, error boundary, form validation helpers).
- `wwwroot/js/site.js` — minimal JS helpers (`scrollToSection`) called via `IJSRuntime` interop.
- `wwwroot/images/icons/brand/` — brand icons (lion, elephant, gate, puma, monogram PNGs). The lion icon is used as the favicon.
- `wwwroot/images/icons/win95/` — Win95-style icon set for the retro UI theme.

`_Imports.razor` and `GlobalUsings.cs` both declare the same `using` namespaces — `_Imports.razor` covers Razor components and `GlobalUsings.cs` covers `.cs` files. Add new
project-wide namespaces to both when needed.

The solution uses the newer `.slnx` format (`Erdmier.ZooTycoonNexus.ProductSite.slnx`), not the legacy `.sln` format.

## Conventions (strict)

- **Nullable reference types** and **implicit usings** are enabled globally.
- Never use scoped CSS (`*.razor.css`) files.
- No test project.
- **One type per file. No exceptions.** Class, record, struct, interface, enum, delegate each in its own file, named after the type, in a folder mirroring the namespace.
- **No files at any project root.** Every file lives under a subfolder mirroring its namespace (e.g. DI registration goes under `Common/Extensions/`).
- **`GlobalUsings.cs`** per assembly consolidates `using` directives. Keep a local `using` only when truly necessary (namespace conflict, alias).
- **File-scoped namespaces** everywhere.
- **British English everywhere** — code comments, XML doc text, identifier wording where there's a choice (`Minimise`, `Initialise`, `Behaviour`, `Colour`), commit messages,
  Markdown docs. Don't switch a US spelling already established by an external API surface (`System.IO`, `Color`, etc.).
- **XML doc comments on every public member and type** (`///`-prefixed `<summary>`, `<param>`, `<returns>`, `<remarks>`, `<exception>` as appropriate). Plain `//` comments are for
  inline implementation notes only.
- **`<c>…</c>` tags carry no inside whitespace.** Write `<c>zoo.ini</c>`, never `<c> zoo.ini </c>`. The same applies to `<code>…</code>`.
- **Spaced bracket style:** `[ STAThread ]`, `[ UsedImplicitly ]`, `[ Parameter ]`. Match the established style.
- **Vertical spacing:** separate consecutive statements of different kinds with a blank line (e.g. an object instantiation followed by a method call, or a local declaration
  followed by a member/static invocation). Keep like statements grouped, with blank lines between groups.
- **UTC for every timestamp.** Column names and properties carry the `Utc` suffix. Localisation only at the UI boundary.
- **Field naming:** instance fields use `_camelCase`; static fields use `s_camelCase` (e.g. `s_name`, `s_submissionState`).
- **Named arguments for clarity:** use named parameters when the argument's purpose is not obvious from context — particularly for string literals, enum values, and multi-parameter calls. Examples: `Split(separator: ' ')`, `Regex.IsMatch(input, pattern: @"…")`, `InvokeVoidAsync(identifier: "scrollToSection", sectionId)`.
- **Pattern matching over equality:** prefer `is` for single-value checks and switch expressions over `==`. Use `var _` (not bare `_`) as the discard arm in switch expressions.
- **Razor HTML attribute formatting:** when an element carries two or more attributes, place each attribute on its own line, indented to align beneath the element name. Self-closing tags keep `/>` on the last attribute line.
- **Suppress IDE anchor warnings:** add `<!--suppress HtmlUnknownAnchorTarget -->` on the line immediately before any `<a href="#section-id">` that targets an in-page anchor, to silence false-positive IDE inspections.
- **Private enums in Razor components:** a `private enum` used exclusively within a single component may be declared inside the `@code` block of that component. All other enums are `public` and live in `Common/Constants/`.
- **Constants naming:** constant members on a static class should be self-describing when read at the call site without needing to see the class name. Prefer `Links.NexusRepoUrl` over `Links.RepoUrl`; `Images.LauncherScreenshot` over `Images.Screenshot`.
- **Column-aligned parameters:** when a positional record (or any multi-parameter signature) spans multiple columns, align the parameter names vertically by padding type names with spaces. Example: `(string Icon, string Title, IReadOnlyList<IniEntry> Entries)` → types left-aligned, names right-aligned in a column.
- **`<see cref>` self-closing:** always write `<see cref="T" />` (space before `/>`) — never `<see cref="T"></see>`.
