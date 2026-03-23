---
name: daily
description: Morning briefing for the current project session. Shows last session summary, pending work, and recommends where to start. Use when the user starts a new session or says good morning / let's work / where were we.
---

Good morning. Read the following to prepare the session briefing:

- `ClaudeDocs/agents/roadmap.md`
- `ClaudeDocs/agents/gotchas.md`
- All files in `ClaudeDocs/tasks/` — focus on the most recent entry in each

Output a concise briefing:

---

## Last session
What was done in the most recent task file (one or two lines).

## Current phase
Which roadmap phase is active and how many items are pending vs done.

## Pending bugs
Unresolved gotchas that are blocking progress, with their numbers.

## Recommended next step
One concrete action to start with today, with the specific file or class to touch.

---

Be brief. This is a warm-up, not a report. No prose, no padding.
