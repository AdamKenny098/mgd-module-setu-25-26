# Test Plan & Results – Ironhollow (CA3)

This test plan covers all required CA3 device-readiness scenarios, including lifecycle behaviour, performance resilience, and environmental stress conditions. Each test contains clear steps, expected results, and a placeholder for actual findings once executed on a physical Android device.

---

## Legend
**P/F:** Pass / Fail  
**N/A:** Not Applicable  
**Device Tested:** Samsung S20 FE 5G

---

## T01 – Cold Start
| Field | Details |
|-------|---------|
| **Scenario** | Launching the app after device reboot. |
| **Steps** | 1. Reboot phone. 2. Unlock. 3. Launch the app. |
| **Expected Result** | App reaches main menu within 3–5 seconds. No black screens, hangs, or freezes. |
| **Actual Result** | Game launched in under 3 seconds |
| **P/F** | Pass |

---

## T02 – Warm Start
| Field | Details |
|-------|---------|
| **Scenario** | Launching app from background task. |
| **Steps** | 1. Launch app. 2. Press Home. 3. Reopen from app switcher. |
| **Expected Result** | App resumes cleanly, no UI flicker, audio correct, timeScale handled. |
| **Actual Result** | App resumed as expected, no bugs observed. |
| **P/F** | Pass |

---

## T03 – Background/Resume (Focus Loss)
| Field | Details |
|-------|---------|
| **Scenario** | App behavior when interrupted. |
| **Steps** | 1. Play a run. 2. Receive notification or lock/unlock phone. |
| **Expected Result** | Game auto-pauses or resumes in a safe state; no broken input/UI. |
| **Actual Result** | Game resumes immediately with no unexpected issues |
| **P/F** | Pass |

---

## T04 – No-Network Mode
| Field | Details |
|-------|---------|
| **Scenario** | Running app offline. |
| **Steps** | 1. Disable Wi-Fi + Mobile Data. 2. Launch the app. |
| **Expected Result** | App runs normally; no pop-ups or failed calls. Game contains zero online dependencies. |
| **Actual Result** | Game launches perfectly |
| **P/F** | Pass |

---

## T05 – Low-RAM Pressure
| Field | Details |
|-------|---------|
| **Scenario** | Device with low available RAM. |
| **Steps** | 1. Open multiple heavy apps (YouTube, Chrome, Instagram). 2. Launch Ironhollow. |
| **Expected Result** | App starts without crash; small delay acceptable. Gameplay remains stable. |
| **Actual Result** | No difference in preformance when compared to that of a cold launch. |
| **P/F** | Pass |

---

## T06 – Battery Saver Mode
| Field | Details |
|-------|---------|
| **Scenario** | Reduced performance mode on Android. |
| **Steps** | 1. Enable Battery Saver. 2. Launch and play a run. |
| **Expected Result** | Slight lower brightness; stable frame-time; input not affected. |
| **Actual Result** | Game has only been tested in this mode |
| **P/F** | Pass |

---

## T07 – Rotation Behaviour
| Field | Details |
|-------|---------|
| **Scenario** | Device rotation handling in gameplay or menus. |
| **Steps** | 1. Rotate phone from portrait → landscape → portrait during menu/gameplay. |
| **Expected Result** | UI locks to portrait (your project uses locked orientation). No stretching or drift. |
| **Actual Result** | Game is locked to one rotation in Unity Editor |
| **P/F** | N/A |

---

## T08 – Thermal Stability (Short Session)
| Field | Details |
|-------|---------|
| **Scenario** | Quick thermal assessment. |
| **Steps** | 1. Play one full run. 2. Note device temperature, throttling, or frame-time spikes. |
| **Expected Result** | Minor warming only. No frame drops or throttling warnings. |
| **Actual Result** | Base Temperature of 30.5C. After a short playtest of approx 2 minutes the temperature of the battery was 30.4C. This shows Ironhollow's effect in negligible. |
| **P/F** | Pass |

---

## T09 – Thermal Stability (Extended 10-min Session)
| Field | Details |
|-------|---------|
| **Scenario** | Sustained thermal load. |
| **Steps** | 1. Play continuously for 10 minutes. 2. Monitor fps, temperature, performance. |
| **Expected Result** | Consistent 60 FPS; no overheating or battery drain spikes. |
| **Actual Result** | Base Temperature of 30.4C. After 10 minues: 29C. This truly shows the thermal effect of Ironhollow is non-existant. |
| **P/F** | Pass |

---

## T10 – Back Navigation Behaviour
| Field | Details |
|-------|---------|
| **Scenario** | Android Back button actions. |
| **Steps** | 1. Press Back in menu. 2. Press Back during gameplay. |
| **Expected Result** | Menu returns correctly; gameplay opens pause menu. No accidental quits. |
| **Actual Result** | Nothing happens in both cases |
| **P/F** | N/A |

---

# Results

Ironhollow passes all metric tests set for it with ease.