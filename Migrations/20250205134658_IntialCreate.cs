using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWebApp.Migrations
{
    public partial class IntialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"

                -- Customers table
                CREATE TABLE customers (
                    id INT IDENTITY(1,1) PRIMARY KEY,
                    full_name NVARCHAR(255) COLLATE Cyrillic_General_CI_AS NOT NULL,
                    phone NVARCHAR(20) UNIQUE NOT NULL,
                    email NVARCHAR(255) UNIQUE,
                    address NVARCHAR(MAX),
                    passport_number NVARCHAR(50) UNIQUE
                );

                -- Categories table
                CREATE TABLE categories (
                    id INT IDENTITY(1,1) PRIMARY KEY,
                    category_name NVARCHAR(255) COLLATE Cyrillic_General_CI_AS UNIQUE NOT NULL
                );

                -- Equipment table
                CREATE TABLE equipment (
                    id INT IDENTITY(1,1) PRIMARY KEY,
                    name NVARCHAR(255) NOT NULL,
                    brand NVARCHAR(255),
                    model NVARCHAR(255),
                    category_id INT,
                    base_price_per_day DECIMAL(10,2) NOT NULL,
                    FOREIGN KEY (category_id) REFERENCES categories(id) ON DELETE SET NULL
                );

                -- Inventory table
                CREATE TABLE inventory (
                    id INT IDENTITY(1,1) PRIMARY KEY,
                    equipment_id INT,
                    serial_number NVARCHAR(255) UNIQUE NOT NULL,
                    purchase_date DATE NOT NULL,
                    condition NVARCHAR(50) CHECK (condition IN ('new', 'used', 'damaged')) NOT NULL DEFAULT 'new',
                    availability BIT NOT NULL DEFAULT 1,
                    FOREIGN KEY (equipment_id) REFERENCES equipment(id) ON DELETE CASCADE
                );

                -- Tariffs table
                CREATE TABLE tariffs (
                    id INT IDENTITY(1,1) PRIMARY KEY,
                    tariff_name NVARCHAR(255) NOT NULL,
                    multiplier DECIMAL(5,2) NOT NULL,
                    discount_percentage DECIMAL(5,2) NOT NULL DEFAULT 0
                );

                -- Rentals table
                CREATE TABLE rentals (
                    id INT IDENTITY(1,1) PRIMARY KEY,
                    customer_id INT,
                    inventory_id INT,
                    start_date DATE NOT NULL,
                    end_date DATE NOT NULL,
                    tariff_id INT,
                    total_price DECIMAL(10,2) NOT NULL,
                    status NVARCHAR(50) CHECK (status IN ('active', 'completed', 'overdue')) NOT NULL DEFAULT 'active',
                    FOREIGN KEY (customer_id) REFERENCES customers(id) ON DELETE CASCADE,
                    FOREIGN KEY (inventory_id) REFERENCES inventory(id) ON DELETE CASCADE,
                    FOREIGN KEY (tariff_id) REFERENCES tariffs(id) ON DELETE SET NULL
                );

                -- Payments table
                CREATE TABLE payments (
                    id INT IDENTITY(1,1) PRIMARY KEY,
                    rental_id INT,
                    payment_date DATE NOT NULL,
                    payment_method NVARCHAR(50) CHECK (payment_method IN ('cash', 'credit_card', 'online_transfer')) NOT NULL,
                    amount DECIMAL(10,2) NOT NULL,
                    FOREIGN KEY (rental_id) REFERENCES rentals(id) ON DELETE CASCADE
                );

                -- Defects table
                CREATE TABLE defects (
                    id INT IDENTITY(1,1) PRIMARY KEY,
                    inventory_id INT,
                    defect_date DATE NOT NULL,
                    description NVARCHAR(MAX) NOT NULL,
                    status NVARCHAR(50) CHECK (status IN ('active', 'fixed')) NOT NULL DEFAULT 'active',
                    repair_cost DECIMAL(10,2),
                    FOREIGN KEY (inventory_id) REFERENCES inventory(id) ON DELETE CASCADE
                );
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF OBJECT_ID('defects', 'U') IS NOT NULL DROP TABLE defects;
                IF OBJECT_ID('payments', 'U') IS NOT NULL DROP TABLE payments;
                IF OBJECT_ID('rentals', 'U') IS NOT NULL DROP TABLE rentals;
                IF OBJECT_ID('tariffs', 'U') IS NOT NULL DROP TABLE tariffs;
                IF OBJECT_ID('inventory', 'U') IS NOT NULL DROP TABLE inventory;
                IF OBJECT_ID('equipment', 'U') IS NOT NULL DROP TABLE equipment;
                IF OBJECT_ID('categories', 'U') IS NOT NULL DROP TABLE categories;
                IF OBJECT_ID('customers', 'U') IS NOT NULL DROP TABLE customers;
            ");
        }
    }
}
