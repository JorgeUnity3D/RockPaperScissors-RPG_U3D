---
name: Code style — match existing conventions
description: Do not introduce naming or structural patterns that don't already exist in the codebase
type: feedback
---

Only use patterns that already exist in the file or class being edited. Do not introduce:
- `const` fields or `static readonly` unless the codebase already uses them in that area
- `UPPER_CASE_WITH_UNDERSCORES` naming unless the file already uses it
- Extra abstractions, helpers, or named constants for values that are simple and unlikely to be reused

**Why:** User explicitly corrected the addition of `public const int CURRENT_VERSION = 1` — it introduced a naming style (UPPER_CASE) and a pattern (const) that doesn't exist in `GameContext.cs` or nearby classes. Also, the value will change, so calling it a "constant" was semantically wrong.
**How to apply:** Before adding any named constant or helper, check if similar patterns exist in that file. If not, use an inline value.
