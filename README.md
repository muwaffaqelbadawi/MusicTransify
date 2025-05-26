# Spotify & YouTube Music Integration API  
**A secure, resilient .NET web API for integrating with Spotify and YouTube Music, featuring robust OAuth2 authentication, dynamic configuration, and modern CI/CD.**

[![.NET Core CI](https://github.com/YOUR_USERNAME/YOUR_REPO/actions/workflows/ci.yml/badge.svg)](https://github.com/YOUR_USERNAME/YOUR_REPO/actions)
![License](https://img.shields.io/github/license/YOUR_USERNAME/YOUR_REPO)
![Built with .NET](https://img.shields.io/badge/.NET-8.0-blue)

---

## 🚀 Overview

This project is a robust, production-ready .NET web API that seamlessly connects with both **Spotify** and **YouTube Music**. It leverages modern OAuth2 flows, secure secret management, and advanced .NET patterns (like Polly for resilience) to help you build music-powered apps with confidence.

---

## 📚 Table of Contents

- [Features](#features)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Usage](#usage)
- [Resilience & Security](#resilience--security)
- [CI/CD & Secret Scanning](#cicd--secret-scanning)
- [Contributing](#contributing)
- [License](#license)
- [Credits](#credits)

---

## ✨ Features

- **Spotify & YouTube Music OAuth2 Authentication**  
  Securely authenticate and exchange tokens without exposing secrets.
- **Dynamic Authorization Headers**  
  Handles token injection dynamically for maximum security.
- **Polly-based Resilience**  
  Automatic retries and transient fault handling for HTTP requests.
- **Centralized Configuration**  
  Static headers in config; sensitive tokens managed in-memory.
- **GitHub Actions CI/CD**  
  Automated testing, build, and security scanning (TruffleHog).
- **Flexible Secret Management**  
  Supports environment variables, user secrets, and best practices.
- **Extensible, Testable Architecture**  
  Built with DI, interfaces, and clear separation of concerns.

---

## ⚡ Getting Started

### **1. Clone the Repository**

```bash
git clone https://github.com/YOUR_USERNAME/YOUR_REPO.git
cd YOUR_REPO
```

### **2. Set Up Configuration**

- Copy the sample configuration and enter your secrets:
    ```bash
    cp appsettings.Development.json.example appsettings.Development.json
    ```
- Set up sensitive secrets via [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) or environment variables for local development.

### **3. Restore and Build**

```bash
dotnet restore
dotnet build
```

### **4. Run the App**

```bash
dotnet run
```

---

## ⚙️ Configuration

Sensitive keys (like Spotify `client_secret`) **should never be stored in code**.  
- Use `appsettings.json`, but override secrets with [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) or environment variables in production.
- Example snippet:
    ```json
    {
      "SpotifyOptions": {
        "ClientId": "YOUR_CLIENT_ID",
        "ClientSecret": "<from secret store>",
        ...
      }
    }
    ```

**Static headers** such as `Content-Type` can be configured in `appsettings.json` or `secrets.json` for clarity and maintainability.

---

## 🛠 Usage

- **Spotify Login:**  
  Visit `/api/spotify/login` to begin the OAuth2 login flow.
- **Token Exchange:**  
  Tokens are exchanged securely; see `SpotifyAuthService` for implementation.
- **YouTube Music Flow:**  
  Similar endpoints and flows are supported.

Refer to the `src/Services` directory for concrete service implementations.

---

## 💪 Resilience & Security

- **Polly** is used for robust HTTP retry logic:
    - Transient errors are automatically retried, with exponential backoff.
- **Secret Scanning**:
    - CI/CD runs [TruffleHog](https://github.com/trufflesecurity/trufflehog) to prevent secret leaks.
- **.gitattributes** is used to ensure consistent line endings across platforms.

---

## 🚦 CI/CD & Secret Scanning

- **GitHub Actions** runs build, test, and secret scanning on every PR and push to `main`.
- **TruffleHog** scans for accidental secret exposure.
- See [ci.yml](.github/workflows/ci.yml) for details.

---

## 🤝 Contributing

Contributions are welcome!  
- Please open an [issue](https://github.com/YOUR_USERNAME/YOUR_REPO/issues) or submit a pull request.
- Run `dotnet test` before submitting to ensure all tests pass.
- Read the [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

---

## 📄 License

Distributed under the MIT License. See [LICENSE](LICENSE) for details.

---

## 🙏 Credits

- [Polly](https://github.com/App-vNext/Polly) for resilience patterns
- [TruffleHog](https://github.com/trufflesecurity/trufflehog) for secret scanning
- [Spotify Web API](https://developer.spotify.com/documentation/web-api/)
- [YouTube Data API](https://developers.google.com/youtube/v3)

---

> Made with ❤️ using .NET 8.0
