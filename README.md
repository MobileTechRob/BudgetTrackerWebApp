This project represent the REST API that the Front End queries from.  

You'll see MyPersonalBudgetAPI as the main solution with two projects
   1) BudgetAPI with is the REST Controller
      a.  HomeBudgetController.cs is the REST interface
   3) DatabaseManager project has the following classes
      a. AppDbContext defines both the
           1. connection to Sql Server
           2. what the data models that represent the database tables look like to the EF Framework
      b. CRUD_Operations uses the AppDBContext and allow an update and fetch against the database.


      


