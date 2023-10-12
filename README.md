# OnlineFoodOrderManagement
Micro Service for Menu Order Management

This Application is used for ordering the menu and also customise the vendor details according to their need.

There are 3 roles User, Admin and Vendor.<br/><br/>
<b>User:</b> <p>Here User will register to view the vendor menu and place items in cart based on the nearest location.</p><br/>
<b>Admin:</b> <p>Here Admin will be able to edit any vendor details. Add Vendor Disable Vendor, add notiifcation.</p><br/>
<b>Vendor:</b> <p>Here Vendor will register their shop in the application. Configure the Vendor menu based on thier needs.</p><br/>
<b>IDS Admin:</b> <p>Here it is used for Microserive registration and token managemnt system. Angular UI will call the IDS server to get the Token and using
the  token we will call individual Microservice based on the access level. If the API is unable to access then we need to give necessary access level by
registering it as an Client ,API Resource and API Scope.
</p><br/>
Code base: .Net Core 6 .Net Core MVC 6, SQL,MongoDB, Docker, IDs server is a mix of API and GraphQl.<br/><br/>

Front End: Angular 15 for UI. <br/>
Front End: React Js for Identity server UI.<br/><br/>

Front end like Angular : https://github.com/shreyasudupas/MenuOrderAngular <br/>
Front end ReactJs: https://github.com/shreyasudupas/react-menu-ids-ui <br/>

<b><u>Overview of Application</u></b> <br/>
![](Documents/OverviewDiagram.PNG)

<p>Here we a single SignOn for all the Users (Admin,Vendor and User). The single SignOn is maagned by IdentityServer4 template. It has a separate admin portal which is designed using ReactJs</p>

<p>Here are some of the pics for Vendor, It shows the Vendor Details page where vendor can configure the menu, enter hotel location using the maps.</p>
![](Documents/VendorVendorDetail.png)<br/><br/>

<p>Here are some of the pics for Admin, Here Admin can configure vendor details, edit vendor location using maps, add/edit category, add/edit menu images, add/edit cuisine items, verify newly added vendor, add new notification.</p>
<b>Admin Vendor List</b><br/>
![](Documents/AdminVendorListPage.png)<br/><br/>

<b>Admin Menu Page</b><br/>
![](Documents/AdminVendorMenuPage.png)<br/><br/>

<b>Admin Menu Item Image</b><br/>
![](Documents/AdminMenuItemImage.png)<br/><br/>

<b>Admin Notification Dashboard</b><br/>
![](Documents/AdminNotificationDashboard.png)<br/><br/>

<p>Here are some of the pics for Vendor, It shows the Vendor Details page where vendor can configure the menu, enter hotel location using the maps.</p>
![](Documents/VendorVendorDetail.png)</p><br/><br/>

<p>Here Users Page is used by end users to view their choice of restaurants, add menu to cart, make payments (right now only supports reward based transaction. </p><br/>
<b>User Menu Page</b><br/>
![](Documents/UserMenuPage.png)<br/><br/>

<b>User Menu Page</b><br/>
![](Documents/AdminVendorMenuPage.png)<br/><br/>

<b>User Menu Page</b><br/>
![](Documents/UserMenuDetailPage.png)<br/><br/>

<b>User Cart Page</b><br/>
![](Documents/UserCartPage.png)<br/><br/>

<b>User Payment Page</b><br/>
![](Documents/UserPaymentPage.png)<br/><br/>

<p> Reference for cache <a href="https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks">HealthChecks</a></p>
 
 <p><b>What is Circuit Breaker?</b></p>
<p>Circuit Breaker is a design pattern, where you try a service call for a configured number of times. And if the service call 
fails consecutively for the configured number of times, you just open the circuit. This means the consecutive calls do not go
 to the service, rather immediately returned. And this continues to happen for a time duration configured.</p></br/><br/>

 </p>
 

