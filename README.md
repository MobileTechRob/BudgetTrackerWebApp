This project represent the REST API that the Front End queries from.  

You'll see MyPersonalBudgetAPI as the main solution with two projects
   1) BudgetAPI with is the REST Controller
      a.  HomeBudgetController.cs is the REST interface
   3) DatabaseManager project has the following classes
      a. AppDbContext defines both the
           1. connection to Sql Server
           2. what the data models that represent the database tables look like to the EF Framework
      b. CRUD_Operations uses the AppDBContext and allow an update and fetch against the database.

DailyCostTracker.sln is the file that contains all projects ..
  1) Importer that understands the downloaded bank statement and communicates to the BudgetAPI over REST. 
  2) The BudgetAPI  is the API that wraps around the database.
  3) The DatabaseManager project that manages the database aspect of the project.

There needs to be a service layer yet that separates the API from the database.





      


