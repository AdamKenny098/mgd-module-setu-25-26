# Ironhollow – Bug List (CA3)

| ID   | Severity | Description | Reproduction Steps | Expected Behaviour | Actual Behaviour | Status | Notes |
|------|----------|-------------|--------------------|--------------------|------------------|--------|-------|
| B001 | High     | Player dies instantly on spawn when enemies shot instantly | 1. Play → Start Game | Player spawns safely with room to move | Player instantly dies | Fixed | Solved after adding a safe first segment |
| B002 | Medium   | Polarity flip provides no boost unless player touches terrain | 1. Move into open space 2. Flip polarity | Flip gives strong vertical impulse anywhere | No movement unless touching walls | Fixed | amplified flip force |
| B003 | High     | TunnelSpawner runs while in editor preview | 1. Open scene in editor | Spawner idle until StartGame is pressed | Segments spawn immediately | Fixed | Added Application.isPlaying guards |
| B004 | High     | UI scales incorrectly after scene reloads | 1. Play → Pause → Restart | UI remains stable and aligned | UI becomes zoomed/warped | Fixed | Removed DontDestroyOnLoad on UI canvas |
| B005 | Medium   | Camera jumps to 20,000+ X coordinate | 1. Start run | Camera stays around origin | Camera drifts extremely far | Fixed | Architecture changed to world-scroll |
| B006 | Medium   | App resume results in half-paused state | 1. Minimize app 2. Resume | Game resumes cleanly in paused state | Game is visually paused but time is running | Fixed | Implemented proper pause/resume handler |
| B007 | Low      | First tunnel segment does not spawn reliably | 1. Start run multiple times | First tile always correct | Random tile sometimes appears | Fixed | Added dedicated SpawnFirstSegment logic |

