Dynamic Table Generator and Data Persistence using .NET 8, MVC, AJAX, and SQL Server
Project Overview
This project aims to create a dynamic table generator and store data in a SQL Server database using the latest technologies such as .NET 8, MVC, AJAX, and SQL Server. The system is designed to efficiently manage invoices and their details.

Features
1. Dynamic Table Generation
Invoice Form: Allows users to fill in invoice details dynamically.
Select Party Name from a list.
Enter Invoice No.
Choose items from a dynamic dropdown list.
Select units for each item.
Specify quantity, rate, and view calculated values.
2. Database Schema
DOCHDR Table:

DochdrId: Unique identifier for each invoice header.
PartyId: Corresponds to the selected party.
InvoiceNo: Invoice number.
InvoiceDate: Date of the invoice.
TotalAmount: Total amount for the invoice.
User: User responsible for the invoice.
DefaultDateTime: Date and time of the invoice creation.
DOCDET Table:

DocdetId: Unique identifier for each invoice detail.
HdrautoId: Foreign key linking to DochdrId.
SrNo: Serial number for items.
ItemId: Identifier for each item.
UomId: Unit of measure for the item.
Qty: Quantity of the item.
Rate: Rate of the item.
Value: Calculated value for the item.
3. Technologies Used
.NET 8: Utilizing the latest features and improvements in the .NET framework.
MVC (Model-View-Controller): A design pattern for separating concerns and improving code organization.
AJAX (Asynchronous JavaScript and XML): Enhancing user experience by allowing asynchronous data retrieval.
SQL Server Database: Storing and managing invoice data efficiently.
Getting Started
Clone the repository: git clone <repository_url>
Set up the database: Execute SQL scripts to create the necessary tables.
Configure the connection string: Update the connection string in the project settings.
Run the application: Start the application using .NET CLI or Visual Studio.
