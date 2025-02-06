using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWebApp.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"
                INSERT INTO categories (category_name) VALUES ('Телевізори');
                INSERT INTO categories (category_name) VALUES ('Холодильники');
                INSERT INTO categories (category_name) VALUES ('Пилососи');
            ");

            migrationBuilder.Sql(@"
                INSERT INTO customers (full_name, phone, email, address, passport_number)
                VALUES ('Іван Петров', '+380501234567', 'ivan.petrov@email.com', 'Київ, вул. Хрещатик, 12', 'AB123456');
                INSERT INTO customers (full_name, phone, email, address, passport_number)
                VALUES ('Анна Коваль', '+380971112233', 'anna.koval@email.com', 'Львів, вул. Шевченка, 45', 'CD654321');
            ");

            migrationBuilder.Sql(@"
                INSERT INTO equipment (name, brand, model, category_id, base_price_per_day)
                VALUES ('Телевізор LG OLED', 'LG', 'OLED55C1', 1, 100);
                INSERT INTO equipment (name, brand, model, category_id, base_price_per_day)
                VALUES ('Холодильник Samsung', 'Samsung', 'RF28R7351SG', 2, 150);
                INSERT INTO equipment (name, brand, model, category_id, base_price_per_day)
                VALUES ('Пилосос Dyson', 'Dyson', 'V11', 3, 80);
            ");

            migrationBuilder.Sql(@"
                INSERT INTO inventory (equipment_id, serial_number, purchase_date, condition, availability)
                VALUES (1, 'LG123456789', '2023-01-15', 'new', 1);
                INSERT INTO inventory (equipment_id, serial_number, purchase_date, condition, availability)
                VALUES (2, 'SAMSUNG987654', '2022-11-10', 'used', 1);
                INSERT INTO inventory (equipment_id, serial_number, purchase_date, condition, availability)
                VALUES (3, 'DYSON54321', '2023-05-20', 'new', 1);
            ");

            migrationBuilder.Sql(@"
                INSERT INTO tariffs (tariff_name, multiplier, discount_percentage)
                VALUES ('Базовий', 1.0, 0);
                INSERT INTO tariffs (tariff_name, multiplier, discount_percentage)
                VALUES ('Тижневий', 0.9, 10);
                INSERT INTO tariffs (tariff_name, multiplier, discount_percentage)
                VALUES ('Місячний', 0.75, 25);
            ");
            migrationBuilder.Sql(@"
                INSERT INTO rentals (customer_id, inventory_id, start_date, end_date, tariff_id, total_price, status)
                VALUES (1, 1, '2024-02-10', '2024-02-20', 2, 900, 'active');
                INSERT INTO rentals (customer_id, inventory_id, start_date, end_date, tariff_id, total_price, status)
                VALUES (1, 2, '2024-03-01', '2024-03-10', 1, 1500, 'completed');
                INSERT INTO rentals (customer_id, inventory_id, start_date, end_date, tariff_id, total_price, status)
                VALUES (2, 3, '2024-03-05', '2024-03-12', 3, 600, 'active');
            ");

            migrationBuilder.Sql(@"
                INSERT INTO payments (rental_id, payment_date, payment_method, amount)
                VALUES (1, '2024-02-20', 'credit_card', 900);
                INSERT INTO payments (rental_id, payment_date, payment_method, amount)
                VALUES (2, '2024-03-10', 'cash', 1500);
                INSERT INTO payments (rental_id, payment_date, payment_method, amount)
                VALUES (3, '2024-03-12', 'credit_card', 600);
            ");

            migrationBuilder.Sql(@"
                INSERT INTO defects (inventory_id, defect_date, description, status, repair_cost)
                VALUES (2, '2024-03-15', 'Проблеми з охолодженням', 'active', NULL);
                INSERT INTO defects (inventory_id, defect_date, description, status, repair_cost)
                VALUES (3, '2024-04-10', 'Пошкоджена щітка', 'fixed', 50);
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
