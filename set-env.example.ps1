# Copy to set-env.ps1 (git-ignored), fill in your own values, then run it before starting CaYaChat.
# Kendi değerlerinizle set-env.ps1 olarak kopyalayın (git'e eklenmez).
[Environment]::SetEnvironmentVariable("CAYACHAT_MONGO", "mongodb+srv://<user>:<password>@<cluster>/<db>", "User")
[Environment]::SetEnvironmentVariable("CAYACHAT_MONGO_DB", "CaYaChatSystem", "User")