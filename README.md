# Introduction 
Model.BO is a web application framework to visualize and apply modifications to a database. Made at the start for a company with custom features, it has been built to be integrated as an internal solution for business purposes.
I had the opportunity to adapt it and create a model that can be used in a variety of projects involving a db. 

![MainScreen](mainScreen.png)

## Features 

### Access Rights using Windows Authentication

Login is made through Windows Authentication, and therefore doesn't need accounts creation. It can be configured easily with groups already existing in the directory. Each group can be configured to have specific access and edit rights for each page (reader, editor, admin), using the CRUDrights.json file:

```json
{
  "groups": {
    "adminGroup": {
      "Accounts": "admin",
      "Users": "admin",
      "Messages": "admin",
      "Orders": "admin",
      "Products": "admin",
      "Prices": "admin"
    },
    "editorGroup": {
      "Accounts": "editor",
      "Users": "editor",
      "Messages": "editor",
      ...
```
Authorization checks can be easily implemented in Razor views to prevent certain elements from rendering before being sent to the client:
```csharp
<h1> Title </h1>
if ((await AuthorizationService.AuthorizeAsync(User, "RequireAdminAccounts")).Succeeded)
{
    <button> Delete all rows </button>
}
<span> Hi </span>
...
```

### Advanced Filtering

Tables can be ordered and filtered effortlessly, with differents filters depending on the data type. Custom options can also be added in the data.json file: 

<img src="customFiltering.png" width="250" />

### Easily Configurable

Tables can be configured in the pagesLayout.json file to display and modify columns. Header and row actions are also customizable:

```json
  "Messages": {
    "Colonnes Tables": {
      "Number of retries": {
        "DBColumn": "RetryCount",
        "Type": "Number"
      },
      "ErrorMessage": {
        "DBColumn": "ErrorMessage"
      }
      "ProcessedOn": {
        "DBColumn": "ProcessedOn",
        "Type": "DateTime"
      },
      "CorrelationId": {
        "DBColumn": "CorrelationId"
      }
    },
    "Colonne Actions": {
      "Buttons": {
        "User details": {
          "StatusRequired": "ReaderUsers",
          "Icon": "bi-eye",
          "Link": {
            "Href": "~/UserData/UserDetail",
            "Parameters": {
              "Id": "Id"
            }
          }
        }
      }
    },
    "VisibleColumnsOnMobile": [ "ErrorMessage", "Priority", "PayloadSize" ],
    "EditableColumns": [ "ProcessedOn", "CorrelationId", "Priority" ],
    "DeletableRows": true,
    "Default message": "No message found."
  },
```



# Getting Started

## Run the demo
1. Clone the repo
```bash
git clone https://github.com/BaptoastGit/db-visualizer-model.git
```
2. Open Visual Studio
3. Press the Play button or run this command in your terminal:
```bash
dotnet run
```

(The SQLite database will be automatically created and seeded with demo data on the first launch).



