# User login/register application
 
This is a test project to demonstrate using Intern with Angular, ngrx and api calling.
 
## Get started
 
### Clone the repo
 
```shell
git clone https://github.com/dharmin-wa/Task1.git
cd Task1
cd user-GDPR-FE
cd user-GDPR-BE
```
 
### Install npm packages
 
Install the `npm` packages described in the `package.json` and verify that it works:
 
```shell
npm install
npm start
```
 
The `npm start` command builds (compiles TypeScript and copies assets) the application into `dist/`, watches for changes to the source files
 
Shut it down manually with `Ctrl-C`.
 
#### npm scripts
 
These are the most useful commands defined in `package.json`:
 
* `npm start` - runs the TypeScript compiler, asset copier, and a server at the same time, all three in "watch mode".
* `npm run build` - runs the TypeScript compiler and asset copier once.
 
 
### Database settings
Update the MongoDB connection settings in the appsettings.json file:
{
  "MongoDB": {
    "ConnectionURI": "your-mongodb-connection-uri",
    "DatabaseName": "user_GDPR",
    "CollectionName": "Users"
  }
}
 
Run the application using Visual Studio or the terminal
`dotnet run`
The application will be accessible with swagger at  https://localhost:{port}/swagger/index.html