# Project Name: Inkwell

RESTful API built with ASP.NET Core Web Api 8.0 
This project was started with educational purpose and it depends on FMI project. 


## Features

- Book Management (CRUD): Full Create, Read, Update, and Delete capabilities for managing a personalized library.
- Secure Authentication: Protected endpoints using JWT (JSON Web Token)
- OpenAPI/Swagger Integration


## Tech Stack

- Framework: ASP.NET Core Web Api 8.0
- Database: MongoDB


## Installation & Setup

1. Clone the repository:

This project depends on **[FMI]** for user creation and authentication. Please clone first FMI one. 
Then clone this project and include the "../fmi/fmi.csproj" file in the Dependencies section.

``` bash
git clone https://github.com/your-username/your-repo-name.git
cd your-repo-name
```

2. Set the db link:

 In "appsettings.json" put your MongoDb connection string.

``` json
"ConnectionStrings": {
    "MongoDb": "mongodb://your-db-string"
  }
```

3. Run the application
