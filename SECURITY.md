Security guidance — secrets and tokens

This repository previously contained a hard-coded GitHub Personal Access Token (PAT) in `appsettings.json`. That token has been removed (replaced with an empty placeholder). Follow the steps below immediately and then complete the recommended follow-ups.

1) Revoke the exposed token on GitHub (do this now)
- Go to GitHub -> Settings -> Developer settings -> Personal access tokens -> Tokens (classic) or Fine-grained tokens.
- Find the token that was exposed and revoke/delete it immediately.

2) Create a new PAT with least privilege
- Create a new token with only the scopes the app needs (e.g., repo read-only). Prefer fine-grained tokens when possible.

3) Store the new token safely
- Development (local): use `dotnet user-secrets` or environment variables.
  - dotnet user-secrets (per-project, development):
    dotnet user-secrets init
    dotnet user-secrets set "GitHub:Token" "<new-token>"
  - Environment variable (PowerShell temporary):
    $env:GITHUB_TOKEN = "<new-token>"
  - Environment variable (PowerShell persistent for current user):
    setx GITHUB_TOKEN "<new-token>"
- Production/CI: use platform secret stores (GitHub Actions secrets, Azure Key Vault, AWS Secrets Manager, etc.).

4) Reading the secret in .NET
- Keep `appsettings.json` free of secrets. Configure the app to read from environment variables or user-secrets in `Program.cs` (ASP.NET Core loads env vars and user-secrets when configured).
- Example configuration key: `GitHub:Token` — bind from config providers.

5) Remove secret from git history (one of the following)
- IMPORTANT: Rewriting history requires force-push and coordination with team members.

Option A — BFG Repo-Cleaner (easy, recommended):
- Download BFG (a single jar) and run:
  java -jar bfg.jar --replace-text replacements.txt repo.git
  (see BFG docs; create `replacements.txt` listing the literal token)
- After BFG run:
  cd repo
  git reflog expire --expire=now --all
  git gc --prune=now --aggressive
  git push --force --all
  git push --force --tags

Option B — git-filter-repo (recommended if available):
- Install `git-filter-repo` (platform-specific). Then run a replacement script per its docs to remove the secret string.

Option C — git filter-branch (last resort):
- This is slow and more error-prone. Example to remove a file (if you want to remove the whole file):
  git filter-branch --force --index-filter "git rm --cached --ignore-unmatch path/to/file" --prune-empty --tag-name-filter cat -- --all
- Then run the same `reflog/gc` and force-push steps above.

6) After history rewrite
- Notify collaborators to re-clone or run steps to reset local clones (e.g., fetch + reset or reclone).

7) Additional recommendations
- Add `appsettings.Development.json` to `.gitignore` if it contains local secrets (and stop committing it). Updating `.gitignore` does not remove existing commits — you must rewrite history for that.
- Use least privilege for tokens and rotate them regularly.
- Treat tokens like passwords; never paste into issues, PRs, or chat.

If you want, I can:
- Revoke the token for you (I can only provide instructions; cannot act on GitHub).
- Create a PR that removes the token (already done) and adds `SECURITY.md` (done).
- Add .gitignore entries (I can add them if you want).
- Provide step-by-step commands for BFG or git-filter-repo tailored to your repo. Please tell me whether you want the more automated BFG approach or prefer the `git-filter-repo` route.
