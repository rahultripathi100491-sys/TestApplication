# TestApplication - Clean Architecture + CQRS + MediatR + MailKit

.NET 8 Web API with:

- Clean Architecture style separation
- CQRS
- MediatR
- MailKit
- Gmail SMTP
- GitHub Actions
- Somee FTP deployment

## Project structure

TestApplication/
├── TestApplication.API/
│   ├── Controllers/
│   │   └── EmailController.cs
│   ├── Program.cs
│   ├── appsettings.json
│   └── TestApplication.API.csproj
├── TestApplication.Application/
│   ├── Email/
│   │   ├── Commands/
│   │   ├── Handlers/
│   │   ├── Models/
│   │   └── Services/
│   └── TestApplication.Application.csproj
├── TestApplication.Repository/
│   ├── DependencyInjection/
│   ├── Services/
│   └── TestApplication.Repository.csproj
├── .github/workflows/deploy.yml
└── TestApplication.sln

## Gmail setup

For Gmail SMTP, use:

Host: smtp.gmail.com
Port: 587
Security: STARTTLS
Username: your Gmail address
Password: Gmail App Password

Do not use your normal Gmail password.

Set the values locally using User Secrets or environment variables rather than committing real credentials.

Example User Secrets:

dotnet user-secrets init --project TestApplication.API

dotnet user-secrets set "EmailSettings:Username" "yourgmail@gmail.com" --project TestApplication.API
dotnet user-secrets set "EmailSettings:Password" "your-16-character-app-password" --project TestApplication.API
dotnet user-secrets set "EmailSettings:From" "yourgmail@gmail.com" --project TestApplication.API

## Test request

POST /api/Email/send

Content-Type: application/json

{
  "to": "recipient@example.com",
  "subject": "Test mail",
  "body": "<div style=\"font-family:Arial,sans-serif;padding:20px\"><h2>Test Email</h2><p>Hello,</p><p>This email was sent from my .NET 8 Web API using CQRS, MediatR and MailKit.</p><p><strong>Status:</strong> SUCCESS</p><p>Regards,<br/>Rudray Technology</p></div>"
}

## Important SMTP behavior

A successful smtp.SendAsync call means the SMTP server accepted the message. It does NOT guarantee that the recipient's mailbox ultimately received it.

For example, if the recipient does not exist, Gmail may accept the message initially and later send a bounce/failure notification. Detecting those later bounces requires processing the bounce notification or using a transactional email provider with delivery/bounce webhooks.

## GitHub / Somee deployment

Add these repository secrets:

FTP_SERVER
FTP_USERNAME
FTP_PASSWORD
FTP_PATH

For the FTP URL:

ftp://testapplication.somee.com/www.testapplication.somee.com

the values are normally:

FTP_SERVER = testapplication.somee.com
FTP_PATH = /www.testapplication.somee.com/

Use the exact FTP path shown by Somee.

The workflow deploys only when code is pushed/merged to main, or when manually started with Run workflow.
