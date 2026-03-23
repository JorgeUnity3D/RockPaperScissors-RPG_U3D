---
name: new-feature
description: Analyzes a new feature request against the roadmap and architecture, proposes a design, and creates an investigation doc before touching any code. Use when the user wants to plan or start a new feature.
argument-hint: "[feature name or description]"
---

New feature request: $ARGUMENTS

## Step 1 — Read context
Read:
- `ClaudeDocs/agents/architecture.md`
- `ClaudeDocs/agents/roadmap.md`
- `ClaudeDocs/agents/dependencies.md`
- `ClaudeDocs/agents/naming-conventions.md`

## Step 2 — Analyze
Determine:
- Where does this feature fit in the roadmap? Is it already planned?
- Which existing systems does it touch?
- Are there gotchas in `ClaudeDocs/agents/gotchas.md` relevant to this feature?
- What new classes/files would be needed, following naming conventions?

## Step 3 — Propose design
Present to the user:
- **Summary**: what the feature does in one paragraph
- **Systems affected**: which managers, services, UI controllers, data classes
- **Proposed approach**: how to implement it, following existing patterns
- **New files needed**: names and locations
- **Risks or unknowns**: anything that needs investigation first

Ask: "Does this approach look right, or do you want to adjust before I write the investigation doc?"

## Step 4 — After confirmation
Create `ClaudeDocs/investigations/YYYY-MM-DD_$ARGUMENTS.md` with the full design proposal.
Do not write any code until explicitly asked.
