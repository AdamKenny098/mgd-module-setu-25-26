# Bug List – Ironhollow (CA3)

This section documents known bugs discovered during testing, including severity, reproduction steps, expected behaviour, actual behaviour, resolution status, and any workarounds implemented.

---

## Bug Tracking Table

| ID | Title | Severity | Environment | Steps to Reproduce | Expected | Actual | Status | Workaround |
|----|--------|----------|-------------|---------------------|----------|--------|---------|------------|
| BUG-01 | Pause Menu Overlays After Resume | Major | Samsung S10 5G | Launch → Play → Home → Resume | Game resumes in paused state cleanly | Pause UI overlaps gameplay for 1 frame | Fixed | N/A |


---

## Bug Write-Ups

### **BUG-01 — Pause Menu Overlays After Resume**
**Severity:** Major  
**Environment:** Samsung S10 5G  
**Reproduction Steps:**  
1. Launch app  
2. Play any run  
3. Press Home  
4. Return to app  
**Expected:** Game resumes paused with clean UI layering  
**Actual:** Pause overlay briefly appears twice  
**Resolution:** Fixed by adjusting OnApplicationFocus → PauseGame logic  
**Workaround:** N/A  

---



# Summary
This bug list documents all issues found during editor and device simulation testing. High-impact issues were fixed, medium issues mitigated, and low-impact UI or editor-only issues were logged with appropriate workarounds.

This aligns with the CA3 requirements for structured testing, bug triage, and evidence-based iteration.

