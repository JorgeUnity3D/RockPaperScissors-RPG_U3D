---
name: fix
description: Analyzes a bug and proposes a fix with full explanation before touching any code. Use when the user asks to fix a bug by name or gotcha number.
argument-hint: "[bug name or gotcha number, e.g. '8' or 'rng-reseed']"
---

Fix target: $ARGUMENTS

## Step 1 — Identify the bug
Read `ClaudeDocs/agents/gotchas.md`. Find the entry matching $ARGUMENTS (by number or by keyword match in the title).

If no match found, say so and stop.

## Step 2 — Read the relevant code
Read the files mentioned in the gotcha entry. Understand the current implementation.

## Step 3 — Explain before touching anything
Present to the user:
- **What the bug is**: one paragraph, plain language
- **Why it happens**: the exact code path causing it
- **Proposed fix**: what you will change and why, with a short code snippet if helpful
- **Risk**: anything that could break as a side effect

Then ask: "Shall I proceed, or do you want to adjust the approach?"

## Step 4 — Only after confirmation
Implement the fix. Then:
- Update `ClaudeDocs/agents/gotchas.md` — mark the entry as fixed with today's date
- Note the fix in the current session (for `/wrap-session` later)
