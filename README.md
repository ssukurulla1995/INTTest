Project Overview:
This project is designed to create a dynamic order management system using the latest technologies such as .NET 8, MVC, AJAX, and SQL Server. It allows for efficient creation, management, and storage of order details in a database.

Features:
1. Dynamic Table Generation
Order Form: Allows users to dynamically fill in order details.
Select Customer Name from a list.
Enter Order No.
Choose products from a dynamic dropdown list.
Select units for each product.
Specify quantity, rate, and view calculated values (e.g., subtotal).
2. Database Schema:
Order Header (ORDHDR):
OrdHdrId: Unique identifier for each order header.
CustomerId: Corresponds to the selected customer.
OrderNo: Order number.
OrderDate: Date of the order.
TotalAmount: Total amount for the order.
User: User responsible for creating the order.
DefaultDateTime: Date and time of order creation.
Order Details (ORDDET):
OrdDetId: Unique identifier for each order detail.
HdrAutoId: Foreign key linking to OrdHdrId.
SrNo: Serial number for products.
ProductId: Identifier for each product.
UomId: Unit of measure for the product.
Qty: Quantity of the product.
Rate: Rate of the product.
Value: Calculated value for the product.
3. Technologies Used:
.NET 8: Leveraging the latest framework for improved performance and features.
MVC (Model-View-Controller): Separating concerns for better code organization and maintainability.
AJAX: Enhancing the user experience with asynchronous data updates.
SQL Server Database: Efficiently storing and managing order data.
Getting Started:
Clone the repository: git clone 
Set up the database: Execute the provided SQL scripts to create the necessary tables.
Configure the connection string: Update the connection string in the project settings to point to your SQL Server instance.
Run the application: Use the .NET CLI or Visual Studio to start the application.
