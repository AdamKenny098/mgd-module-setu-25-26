# Ironhollow
A 2D magnetic survival game built in Unity for Mobile Game Development 2025/2026.

Ironhollow is a fast-paced polarity-based survival runner set inside a collapsing underground facility.
The player flips magnetic polarity to attract to or repel from metal surfaces, navigating increasingly dangerous tunnels while avoiding hazards and managing moment-to-moment movement decisions.

This project demonstrates strong technical design, efficient 2D physics behaviour, mobile optimisation, and system-driven gameplay.

---

## Overview

Ironhollow centres around a single, expressive mechanic: magnetic polarity.
Every tap flips the player between red and blue states, directly affecting movement, boosts, and hazard interaction.

Post-CA3 development focused on stability, onboarding, lifecycle correctness, performance improvement, and the architectural changes needed to turn the CA2 prototype into a polished mobile-ready vertical slice.

---

## Key Features

- **Magnetic Polarity System**  
  Flip between polarities to attract to or repel from nearby metal surfaces.

- **Responsive Magnetic Physics**  
  Smooth boosts, redirection, and wall interactions driven by dynamic magnetic forces.

- **Procedural Tunnel Generation**  
  Pooled segments create an endless run without stutter or memory spikes.

- **Tuned Hazards & Magnetic Enemies**  
  Includes six types of foes along with a boss battle, stationary emitters, and ranged hazards with clear telegraphs.

- **Mobile-Optimised UI**  
  First-run tutorial, pause/resume, safe-area support, and scalable HUD.

- **Performance-Focused Architecture**  
  Zero allocations in gameplay, stable 60 FPS on mid-range Android hardware.

---

## Technical Breakdown

| Component | Details |
|----------|---------|
| Engine | Unity 6.2 (URP 2D Renderer) |
| Language | C# |
| Platform | Android (IL2CPP, ARM64) and PC |
| Architecture | Scrolling world + pooled procedural generation |
| Input | Unity Input System (touch controls) |
| Camera | Cinemachine (Framing Transposer) |
| Persistence | PlayerPrefs (run summary) |
| Target Frame Rate | 60 FPS |
| Build Size | <150 MB |

---

## Core Systems

| System | Purpose |
|--------|---------|
| PlayerMagnetController | Controls player motion and magnetic interactions |
| StaticMagneticEmitters | Area-based influence on player polarity |
| TunnelSpawner | Fully pooled segment generation (no Instantiates during gameplay) |
| GameManager | Handles pause, restart, state transitions, lifecycle recovery |
| UIManager | Menus, HUD, scaling, and general UI flow |
| TutorialManager | First-run contextual onboarding |

---

## Performance & Optimization

Final CA3 profiling results:

- Zero per-frame allocations in gameplay  
- Stable frame pacing on mid-range Android devices  
- Magnetic force calculations optimised with cached masks and squared magnitude checks  
- Reduced overdraw from simplified sprites and limited particle usage  
- Physics queries restricted to specific layers for efficiency  
- CPU frame time: ~3.0–3.5 ms  
- GPU frame time: ~1.0 ms  

These improvements were validated using Unity Profiler with custom ProfilerMarkers.

---

## Visual Direction

- Industrial minimalism and decaying machinery motifs  
- Red/blue colour language for instant polarity readability  
- High-contrast silhouettes for hazard clarity on mobile  
- Controlled use of parallax for depth without visual clutter  

---

## Development History

Ironhollow began as a physics prototype demonstrating magnetic force calculations.
Throughout CA2 and CA3, the project evolved significantly:

- Added object pooling to eliminate runtime allocations  
- Redesigned architecture to keep the world centred on origin (fixing camera drift)  
- Added onboarding and tutorial flow  
- Implemented correct Android lifecycle handling  
- Tuned hazard difficulty curve and projectile speeds  
- Improved UI scaling and readability  
- Added persistent run summary  
- Conducted an 11-player playtest and applied feedback-driven changes  

Ironhollow now represents a stable, polished 2D vertical slice demonstrating modern Unity mobile development techniques.

---

## Status

**Ironhollow – CA3 Version: Complete**  
A technically stable, well-optimised mobile-ready vertical slice showcasing a unique polarity movement system and robust CA3 implementation.
