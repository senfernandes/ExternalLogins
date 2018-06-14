# External Logins in .NET Core App

## Create app on Facebook's Developer API
- Create New App in My Apps menu
- Go to basic app settings and add a new platform. Choose "Website";
- In the next step, inform the website's URL (it can be found on launchSettings.json file);
- Save your changes.

## Configure Facebook User Secrets & Facebook Autho2 Credentials
- Add Facebook User Secrets (App Id and Secret Key) to .env file;
- Install Microsoft.AspNetCore.Authentication.Facebook Nuget package in your project;
- On ```Startup``` file:
    -  Add ```services.AddAuthentication().AddFacebook();``` in the ```ConfigureServices``` method;
    - Add ```app.UseAuthentication();``` n the ```Configure``` method;
