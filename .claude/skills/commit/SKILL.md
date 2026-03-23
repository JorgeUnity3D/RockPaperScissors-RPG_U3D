---
name: commit
description: Generates a commit message in the project's format based on recent changes. Use when the user is ready to commit.
---

Generate a commit message for the current changes.

## Step 1 — Read recent changes
Run `git diff --staged` to see staged changes. If nothing is staged, run `git diff` instead and note that nothing is staged yet.

## Step 2 — Generate the commit message
Use this exact format — no deviations:

```
+ filename1: brief description of change
+ filename2: brief description of change
```

Rules:
- One line per file changed
- Filename only, no path (e.g. `ScissorBonfireManager.cs`, not the full path)
- Description is lowercase, concise, past tense
- No header line, no body, no footer — just the `+` lines
- If a file was deleted use `-` instead of `+`

## Step 3 — Output
Show the commit message ready to copy. Do not run `git commit` automatically.
