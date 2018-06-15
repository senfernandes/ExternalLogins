# External Logins in .NET Core App (beta version)
App with external logins using .NET Core.
At this point (June 16th, 2018), only Facebook Login is being implemented.

## Create app on Facebook's Developer API (https://developers.facebook.com)
- Create New App in My Apps menu
- Go to basic app settings and add a new platform. Choose "Website";
- In the next step, inform the website's URL (it can be found on launchSettings.json file);
- Save your changes.

## Configure Facebook User Secrets & Facebook Autho2 Credentials
- Add Facebook User Secrets (App Id and Secret Key) to .env file;
- Install Microsoft.AspNetCore.Authentication.Facebook Nuget package in your project;
- On ```Startup``` file:
    -  Add ```services.AddAuthentication().AddFacebook();``` in the ```ConfigureServices``` method;
    - Add ```app.UseAuthentication();``` in the ```Configure``` method;

## Add Identity (check it out the Project to see configurations)
- Add corresponding packages
- Change ```Settings``` file
- Create tables in database

## Create Facebook login interface
- Check views and controllers
