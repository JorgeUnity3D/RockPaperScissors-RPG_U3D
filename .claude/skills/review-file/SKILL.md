---
name: review-file
description: Reviews a specific file for bugs, inconsistencies with architecture, naming violations, and potential gotchas. Use when the user asks to review or audit a file.
argument-hint: "[file path or class name]"
---

Review target: $ARGUMENTS

## Step 1 — Find the file
If $ARGUMENTS is a full path, read it directly.
If $ARGUMENTS is a class name, locate it using the file index in `ClaudeDocs/agents/file-index.md`, then read it.

## Step 2 — Read context
Read:
- `ClaudeDocs/agents/architecture.md`
- `ClaudeDocs/agents/naming-conventions.md`
- `ClaudeDocs/agents/gotchas.md`

## Step 3 — Review
Check for:
- **Existing gotchas**: does this file contain any of the known issues from gotchas.md?
- **Naming violations**: anything that deviates from naming-conventions.md
- **Architecture violations**: runtime Find(), missing null-conditional on AppEvents, etc.
- **Code smells**: inconsistencies, dead code, TODOs, fragile patterns
- **Missing pieces**: incomplete implementations flagged in roadmap

## Step 4 — Report
Output a structured report:

**Critical** — bugs or crashes waiting to happen
**Warnings** — deviations from architecture or conventions
**Minor** — style inconsistencies, dead code, TODOs
**Looks good** — notable things that are well implemented

If nothing to report in a category, omit it.
Do not suggest changes unprompted — report only. Ask before fixing anything.
