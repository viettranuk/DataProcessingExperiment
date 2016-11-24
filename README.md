# Data Processor

This is a small web application that processes account transaction data to calculate tax figures for a client tax return. The focus is on the data input/manipulation stage of the process which is achieved through an upload feature. 

The transaction data has 4 columns: 

- Account (text)
- Description (text)
- Currency Code (string)
- Amount (decimal) 

and could be attached to a database with >1mil rows. The format of the data will be in Excel (.xlsx) or CSV and each individual file could contain up to 100k rows.

The application does the following:

- Allow the user to select a file and show the current file name (only CSV supported at this stage)
- Allow the user to upload the content of the current file to a table in a SQL database
- Each line of data is validated before it is uploaded

> All fields are required

> Currency Code must be a valid ISO 4217 currency code

> Amount must be a valid number

- On completion, the number of lines imported and the details of any lines ignored due to failed validation will be shown
- Allow the user to see all the transaction data in the system
- Allow user to remove or edit the transaction data (not currently supported)

The application adhered to best practices where appropriate such as SOLID principles.
