Reflective Development Journal – Ironhollow (CA2)

**Module:** Mobile Game Development
**Engine:** Unity 6.2 (URP)
**Platform:** Android (IL2CPP · ARM64)
**Student:** 20102588 / Adam Kenny

**Version:** v0.2.0 / CA2

-

## 1. Introduction

*Ironhollow* is a 2D magnetic survival game, developed as a mobile-optimized vertical slice for the CA2 assessment of Mobile Game Development.

The core mission of this project was to demonstrate technical proficiency under the constraints of mobile: smooth framerates, reliable touch input, and efficient UI lifecycle behavior — all to show a cohesive gameplay loop centered around a single central mechanic, polarity switching.

This reflection is conducted using the framework **Description → Evaluation → Analysis → Action**: the design and technical decisions that developed this prototype to a stable and performant Android build are discussed. Evidence was drawn from Unity Profiler results, editor simulations, and real device testing to validate performance outcomes.

---

## 2. Description

The project started from one simple idea: **magnetism** in 2D physics.

This player is constantly moving right through an industrial tunnel, switching between polarities of Red and Blue.
Matching polarities attract, whilst opposite polarities repel; the whole gameplay loop centers on using this mechanic to avoid hazards, manipulate distance from objects and the player's own speed for surviving as long as possible.
The architecture was deliberately modular:
- **MagnetController** implemented the `IMagnetic` interface, allowing shared logic between player and enemy types.
- Enemies, such as of the **Kamikaze** type, would respond dynamically to polarity states by utilizing physics-based attraction and repulsion.

- A **GameManager** managed all the lifecycle transitions (pause, resume, restart) and scene flow.

- A **UIManager** delivered the main menu, HUD and pause menu, all set up for touch input.

- Finally, a **TunnelSpawner** dynamically generated endless tunnel segments, giving an illusion of procedural progression.

From the outset, the focus was on consistent frame rate and reliable touch response on Android devices using **Unity's new Input System**, minimizing per-frame overhead.

---

## 3. Evaluation

Earlier versions of the prototype worked properly in the editor, but when transferred to mobile, something went wrong. What was happening was input system fragmentation: both the legacy `Input` class and the new Input System were enabled at the same time, causing duplicated event reads and delayed touches. That made for some frustrating first playtests: taps were either ignored or came very late, and sometimes presses on a button wouldn't register at all.

The other problem was the responsiveness of the UI. Whereas the button clicks did work on the desktop, they would not work on actual devices because the **Event System** was missing proper touchscreen bindings. By default, the input system's `InputSystemUIInputModule` is set up for mouse-based interaction, completely disregarding touch events.

Once the new input system and UI system were united, the gameplay started to feel consistent.

The player could flip polarity in an instant with a single tap, even while moving, and the UI no longer required “pre-activation” clicks.
Performance profiling provided further insight. Using the Unity Profiler, the editor simulator reported:
- **CPU time:** ~3.3 ms

- **GPU time:** ~1.1 ms

- **Frame rate:** ~60 FPS

- **Garbage Allocations:** ~368 B/frame

- **Draw Calls:** ~18

- **Cold Start:** 2.7 seconds

These values were excellent for a mobile-targeted build.

---

## 4. Analysis

The major lesson that came from this development process was the **importance of input handling and performance profiling under actual mobile conditions**. Unity's Editor simulator only approximates Android performance, and the differences in event life cycle and GC behavior became apparent once deployed to a device.

Migrating fully to the **new Input System** solved not only the tap recognition but also allowed unified handling for both keyboard and touchscreen testings.

Another great insight was the **AI and magnetic force interaction system.** Kamikaze enemies were diving towards the player using `MoveTowards`, bypassing the physics of a Rigidbody and completely ignoring polarity during an attack.

Dashes now became subject to magnetic influence by refactoring them to apply directional forces. This fix made the polarity mechanic universal — affecting all entities equally — and greatly improved player feedback.

Performance-wise, the real challenge was to ensure that **game logic did not exceed Unity's 16.6 ms frame-time budget** on mobile hardware. Rather than rely on physics recalculation per every frame, I constrained detection ranges, used LayerMasks to reduce overlap checks, and replaced `Find` methods with cached references.

CPU frame-time consequently stabilized at around **3.3 ms**, leaving ample GPU headroom for rendering and UI. Garbage allocations dropped below 400 bytes per frame, a strong indicator that object pooling and reference caching were working as intended.

