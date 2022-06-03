# OnlineFoodOrderManagement
Micro Service for Menu Order Management

This Application is used for ordering the menu and also customise the vendor details according to their need.

Code base: .Net Core 5, SQL, Redis,Mongo, Docker, EF Core 5, Background Service (.Net core), Ocelot Gateway.<br/><br/>

<b><u>Overview of Application</u></b> <br/>
![](Documents/OverviewDiagram.PNG)

<p>the user will login the application and then based of the request diffrent microservice.</p>

<ul>
 <li> <b>Identity Micro Service</b><br/> User Authentication and getting user personal details. </li>
 <li> <b>Basket Service</b><br/> Redis cache is used for storing the cart information. </li>
 <li> <b>Menu Inventory MicroService</b><br/> Mongo DB is used to store custom vendor details and these details are handled in theis microservice.. </li>
 <li> <b>Order Microservice</b><br/> Used for ordering the user menu details. </li>
 <li> <b>Health Checks</b><br/> User Authentication and getting user personal details. </li>
 <li> <b>Identity Micro Service</b><br/>Can check if the API, SQL, Redis Cache are in working conditons or not. </li>
 </ul>

<p> Reference for cache <a href="https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks">HealthChecks</a></p>
 
 <p><b>What is Circuit Breaker?</b></p>
<p>Circuit Breaker is a design pattern, where you try a service call for a configured number of times. And if the service call 
fails consecutively for the configured number of times, you just open the circuit. This means the consecutive calls do not go
 to the service, rather immediately returned. And this continues to happen for a time duration configured.</p></br/><br/>
 
 ![](Documents/ServiceInstallerPollyCBRegister.png)
 <p>In the Project <b>MenuOrder.MenuService</b> HttpClientFactoryInstaller File the HttpClient is registered with client name as shown in the diagram above.
 </p><br/><br/>
 
  ![](Documents/HttpClientService.png)
 <p>Http client will call the named instance of httpClient i.e Basket MicroService and will try to call the url configured. If the 
 service is not up and running then circuit breaker comes into play and tries to call the API 5 times if success then result is 
 given or else it will not process and inform the user that server is busy.</p>
 
 <u><b>Basket Microservice</b></u><br/>
 <p>Basket Microservice is MenuManagement.BasketMicroService.API project in the solution folder. It uses Redis cache to store the User Cart Information.
 There are commands for POSTMAN collection for basket which are listed below
 <ul>
  <li>Initize User Cart : This will load the user cart with username</li>
  <li>Get User Basket Info : This will get the user basket information</li>
  <li>Add Item to Cart : This will add Item to cart</li>
  <li>Edit Basket Item : This will edit user cart item</li>
  <li>Delete Cart Item: This will delete user cart (full delete Items and Vendor Details)</li>
  <li>Get User basket count: This will get the count of basket Items</li>
 </ul>
 
 In order to start Basket Micorservice, we need to call initilize the user cart and only then we can use the other operation.
 </p>
 

