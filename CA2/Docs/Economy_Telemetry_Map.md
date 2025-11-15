# Economy & Telemetry Event Map – *Ironhollow*

Although Ironhollow does not use a currency or upgrade system, it includes a built in distance statistic and event-driven **telemetry logging**.  
All telemetry is local, printed to the Unity Console using `Debug.Log()` calls for validation.

---

## Event Map

| **Event Name** | **Parameters** | **Triggered From** | **Description** |
|-----------------|----------------|--------------------|-----------------|
| `level_start` | `level` | `GameManager.Start()` | Logged at the start of the main gameplay scene. |
| `enemy_destroyed` | `type` | `Enemy_Kamikaze.Explode()` | Triggered when a Kamikaze enemy explodes or is destroyed. |
| `player_died` | — | `GameManager.PlayerDied()` | Fired when the player collides with a hazard or enemy. |
| `level_complete` | `level` | `GameManager.PlayerDied()` | Intended for tracking successful when a level is over. |
| `session_end` | — | `UIManager.QuitGame()` | Fired when the player quits the game. |

---

## Reward Loop Summary

| **Action** | **Reward** | **Effect** |
|-------------|-------------|------------|
| Traversing Horizontally | Distance tracker + | Distance increases as a way to show "progress" |


---

### Notes
- All events are recorded via `Debug.Log()` and visible in **Editor Console**  on device.  
- The reward system is purely illustrative,  but could be used for a leaderboard system in CA3.