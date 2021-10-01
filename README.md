# OnlineFoodOrderManagement
Micro Service for Menu Order Management

This Application is used for ordering the menu and also customise the vendor details according to their need.

Code base: .Net Core 5, SQL, Redis,Mongo, Docker, EF Core 5, Background Service (.Net core), Ocelot Gateway.

<b><u>Overview of Application</u></b> <br/>
![](Documents/OverviewDiagram.PNG)

the user will login the application and then based of the request diffrent microservice.

1) Identity Micro Service
 User Authentication and getting user personal details
 
2) Basket Service
  Redis cache is used for storing the cart information
  
3) Menu Inventory MicroService
 Mongo DB is used to store custom vendor details and these details are handled in theis microservice.
 
4) Order Microservice
  USed for ordering the user menu details.

5) Health Checks
 Can check if the API, SQL, Redis Cache are in working conditons or not.
 
 Reference for cache <a href="https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks">HealthChecks</a>
