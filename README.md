# Ironhollow

A 2D magnetic survival game built in Unity for Mobile Game Development 2025/2026.  
Ironhollow explores polarity-based movement, magnetic physics, and minimalist industrial design.

---

## Overview

**Ironhollow** is a one-mechanic 2D action platformer where players use magnetic polarity to navigate a collapsing underground facility.  
With every tap, polarity flips — changing whether the player is drawn toward or repelled from nearby metal surfaces.

The game emphasizes **responsive control**, **performance optimization**, and **system-driven gameplay** rather than graphical complexity.  
It was designed to demonstrate strong technical design, reliable physics behavior, and scalable system architecture for mobile and desktop.

---

## Key Features

- **Magnetic Polarity System:**  
  Flip between red and blue charges to attract towards similar and repel agains opposing surfaces.

- **Reactive Physics:**  
  Dynamic 2D forces determine player motion, enemy AI behavior, and environmental hazards.

- **Procedural Level Stitching:**  
  The world extends dynamically as new tunnel segments spawn during gameplay.

- **Kamikaze AI Enemies:**  
  Polarity-sensitive dashing enemies that react to player state.

- **Fully Functional UI:**  
  Main menu, in-game HUD, pause/resume system, and responsive mobile touch interface.

- **Optimized for Android:**  
  Stable 60 FPS on mid-range devices with lightweight draw calls and low memory use.

---

## Development Philosophy

Ironhollow was developed as a **system-first project** — prioritizing physics accuracy, modular code, and efficient runtime behavior.  
It serves as a demonstration of modern Unity best practices for small-scale but technically rich 2D projects.

All gameplay systems were built from scratch without third-party plugins, relying solely on:
- Unity’s **Input System** for cross-platform control
- **URP (2D Renderer)** for efficiency and visual clarity
- **ScriptableObject**-based data for scalability and ease of iteration
- **TextMeshPro** for responsive UI

---

## Technical Breakdown

| Component | Description |
|------------|--------------|
| **Engine** | Unity 6.2 (URP, 2D) |
| **Language** | C# |
| **Platform** | Android (IL2CPP, ARM64), PC |
| **Architecture** | Entity-driven MonoBehaviour system |
| **Input Handling** | Unity Input System |
| **Persistence** | PlayerPrefs (local) |
| **Frame Target** | 60 FPS |
| **Build Size** | <150 MB (Release) |
| **Camera System** | Cinemachine Virtual Camera (Framing Transposer) |

---

## Core Systems

| System | Responsibility |
|---------|----------------|
| **PlayerMagnetController** | Handles player movement, magnetic interactions, and polarity flips |
| **EnemyMagnetController** | Governs stationary magnetic enemies and their reactions |
| **EnemyKamikaze** | Controls pursuit AI and explosive behavior |
| **GameManager** | Global state management (pause, restart, scene flow) |
| **UIManager** | Manages menus, HUD, and user interaction flow |
| **TunnelSpawner** | Dynamically generates procedural tunnel sections |
| **TelemetryManager** | Logs gameplay events (session start, deaths, progress) |

---

## Performance & Optimization

Ironhollow’s systems are optimized to maintain consistent frame-times across mobile hardware:

- Cached all component lookups (`FindFirstObjectByType` replaced by lazy initialization)
- Reduced per-frame memory allocations to under **400 B/frame**
- Batched all static geometry and sprites under shared materials
- Minimized physics overhead by limiting magnetic detection to `LayerMask` filters
- Verified stable **~3.3 ms CPU frame time** and **1.1 ms GPU time** under Unity Profiler

---

## Visual Direction

- Industrial minimalism inspired by decaying machinery and magnetic resonance imagery  
- 2D parallax layers for depth and spatial context  
- Color as feedback — red and blue instantly communicate polarity state  
- Emphasis on silhouette and feedback clarity for mobile readability  

---

## Audio Design

N/A see next build

---

## Development History

**Initial Concept:**  
Born as a prototype for demonstrating custom magnetic force calculations in Unity.  

**Progression:**  
- Core polarity and movement systems implemented first.  
- Enemy AI and environmental hazards added next.  
- Procedural generation, UI systems, and Android lifecycle polish completed last.  

**Goal:**  
Deliver a high-performance, low-overhead 2D vertical slice that could evolve into a full indie release with additional levels and progression systems.

---

## Future Plans

- Introduce boss encounters using polarity-based puzzles
- Introduce more enemy and hazard types
- Integrate persistent save data and difficulty scaling ?

---