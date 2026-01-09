
# Exercise — Weekend Surcharge Refactor

## Current Behavior
Weekend surcharge (+10%) applies **only** to International shipments inside `RegionMultiplierPolicy`.

## New Requirement
Weekend surcharge should apply to **all shipments**, not just International.

## Tasks
1. Naively implement the new requirement (e.g., add weekend logic in another policy or duplicate checks).
2. Observe duplication risk and coupling.
3. Refactor:
   - Remove weekend logic from `RegionMultiplierPolicy`.
   - Introduce a unified weekend policy/decorator.
   - Keep code expressive and minimal.

## Run
```bash
dotnet build
dotnet test
```
Enable `RefactorTests` after refactor.
