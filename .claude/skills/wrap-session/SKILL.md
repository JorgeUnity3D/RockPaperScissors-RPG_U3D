---
name: wrap-session
description: Wraps up the current work session by saving a task summary to ClaudeDocs/tasks/ and updating gotchas.md if needed. Use when the user types /wrap-session or says they are done working, ending a session, or about to /clear.
argument-hint: task-name
---

Wrap up the current work session for task: $ARGUMENTS

1. If `ClaudeDocs/tasks/$ARGUMENTS.md` does not exist, create it with:
   - Date (today)
   - What was accomplished this session
   - Files created or modified (with paths)
   - Bugs fixed (reference gotchas.md numbers if applicable)
   - Pending work / next steps

2. If `ClaudeDocs/tasks/$ARGUMENTS.md` already exists, append a new dated entry with the same structure — do not overwrite previous entries.

3. If any new gotchas or unexpected behaviors were discovered during this session, append them to `ClaudeDocs/agents/gotchas.md` with a new numbered entry.

Be concise. Write for an AI reader, not a human.
