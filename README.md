HOW TO RUN AND OPEN THIS PROJECT

1)To run this project you will have to download the zip file from the git hub repository .Once the project has been downloaded ,place the project in your repo along with the other projects and unzip the project.After going into visual studio and tap on open project solution and select the c# project file within the project and from there you will be able to perfrom the next step.
2) The next step is to add your connection string ,go to the view tab and open up your sql object explorer and right click on your prefered to get the connection string.After copying the connectionsstring paste it in your appsettings.json.This is what connects the project to your local server
IF YOU DO NOT HAVE A DB CREATED :Right click on the server ,select new query and right this query : CREATE DATABASE databasename; to create your database.
3) Migrations
For the project to work you also need to remove and add a new migration ,to do so run these commands in your package manager console which will be found in your tools tab:
Update-Database InitialCrete //To  revert to initial migration
Remove Migration //Remove the current main migration
Update Database //Update the database after removal of migration
Get-Migration //checks if it has been removed
add-migration AddClaims //to add a new migration
Update-Database //ensures that the migration is applied to the database
Get-Migration //checks if the newly added migration has been applied
After this go to the build tab and "rebuild solution" ,after the solution has been rebuilt successfully you can now press the play button and run the project.
