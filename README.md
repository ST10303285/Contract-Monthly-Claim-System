Contract Monthly Claim System
The Contract Monthly Claim System is a web-based program that makes it easier for lecturers, coordinators, and HR staff to submit, approve, and manage claims.
This system automates calculations, validations, and approvals to reduce manual errors and increase efficiency.

Features:
Lecture View:
-Submit claims by entering hours worked and hourly rates.
-Auto-calculation of claim amounts.
Coordinator View:
-Review and verify submitted claims
-Automated validation of hours and rates againt predefined criteriaa.
-Approve or reject claims with a single click.
HR view:
-Generate detailed invoices for approved claims.
-Manage lecturer data.
Automation Features:
-Auto-rejection of clauims exceeding policy limits.

SetUp Instructions:
Prerequisites:
-Visual Studio
-SQL Server(LocalDB)
-.NET Core SDK installed.

Steps to Run:
1. Clone Repository:
   git clone <repository-url>
   cd Contract-Monthly-Claim-System
2. Configure Database Connection:
   -Open appsettings.json in project directory
   -Update the DefaultConnection string to point to your SQL Server instance.
3. Run Database Migration:
   Update-Database
4. Start the application.

How it works:
1.Login/Register:
-Users register and login to access features relevent to their roles (dont need to register or login to access hr)
2.Claim Submission:
-Lecturers submit claims which are auto calculated and validated.
3.Approval Workflow:
-Coordinator review.approve, or reject claims. Automation ensures compliance.
4.Invoice Generation:
-HR staff generates and downlaods invoices for approved claims.
5.Lecturer Management:
-HR staff manage lecture details and cna edit, delete, or create new lecture profiles.

License
This project is for academic purposes only and is not licensed for commercial use.
