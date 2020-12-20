# Customer Registration API

## Getting started
 
#### Database Creation
This project uses entity framework core. 
To create the database open the solution in Visual Studio and open the package manager console,
run this command.
```
dotnet ef database update
```

You should now have a database in your MSSQLLocalDb called **afi_customers_local_db**.

If you wish to change the location of the database edit the connection string in the 
appsettings.Development.json file located in AFI.Registration.Api.

#### Debugging
Set AFI.Registration.Api as your startup project and run with IIS Express.
You will see a **Swagger** page where you can test the api. 
Here are some example requests.
```
Good Request 

{
  "firstName": "john",
  "lastName": "Doe",
  "referenceNumber": "XX-123456",
  "dateOfBirth": "2000-01-01",
  "emailAddress": "abcd@123.com"
}
```
```
Bad Request

{
  "firstName": "Jo",
  "lastName": "Do",
  "referenceNumber": "X-123456",
  "dateOfBirth": "2021-12-18",
  "emailAddress": "abcd@123.net"
}
```

#### Integration and Unit Tests
Whilst developing the project I have been using [NCrunch](https://www.ncrunch.net/download), this was used for it's instant feedback
when writing tests.

#### Frameworks Used
- **Swagger**
  - For manually testing the api
- **Fluent assertions**
  - Readable test assertions
- **Fluent Validation**
  - Easy validation and rule building
- **Entityframework Core**
  - Database ORM
- **NodaTime**
  - A better framework for dealing with Datetime
- **NSubstitute & Autofixture**
  - Easy mocking and testing
- **XUnit**
  - Test runner

#### TODO:
Having more time I would move logging and exception handling out of the controller and have a
global handler.