# Erreur connue dans le système

## Sendgrid API

Pour avoir la feature mot de passe oublié, il faut mettre une clé d'API dans appSetting.json. Aussi, une fois en prod, mettre un email et envoyé le email au utilisateur.

_appsettings.json_

```
  "Sendgrid":  {
    "ApiKey": "YOUR_API_KEY"
  }
```

_EmailService.cs_

```
var sendGridMessage = MailHelper.CreateSingleEmail(
new EmailAddress("sendgrid@email.com"), new EmailAddress("email@toSend.com"), subject, message, BuildResetPasswordEmailHtml(message));
```
