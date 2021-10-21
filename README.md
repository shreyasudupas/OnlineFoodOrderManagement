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
  Used for ordering the user menu details.

5) Health Checks
 Can check if the API, SQL, Redis Cache are in working conditons or not.
 
 Reference for cache <a href="https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks">HealthChecks</a>
 
 <p><b>What is Circuit Breaker?</b></p>
<p>Circuit Breaker is a design pattern, where you try a service call for a configured number of times. And if the service call 
fails consecutively for the configured number of times, you just open the circuit. This means the consecutive calls do not go
 to the service, rather immediately returned. And this continues to happen for a time duration configured.</p>
 
 <p>In the Project <b>MenuOrder.MenuService</b> HttpClientFactoryInstaller File the HttpClient is registered with client name as shown in the diagram
 </p>
 ![](Documents/ServiceInstallerPollyCBRegister.png)
 
 <p>Http client will call the named instance of httpClient i.e Basket MicroService and will try to call the url configured. If the 
 service is not up and running then circuit breaker comes into play and tries to call the API 5 times if success then result is 
 given or else it will not process and inform the user that server is busy.</p>
 ![](Documents/HttpClientService.png)
