# Ironhollow – Mobile Game Development CA2

**Module:** Mobile Game Development  
**Engine & Platform:** Unity 6.2 · Android (IL2CPP · ARM64)  (Unity 6000.2.12.f1) 

**Developer:** Adam Kenny  
**Submission:** CA2 Vertical Slice · Due 16 Nov 2025  
**Version Tag:** v0.2-CA2  

---

## Description

**Ironhollow** is a one mechanic action platform built for mobile.  
The player controls a magnetized entity who continuously moves through a dangerous factory corridor filled with traps and Kamikaze drones.  
Polarity swapping allows attraction or repulsion against magnetic surfaces — the key survival mechanic.

---

## Core Loop
- **Move forward automatically**
- **Tap anywhere** to swap polarity
- **Dodge hazards** and **Manage Momentum** using magnetic push/pull
- **Survive as long as possible** — tracked by distance

---

## Controls

| Action | Input | Description |
|--------|--------|-------------|
| Swap Polarity | Tap Screen / Spacebar (Editor) | Switch between Red and Blue polarity |
| Pause / Resume | Tap Pause / Resume Button | Opens or closes the pause menu |
| Restart | Tap Restart Button | Restarts the level |
| Quit | Tap Quit Button | Exits the game |

---

## Device Target

| Property | Value |
|-----------|--------|
| Target Platform | Android |
| Architecture | ARM64 |
| Graphics API | Vulkan |
| Input | Unity Input System (Touch) |
| Safe Areas | Respected |
| Orientation | Landscape Right |

---

## Technical Implementation

- Input System unified (legacy input removed)  
- Object pooling and event-based state management  
- UI event system verified with touch responsiveness  
- Magnetic interactions filtered by `LayerMask` for performance  
- Optimized cold start time (≈2.7s) by caching references

---

## Performance Summary

| Metric | Reading |
|--------|----------|
| **Frame Time (avg)** | 3.31 ms CPU / 1.13 ms GPU |
| **Frame Rate** | ~60 FPS |
| **GC Alloc / Frame** | 368 B |
| **Total Memory Use** | ~420 MB (Tracked + Reserved) |
| **Draw Calls** | 18 |
| **Triangles / Vertices** | 364 / 630 |
| **Cold Start** | ~2.7 seconds |
| **APK Size** | 47 MB |

**Bottleneck Identified:**  
Input handling conflict between old and new systems (caused UI delay).  
**Fix:** Switched fully to new Input System and reconfigured UI module.  
**Result:** 2× improvement in UI response time, stable 60 FPS.

---

## Telemetry & Economy Stub

Minimal scoring and telemetry system integrated.

| Event | Trigger | Description |
|--------|----------|-------------|
| `level_start` | Scene load | Logs start of main level |
| `enemy_destroyed` | Kamikaze explode | Drone has Collided with something |
| `player_died` | Collision | Tracks player death |
| `session_end` | Quit button | Ends session |

See `Docs/Economy_Telemetry_Map.md` for full details.

---

## AI Assistance

Generative AI such as ChatGPT were used to quicken development time in trivial ways such as:

* Generating the artwork used (Tiles, Background, KamiKaze Drones)
* Trouble shooting why the android JDK and SDK were not in their folder.
* Creating templates for documentation that I then corrected accordingly.
---

## Known Issues – Ironhollow (CA2)

This document lists the known technical and UX issues in the Ironhollow CA2 build (v0.2-CA2).  
All issues were tested on both the Unity Editor and Android build.

---

## Gameplay & Interaction

- **Kamikaze Collision Variance:**  
  Kamikaze enemies sometimes trigger early collisions when spawned very close to walls or spikes.  
  *Status:* Uncommon – visual only.

- **Constant Acceleration:**  
  The player is constantly accelerating due to a force being applied without a max speed. 
  
  *Status:* Can lead to a new challenge but will be fixed.

---

##  Visual / UI

- **Safe Area Padding:**  
  On ultra-wide aspect ratios (e.g., 21:9), pause and quit buttons may appear slightly offset.  
  *Status:* Visual only; all buttons remain functional.

---

## Technical

- **Profiler Connection (Unity Editor):**  
  Android Profiler connection intermittently fails if ADB ports are busy. Not reproducible in final APK.  
  *Status:* Editor-only.

---

## Stable Features
- Polarity swapping (touch + spacebar)
- Pause/resume lifecycle on Android
- Enemy AI and explosion physics
- HUD distance tracking
- Main menu and restart flow
- Stable 60 FPS on simulator and test device

---

