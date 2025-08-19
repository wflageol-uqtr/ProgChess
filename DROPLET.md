# Droplet Digtal Ocean

## Documentation

[How to Deploy ASP .NET Core Application to Digital Ocean Droplets](https://juldhais.net/how-to-deploy-asp-net-core-application-to-digital-ocean-droplets-40861be83db7)

[Host ASP.NET Core on Linux with Nginx](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-9.0&tabs=linux-ubuntu)

[How To Deploy a React Application with Nginx on Ubuntu](https://www.digitalocean.com/community/tutorials/deploy-react-application-with-nginx-on-ubuntu).

[How To Configure Nginx as a Reverse Proxy on Ubuntu 22.04](https://www.digitalocean.com/community/tutorials/how-to-configure-nginx-as-a-reverse-proxy-on-ubuntu-22-04).

[Install Docker Engine on Ubuntu](https://docs.docker.com/engine/install/ubuntu/#install-using-the-repository)

## Prérequis

Installer Nginx sur le droplet

```
sudo apt update
sudo apt install nginx
```

Permettre HTTP sur le firewall et regarder que Nginx est actif

```
systemctl status nginx
sudo ufw allow 'Nginx HTTP'
```

Installer Node sur le droplet

```
sudo apt install nodejs npm
```

Installer .net

```
sudo apt-get install -y dotnet-sdk-9.0
sudo apt-get install -y aspnetcore-runtime-9.0
dotnet tool install --global dotnet-ef --version 9.*
```

Installer Postgresql

```
sudo apt install postgresql postgresql-contrib
```

Configurez le repo apt de Docker.

```
sudo apt-get update
sudo apt-get install ca-certificates curl
sudo install -m 0755 -d /etc/apt/keyrings
sudo curl -fsSL https://download.docker.com/linux/ubuntu/gpg -o /etc/apt/keyrings/docker.asc
sudo chmod a+r /etc/apt/keyrings/docker.asc

# Add the repository to Apt sources:
echo \
  "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.asc] https://download.docker.com/linux/ubuntu \
  $(. /etc/os-release && echo "${UBUNTU_CODENAME:-$VERSION_CODENAME}") stable" | \
  sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
sudo apt-get update
```

Installer Docker

```
 sudo apt-get install docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin

 systemctl status docker
```

## Client React

Premièrement, j'ai créé un dossier où serait mon client

```
mkdir /var/www/161.35.187.194
```

Ensuite, il faut configurer le service Nginx pour qu’il pointe vers le bon répertoire

```
sudo vi /etc/nginx/sites-available/161.35.187.194
```

Configuration

```
server {
        listen 80;
        listen [::]:80;

        root /var/www/161.35.187.194;
        index index.html;

        location / {
                try_files $uri /index.html;
        }
}
```

Copier la configuration dans site-enabled

```
sudo ln -s /etc/nginx/sites-available/161.35.187.194 /etc/nginx/sites-enabled/
```

Supprimer le fichier de configuration par défaut de Nginx, voir si tout fonctionne

```
sudo unlink /etc/nginx/sites-available/default
sudo nginx -t
```

**Important** : si l’on veut continuer les tests maintenant, sinon revenir après avoir fait la configuration de l’API.

**Retour sur votre machine locale** : il faut maintenant mettre le build de notre projet React sur le droplet. Avant de créer le build, veillez à mettre à jour les informations dans le fichier .env.

```
VITE_APP_BACKEND_URL="http://161.35.187.194/backend/"
VITE_APP_FRONTEND_URL="http://161.35.187.194"
```

Créer un build de notre projet. Une fois terminé, un dossier dist est créé

```
npm run build
```

Via SSH, copié le contenue de dist sur notre droplet

```
scp -r dist/* root@161.35.187.194:/var/www/161.35.187.194/
```

Redémarrer Nginx

```
sudo systemctl restart nginx
```

Ça devrait marcher quand on va sur http://161.35.187.194/admin/login

## Dotnet API

Cloner le projet dans _home_

```
cd /home
git clone https://github.com/wflageol-uqtr/ProgChess.git
```

Créer un databse local sur la machine, pour ensuite mettre les informations dans _appsettings.json_

```
cd ProgChess/ProgChess/ProgChess.Server/
vi appsettings.json
```

Mettre les informations de la base de données dans **DefaultConnection**

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost; Database=progchess; Username=postgres; Password=Prog2025Chess;"
  },
  "JwtSettings": {
    "Secret": "your-256-bit-base64-secret-goes-here",
    "Issuer": "MyAwesomeIssuer",
    "Audience": "MyAwesomeAudience"
  },
  "Sendgrid":  {
    "ApiKey": "API_KEY_SENDGRID"
  }
}
```

Rouler les migrations

```
dotnet ef database update
```

Partir l'API

```
dotnet publish --configuration Release
cd bin/Release/{TARGET FRAMEWORK MONIKER}/publish
dotnet ProgChess.Server.dll
```

Ensuite, il faut configurer le service Nginx pour un reverse proxy

```
sudo vi /etc/nginx/sites-available/161.35.187.194
```

Configuration

```
server {
        listen 80;
        listen [::]:80;

        root /var/www/161.35.187.194;
        index index.html;

        location / {
                try_files $uri /index.html;
        }
        // Ajouter ceci
        location /backend/ {
                proxy_pass http://localhost:5000/;
                include proxy_params;

        }

}
```

## Sandbox sur Docker

**\*Docker doit être installé sur la machine, voir en haut si non**.

Installer les packages

```
cd /home/ProgChess/ProgChess/progchess.docker.sandbox/
npm install
```

Build le container

```
docker build -t sandbox .
docker run -d -p 3000:3000 --name progchess-container sandbox
```

Ensuite, il faut configurer le service Nginx pour un reverse proxy

```
sudo vi /etc/nginx/sites-available/161.35.187.194
```

Configuration

```
server {
        listen 80;
        listen [::]:80;

        root /var/www/161.35.187.194;
        index index.html;

        location / {
                try_files $uri /index.html;
        }
        location /backend/ {
                proxy_pass http://localhost:5000/;
                include proxy_params;

        }
        // Ajouter ceci
        location /sandbox/ {
                proxy_pass http://localhost:3000/;
                include proxy_params;

        }
}
```

Copier et redémarrer Nginx

```
sudo ln -s /etc/nginx/sites-enabled/161.35.187.194 /etc/nginx/sites-available/
sudo nginx -t
sudo systemctl restart nginx
```
