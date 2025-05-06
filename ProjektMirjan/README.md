## Instalacja

Struktura tabel bazy danych

```bash
CREATE TABLE ExchangeRateTables (
    Id INT PRIMARY KEY IDENTITY,
    TableCode NVARCHAR(1),
    No NVARCHAR(20),
    EffectiveDate DATE
);

CREATE TABLE CurrencyRates (
    Id INT PRIMARY KEY IDENTITY,
    Currency NVARCHAR(50),
    Code NVARCHAR(3),
    Mid DECIMAL(18, 4),
    ExchangeRateTableId INT FOREIGN KEY REFERENCES ExchangeRateTables(Id)
);
```

Skrypt do tworzenia tabel
```bash
dotnet ef migrations add Migracja
dotnet ef database update
```