On the design side, the most crucial finding was that **camera behavior directly impacts player reaction time**. The original composition of Cinemachine kept the camera centered on the player, allowing them very little time to react to oncoming enemies. Anchoring the player to the left quarter of the frame allowed me to extend their visible horizon and vastly improve overall game readability without touching core movement speed. That one small UX adjustment proved more impactful than any mechanical tweak, and further demonstrated the value of thoughtful camera composition in fast-action mobile design.

Finally, lifecycle stability was validated via repeat pause/resume and quit cycles under Android. The game retained state and UI flow seamlessly, proving that the pause system correctly freezes physics simulation and input handling during background states-a keystone of mobile reliability.

---

## 5.  Action

Based on this review, several concrete steps were taken to revise *Ironhollow* into a CA2-ready build:

1. **Unified Input Architecture:** Removed all legacy input references and migrated fully to new Input System. Added dedicated bindings for touchscreen input: 'Tap' and 'TouchPress'. Ensured that mobile and desktop environments function identically.

2. **Performance Optimization:** Caching object lookups (e.g., Player, TunnelSpawner). Lazy initialization patterns were also introduced. This reduced micro-stutter and improved GC performance.

3. **Responsiveness of UI:** Added coroutine to delay UI activation by one frame on startup that solved a "first tap ignored" issue caused by EventSystem initialization.

4. **Magnetic Consistency:** Rewrote Kamikaze behavior to use forces against a Rigidbody while dashing so that polarity physics remains active during the movement.

5. **Camera Reframing:** Adjusted Cinemachine follow target to frame player in the left quarter of the screen, so as to improve visibility and reaction time.

6. **Profiling Validation:** Used the built-in simulator to reproduce the performance of phones; then confirmed that the frametime, cold start, and allocation behaviors are stable.

These refinements collectively transformed *Ironhollow* from a functional prototype into a **stable, cohesive, and responsive vertical slice** that could demonstrate technical mastery within mobile constraints.

---

## 6. Summary of Evaluation

| **Category** | **Target** | **Result** | **Status** |

|---------------|-------------|-------------|-------------|

| Frame-time Stability | <16.6 ms | 3.31 ms avg | ✅ |

| Garbage Allocations | <1 KB/frame | 368 B/frame | ✅ |

| Touch Responsiveness | <2 frames delay | Achieved | ✅ |

| Lifecycle Behavior | Stable across focus loss | Achieved | ✅ |

| Physics Consistency | Magnetic forces universal | Achieved | ✅ |

| UI Responsiveness | No tap delay | Achieved | ✅ |

| Camera Framing | Better visibility | Accomplished | ✅ |

---
## 7. Learning Outcomes

Technical Competence:
It reinforced for me how much mobile optimization is all about the removal of redundancy: creating a well-written script isn't just correctness, it's *frequency*. With one misplaced `Find()` or redundancy in allocation, frame-time can become destabilized. Profiling provided actionable insight into where resources were being wasted and how to fix it efficiently.


Design Understanding: This project showed that readability in gameplay is as much a matter of visual framing and pacing as of mechanics. Moving the position of the player within the camera view fundamentally changed the feel of the game, making spatial anticipation a key design element. **Mobile-Specific Insight:** Testing on hardware revealed some unique constraints, especially regarding Unity's UI event lifecycle. While the desktop builds can tolerate focus loss and cursor resets rather gracefully, mobile requires explicit lifecycle management to prevent soft locks from occurring or input events from being missed. - 

## 8. Forward Planning 

The next development stage (CA3) will be about implementing light progression and long-term replayability through the following objectives: 

| **System** | **Planned Upgrade** | **Purpose** | 
|-------------|--------------------|-------------| 
| **Telemetry System** | Removal due to redundancy. | Minimum Size | 
**Level Generation** | Introduce more segments of the tunnel | Increase variety and scaling difficulty | 
| **UX Feedback** | Add queues that give feedback - Improve tactile and sensory response | 
| **UI** | Refine UI to give it a more polished and metallic look. | Improve player experience. 
| **Audio** | Add in some music and sfx | Improve player experience | 
| **Player Movement** | Add a max speed to the player | Gives the player more reaction time / breathing room | 
| **Hazards** | Add in more hazards ie Explosive tiles or molten metal | Add more variety and difficulty to the game | 
| **Boss Encounter** | Include a miniboss - KamiKaze factory | For the marks 
---