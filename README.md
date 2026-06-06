# ⚡ Enterprise Offline Billing Engine

A modern, lightweight desktop Point of Sale (POS) and Billing application built with **WPF (.NET)** following the **MVVM architecture** and powered by **SQLite** via **Entity Framework Core**.

## 🚀 Key Features

- **Modern UI/UX:** Dark-themed responsive sidebar navigation with smooth view switching.
- **Interactive POS Screen:** Real-time barcode/product search, dynamic quantity updating, and instant subtotal, tax (VAT/GST), and total calculations.
<img width="1362" height="755" alt="image" src="https://github.com/user-attachments/assets/f3800564-df88-4109-a62b-5e8f874277d1" />
- **Customer Directory:** Reactive search modal dialog to bind invoices to specific tax profiles or default to a generic consumer.
- **Transactional Invoice History:** Full auditing screen to review past sales with a high-performance in-memory search filter.
- **Deep-Dive Details:** Eager-loaded relational pop-up window showing exact itemized breakdowns and stock history per invoice.
<img width="1792" height="586" alt="image" src="https://github.com/user-attachments/assets/8b076670-7041-442e-9bd3-d73ce0517390" />
- **Inventory Control:** Automatic stock deduction upon invoice finalization.

## 🛠️ Tech Stack

- **Framework:** .NET 8.0 / WPF (Windows Presentation Foundation)
- **Architecture Pattern:** MVVM (Model-View-ViewModel)
- **Database:** SQLite
- **ORM:** Entity Framework Core (Code-First approach with Migrations)

## 📦 How to Run the Project

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/joseantnei/enterprise-offline-billing-engine.git](https://github.com/joseantnei/enterprise-offline-billing-engine.git)
Open the solution in Visual Studio 2022.

Restore NuGet Packages if prompted.

Create the local SQLite database: Open the Package Manager Console (Tools > NuGet Package Manager > Package Manager Console) and run:

Plaintext
Update-Database
Press F5 to Build and Run! (An initial admin user and sample items will be automatically seeded into the database).

🗺️ Roadmap & Future Enhancements
To keep this portfolio project lean, some management modules are listed for future iterations:

[ ] User Authentication & Role Management (Login Screen).

[ ] Full Inventory & Customer CRUD Management Screens.

[ ] Daily Cash Closing (X/Z Reports) and PDF Invoice Export.

