# Reflective Development Journal – Ironhollow (Mobile Game Dev CA3)
---
Ironhollow began as a simple endless-runner prototype centred on a polarity-based movement mechanic. The initial concept was small, but as the semester progressed—and following feedback from CA2—it became clear that several systems needed to be redesigned from the ground up. Performance was inconsistent, the game’s architecture was working against me, and new players struggled to understand how to interact with the core loop.

CA3 became less about adding features and more about stabilising the foundations. This journal reflects on the decisions I made, the problems I solved, and—more importantly—the assumptions I challenged. It documents not only what was changed, but why those decisions were made and what I would approach differently if I were starting again.

---
# Early State and Problems Identified
---
Looking back, the CA2 version of Ironhollow suffered from issues that all stemmed from the same root cause: I was building blindly. Without profiling tools, I was effectively guessing where bottlenecks were coming from. I knew the game felt rough on Android, but I didn’t have the evidence to pinpoint where the pain points were.

The heavy use of Instantiate and Destroy in the tunnel generator was one of the first issues that stood out. In retrospect, this was a clear sign of inexperience in mobile optimisation. I knew object pooling existed, but I assumed it would only matter in extreme cases. That assumption was wrong. The constant churn of objects created:

* GC pressure every few frames.

* Obvious stutters during gameplay.

* Gradually degrading performance over longer sessions.

Lifecycle issues were a similar story. When the app was minimised or interrupted, it often came back in a broken state—with duplicated UI, missing input, or a paused game that thought it was still running. Initially, I blamed Android quirks or Unity bugs. Only later did I realise these behaviours were the direct result of not handling OnApplicationPause and OnApplicationFocus correctly.

The absence of persistence was another oversight. I had assumed that a simple endless runner didn’t need to save anything—but on mobile, process kills are normal. Losing a run just because the OS reclaimed memory created a fragile user experience.

Finally, the lack of onboarding meant new players had to figure out the polarity mechanic through trial and error. During early playtests, it became obvious that people didn’t understand how magnetism or movement worked. This wasn't a gameplay problem—it was a communication problem.

---
# Key Improvements and Rationale
---

## Introduction to Object Pooling

Replacing Instantiate and Destroy with a centralised object pool immediately stabilised performance. It eliminated garbage allocations completely during gameplay and removed the stutters caused by the allocator.

This wasn’t just a technical improvement—it was a mindset shift. I stopped thinking like a desktop developer and started thinking like a mobile one. Every allocation matters; every stutter breaks the experience.

Profiling Instrumentation Using ProfilerMarkers

Adding ProfilerMarkers exposed the real issues. Areas I assumed were cheap (input handling, polarity switching) turned out fine, while areas I expected to be lightweight (segment spawning) were actually the worst offenders.

This was the moment I realised that guessing is dangerous. The profiler doesn’t lie, and I should have leaned on it much earlier.

## Correcting the Lifecycle and Pause/Resume Handling

Implementing proper lifecycle management completely changed the stability of the project. The game now:

* Consistently pauses on backgrounding.

* Resumes without duplicating UI.

* Predictably restores input.

* Doesn't get stuck on half-paused states.

Had I understood lifecycle management earlier, I would have avoided many frustrating debugging sessions.

## Lightweight State Persistence

Adding a simple system to store the last run made the game feel more professional. It’s a tiny feature, but it aligns with player expectations on mobile. If the OS kills the game, nothing meaningful is lost.

This change taught me that small UX touches can have huge perceived value.

## Onboarding and First-Run Guidance

The new TutorialManager made onboarding approachable. Rather than dumping instructions onto the screen, the tutorial now appears contextually as the player encounters new mechanics.

The reflective part here is acknowledging that I underestimated how unintuitive polarity was for new users. As the developer, it felt obvious—but players don’t have that context.

## Code Improvements on Safety and Reliability

Caching values like Camera.main, adding null guards, and removing per-frame logging strengthened overall runtime safety. These changes seem minor, but collectively they removed entire classes of bugs.

I learned that defensive coding is not optional—especially under mobile constraints.

## Architectural Redesign to Avoid Floating-Point Drift

This was the biggest pivot. Initially, I let the player move infinitely in the positive X direction. Over time, the camera began to jitter, UI elements shook, and physics became unstable. My first instinct was to blame Cinemachine.

After profiling world coordinates, the cause became obvious: floating-point precision loss.

Redesigning the world to scroll toward the player, rather than moving the player through space, solved the problem cleanly. It also simplified pooling logic dramatically.

## Evidence-Based Analysis of Improvements

Using real profiler data changed how I approached optimisation:

### Before Pooling:

* ~1KB of GC allocations every few frames,

* 8–12 ms stutters,

* Input feeling inconsistent during spikes.

### After Pooling + Cleanup:

* 0 B allocated during game play,

* Smooth frame pacing,

* Consistent physics and input feel,

* Stable FPS even on mid-range Android devices.

These results weren’t a lucky optimisation—they were the product of finally basing decisions on measurable evidence instead of instinct.

---
# Challenges and Lessons Learnt
---
One of the biggest challenges was unlearning assumptions. I often thought, “…this can’t be the issue”, only to discover that the profiler proved otherwise.

## Biggest lessons:

* Profiling should guide decisions, not guesses.

* Lifecycle handling means much more on mobile compared to desktop.

* Architectural problems can mask themselves as UI or camera issues.

* User onboarding isn't optional, even for "simple" games.

* The small UX touches-like persistence-make for drastically improved perceived quality.

Without these realisations, the project would have stayed stuck in CA2 territory—functional, but fragile.

---
# What I Would Have Done Differently
---

Starting over, a number of decisions would be different:

1. Instrument for performance from day one.

Building without profiling was painful. I wasted hours optimising the wrong things simply because I had no measurable insight into what was actually slow.

2. Adopt mobile constraints sooner.

I first designed this as if it were a desktop game. On mobile:

* allocations matter

* lifecycle matters

* memory is relevant.

I internalised this truth only midway through the project.

3. Design the architecture around a stable origin from the beginning.

Letting the player move endlessly in world space seemed harmless until the camera started to shake. If I had understood floating-point precision earlier, I would have built a scrolling-world system from the beginning.

4. Prioritize onboarding as part of core game design.

The polarity mechanic needed explanation. Leaving it as an implicit system hurt early playtests. If the player doesn't understand your mechanic, the problem is communication—not the mechanic.

5. Avoid using DontDestroyOnLoad prematurely.

Using it without a full UI plan caused UI duplication headaches afterwards. I'll make a UI flow more deliberately the next time.

6. Consider persistence to be a base-line feature rather than a luxury.

Losing a run because the OS kills the process is unacceptable. I really should have implemented lightweight save-state functionality ages ago.

7. Testing iteratively rather than with long sprints of coding

A lot of the problems only manifested themselves when on real hardware. Testing on devices sooner would have saved a few missteps.

These reflections are not regrets; they're insights that will directly shape how I approach future mobile projects. 

---

# Conclusion 

---

The CA3 development cycle took Ironhollow from an unstable prototype to a polished, mobile-appropriate vertical slice. Profiling, architectural redesign, lifecycle management, and better onboarding mean the game now performs reliably and clearly communicates its mechanics. More importantly, the project switched my mindset from assumption-driven development to evidence-based engineering. Every major improvement came through observing real behaviour rather than guessing at causes. Ultimately, the reflective process herein-what went wrong, correction of assumptions, and what I would have done differently-is the most valuable part of CA3. It reinforced both the game and my knowledge of professional mobile development practices.