# OnlineFoodOrderManagement
Micro Service for Menu Order Management

This Application is used for ordering the menu and also customise the vendor details according to their need.

There are 3 roles User, Admin and Vendor.<br/><br/>
<b>User:</b> <p>Here User will register to view the vendor menu and place items in cart based on the nearest location</p>.<br/>
<b>Admin:</b> <p>Here Admin will be able to edit any vendor details. Add Vendor Disable Vendor, add notiifcation.</p>.<br/>
<b>Vendor:</b> <p>Here Vendor will register their shop in the application. Configure the Vendor menu based on thier needs.</p>.<br/>
<b>IDS Admin:</b> <p>Here it is used for Microserive registration and token managemnt system</p>.<br/>
Code base: .Net Core 6 .Net Core MVC 6, SQL,MongoDB, Docker, IDs server is a mix of API and GraphQl.<br/><br/>

Front End: Angular 15 for UI. <br/>
Front End: React Js for Identity server UI.<br/><br/>

Front end like Angular : https://github.com/shreyasudupas/MenuOrderAngular <br/>
Front end ReactJs: https://github.com/shreyasudupas/react-menu-ids-ui <br/>

<b><u>Overview of Application</u></b> <br/>
![](Documents/OverviewDiagram.PNG)

<p>the user will login the application and then based of the request diffrent microservice.</p>

<p> Reference for cache <a href="https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks">HealthChecks</a></p>
 
 <p><b>What is Circuit Breaker?</b></p>
<p>Circuit Breaker is a design pattern, where you try a service call for a configured number of times. And if the service call 
fails consecutively for the configured number of times, you just open the circuit. This means the consecutive calls do not go
 to the service, rather immediately returned. And this continues to happen for a time duration configured.</p></br/><br/>

 </p>
 

