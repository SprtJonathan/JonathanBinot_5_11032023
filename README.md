# ExpressVoitures
# Projet 5 : Créez votre première application avec ASP .NET Core

## Authentification Admin à l'application :
Id utilisateur : Admin

Mot de passe : Password123

## Prérequis :
MSSQL Developer 2019 ou Express 2019 doit être installé avec Microsoft SQL Server Management Studio (SSMS).

MSSQL : https://www.microsoft.com/fr-fr/sql-server/sql-server-downloads

SSMS : https://docs.microsoft.com/fr-fr/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16

*Remarque : les versions antérieures de MSSQL Server devraient fonctionner sans problèmes, mais elles n’ont pas été testées.

*Dans le projet ExpressVoitures, ouvrez le fichier appsettings.json.*

Vous avez la section ConnectionStrings qui définit les chaînes de connexion pour les 2 bases de données utilisées dans cette application.

      "ConnectionStrings":
      {
        "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=ExpressVoitures;Trusted_Connection=True;MultipleActiveResultSets=true; TrustServerCertificate=True",
        "ApplicationDbContextConnection": "Server=(localdb)\\mssqllocaldb;Database=ExpressVoitures;Trusted_Connection=True;MultipleActiveResultSets=true"
      }

**DefaultConnection** - chaîne de connexion à la base de données de l’application. (Assurez-vous que le nom de l'instance est correct sur votre poste. Le repository utilise SQLEXPRESS04 car il s'agit de l'instance 04 sur mon poste. Il faudra probablement mettre SQLEXPRESS dans votre cas)

**ApplicationDbContextConnection** - L'application utilise Identity pour la gestion de comptes. Ce package utilise une base de données différente de celle du site web.

Il existe des versions différentes de MSSQL (veuillez utiliser MSSQL pour ce projet et non une autre base de données). Lorsque vous configurez le serveur de base de données, diverses options modifient la configuration de sorte que les chaînes de connexion définies peuvent ne pas fonctionner.

Les chaînes de connexion définies dans le projet sont configurées pour MSSQL Server Standard 2019. L’installation n’a pas créé de nom d’instance, le serveur est donc simplement désigné par « . », qui désigne l’instance par défaut de MSSQL Server fonctionnant sur la machine actuelle. Pendant l’installation, c’est l’utilisateur intégré de Windows qui est configuré dans le serveur MSSQL par défaut.

Si vous avez installé MSSQL Express, la valeur à utiliser pour Server est très probablement .\SQLEXPRESS. Donc votre chaîne de connexion ExpressVoitures serait :

    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=ExpressVoitures;Trusted_Connection=True;MultipleActiveResultSets=true; TrustServerCertificate=True",
  
Si vous avez des difficultés à vous connecter, essayez d’abord de vous connecter à l’aide de Microsoft SQL Server Management Studio (assurez-vous que le type d’authentification est « Authentification Windows »), ou consultez le site https://sqlserver-help.com/2011/06/19/help-whats-my-sql-server-name/.

## Compte utilisateur :
Du fait du besoin du concessionnaire à être le seul à pouvoir se connecter au site web, la fonction d'inscription est désactivée. Cependant il faut un compte pour avoir accès aux fonctionnalités d'administration du site. La version hébergée sur Azure dispose d'un compte administrateur actif cependant pour la version locale, il faudra ajouter manuellement un compte administrateur avec SSMS par exemple : 
| Id | UserName | NormalizedUserName | Email | NormalizedEmail | EmailConfirmed | PasswordHash | SecurityStamp | ConcurrencyStamp | PhoneNumber | PhoneNumberConfirmed | TwoFactorEnabled | LockoutEnd | LockoutEnabled | AccessFailedCount |
| ------------- | ------------- | ------------- | ------------- | ------------- | ------------- | ------------- | ------------- | ------------- | ------------- | ------------- | ------------- | ------------- | ------------- | ------------- |
| 7c34f156-dd08-4a0a-be5f-abf83d7f3970 | admin@expressvoitures.com | ADMIN@EXPRESSVOITURES.COM | admin@expressvoitures.com | ADMIN@EXPRESSVOITURES.COM | 1 | AQAAAAIAAYagAAAAEO7JuglrhvZ7yyd1RJJcGlXPi3+4F+vK7PO925V9cdKFA1lJ6QLVuAINOJRZD6rHng== | FDYPPWPCAP45MAMTBOQLDZNULID3N33M | 961785ff-4241-4cef-bbfa-ac6066b3e733 | NULL | 0 | 0 | NULL | 1 | 0 |

# Le site est également hébergé sur Azure : 
https://expressvoitures-jonathanbinot.azurewebsites.net/
Le compte administrateur est accéssible avec les identifiants suivants : 
login : admin@expressvoitures.com
mot de passe : P@ssword123
