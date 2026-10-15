namespace OnlineShopApi.Data.Seed;

public static class SeedCatalog
{
    public record SeedAdmin(string Email, string Password, string DisplayName, string Department, string Notes);

    public record SeedCustomer(string Email, string Password, string FullName, string BillingAddress, string DefaultShippingAddress, string Country, string Phone);

    public record SeedProduct(string Sku, string Name, int Price, int Weight, string Descriptions, string Category, DateTime CreateDate, int Stock);

    public record SeedOrderLine(string Sku, int Price, int Quantity);

    public record SeedOrder(string CustomerEmail, int Ammount, string ShippingAdress, string OrderAdress, DateOnly OrderDate, string OrderStatus, SeedOrderLine[] Lines);

    public static readonly SeedAdmin[] Admins =
    [
        new("admin@shop.com", "admin1234", "Site Administrator", "Operations", "Default seeded admin account."),
        new("admin@onlineshop.ro", "admin123", "Admin Shop", "Operations", "Seeded from db/seed-data.sql.")
    ];

    public static readonly SeedCustomer[] Customers =
    [
        new("customer@shop.com", "customer1234", "Demo Customer", "Demo Street 1", "Demo Street 1", "RO", "0712345678"),
        new("andrei.popescu@email.ro", "parola123", "Andrei Popescu", "Str. Victoriei 45, Sector 1, Bucuresti", "Str. Victoriei 45, Sector 1, Bucuresti", "Romania", "0722 111 222"),
        new("maria.ionescu@email.ro", "maria2024", "Maria Ionescu", "Bd. Unirii 12, Cluj-Napoca", "Bd. Unirii 12, Cluj-Napoca", "Romania", "0733 222 333"),
        new("klaus.muller@email.de", "klaus2024", "Klaus Muller", "Berliner Str. 88, Berlin", "Berliner Str. 88, Berlin", "Germania", "+49 170 123 456"),
        new("elena.v@email.ro", "elena2024", "Elena Vasilescu", "Str. Mihai Eminescu 7, Iasi", "Str. Libertatii 23, Iasi", "Romania", "0744 333 444"),
        new("pierre.dubois@email.fr", "pierre2024", "Pierre Dubois", "15 Rue de Rivoli, Paris", "15 Rue de Rivoli, Paris", "Franta", "+33 6 12 34 56")
    ];

    public static readonly SeedProduct[] Products =
    [
        new("SKU-EL-001", "Laptop ProMax 15", 4599, 2100, "Laptop performant cu procesor ultima generatie, 16GB RAM, 512GB SSD. Ecran IPS Full HD de 15.6 inch.", "Electronics", Date(2024, 1, 15), 45),
        new("SKU-EL-002", "Monitor UltraWide 34\"", 2199, 6800, "Monitor curbat IPS, rezolutie WQHD 3440x1440, timp de raspuns 1ms, 144Hz.", "Electronics", Date(2024, 1, 18), 22),
        new("SKU-EL-003", "Tastatura Mecanica RGB", 349, 820, "Tastatura mecanica cu switch-uri Cherry MX Red, iluminare RGB per tasta, layout full-size.", "Electronics", Date(2024, 3, 5), 31),
        new("SKU-EL-004", "Mouse Wireless Ergonomic", 189, 95, "Mouse wireless ergonomic, sensor 16000 DPI, baterie reincarcabila, conectivitate Bluetooth + USB.", "Electronics", Date(2024, 3, 10), 58),
        new("SKU-CL-010", "Tricou Sport Dry-Fit", 89, 180, "Material respirabil, uscare rapida, ideal pentru antrenament. Disponibil in 5 culori.", "Clothing", Date(2024, 2, 3), 5),
        new("SKU-CL-011", "Geaca Iarna Puf", 599, 750, "Geaca cu puf natural, rezistenta la apa, captuseala termica, gluga detasabila.", "Clothing", Date(2024, 3, 12), 18),
        new("SKU-HM-020", "Lampa LED Birou", 149, 950, "Lampa LED cu 3 moduri de iluminare (cald, neutru, rece), brat flexibil, port USB integrat.", "Home", Date(2024, 2, 10), 0),
        new("SKU-HM-021", "Set Prosoape Bumbac", 79, 600, "Set 4 prosoape 100% bumbac egipt, 600gsm, diverse culori. Include 2 mari si 2 mici.", "Home", Date(2024, 3, 18), 8),
        new("SKU-SP-015", "Minge Fotbal Pro", 120, 430, "Minge profesionala FIFA Quality Pro, marime 5, cusuta manual, material PU premium.", "Sports", Date(2024, 3, 1), 67),
        new("SKU-SP-016", "Banda Alergare Pliabila", 2899, 35000, "Banda de alergare pliabila, motor 2.5HP, viteza max 16km/h, ecran LCD, 12 programe presetate.", "Sports", Date(2024, 4, 1), 3)
    ];

    public static readonly SeedOrder[] Orders =
    [
        new("andrei.popescu@email.ro", 4897, "Str. Victoriei 45, Bucuresti", "Str. Victoriei 45, Bucuresti", new DateOnly(2024, 4, 1), "Delivered",
            [new("SKU-EL-001", 4599, 1), new("SKU-CL-010", 89, 2), new("SKU-SP-015", 120, 1)]),
        new("maria.ionescu@email.ro", 689, "Bd. Unirii 12, Cluj-Napoca", "Bd. Unirii 12, Cluj-Napoca", new DateOnly(2024, 4, 2), "Processing",
            [new("SKU-CL-011", 599, 1), new("SKU-CL-010", 89, 1)]),
        new("klaus.muller@email.de", 2548, "Berliner Str. 88, Berlin", "Berliner Str. 88, Berlin", new DateOnly(2024, 4, 3), "Shipped",
            [new("SKU-EL-002", 2199, 1), new("SKU-EL-003", 349, 1)]),
        new("pierre.dubois@email.fr", 120, "15 Rue de Rivoli, Paris", "15 Rue de Rivoli, Paris", new DateOnly(2024, 4, 5), "Cancelled",
            [new("SKU-SP-015", 120, 1)]),
        new("andrei.popescu@email.ro", 947, "Str. Victoriei 45, Bucuresti", "Str. Victoriei 45, Bucuresti", new DateOnly(2024, 4, 7), "Processing",
            [new("SKU-EL-003", 349, 1), new("SKU-CL-011", 599, 1)])
    ];

    private static DateTime Date(int year, int month, int day) => new(year, month, day, 0, 0, 0, DateTimeKind.Utc);
}
