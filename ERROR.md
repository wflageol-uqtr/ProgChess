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

## Version Node 23

Le parser, qui s’occupe de traiter la sortie renvoyée par notre sandbox, fonctionne principalement avec les versions récentes de Node.js. En effet, il analyse les caractères de sortie ; ainsi, avec une version plus ancienne de Node (par exemple, la commande OK mon_test ne retournerait rien), alors qu’avec Node 23, cela renvoie entre ✅ mon_test.

## Erreur d'overflow sur Sortie des tests

Quand on réduit beaucoup la section Sortie de tests, c'est comme si les Tabs dépasse. Si on mets un overflow-x-auto, notre badge est maintenant coupé.

## Responsive sur Ipad

Le système semble plutôt bien adapté pour les téléphones, mais ça ne semble pas le cas avec les tablettes. Quand on regarde dans le simulateur du browser, le layout est bizarre.

## Impossible de soummettre du code qui compile pas

J'ignore si c'est un bug, mais on ne peut pas soummettre du code qui ne compile pas, le sandbox retourne un erreur afficher dans le UI.

## Problème avec StudentExercise

Il peut arriver le cas suivant dans l'application, en supprimer le résultat d'un étudiant alors que celui-ci à le flag isComplete à true. Il ne pourrait pas refaire l'exercice. Il faut soit aller set le flag à false manuellement dans la base de données. Sinon, supprimer et remmettre l'étudiant.

## Uniformiser les messages

Quand je regarde ça, je n'ai pas trop uniformisé les messages du système. Desfois c'est le backend qui me retourne un message, desois c'est mon frontend.
