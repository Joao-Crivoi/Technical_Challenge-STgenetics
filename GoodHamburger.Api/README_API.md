# Good Hamburger API - STgenetics Technical Challenge

This project is a REST API developed in C# with .NET/ASP.NET Core for the **Good Hamburger** snack bar order registration system. The goal is to demonstrate code organization practices, domain modeling, and architectural decision-making.

## 📋 Business Rules (Domain)

### Menu
- **Sandwiches:**
  - X Burger: R$ 5.00
  - X Egg: R$ 4.50
  - X Bacon: R$ 7.00
- **Sides & Drinks:**
  - French fries: R$ 2.00
  - Soda: R$ 2.50

### Discount Rules (Combos)
The system automatically applies discounts based on the order composition:
- **Full Combo (Sandwich + Fries + Soda):** 20% discount.
- **Sandwich + Soda Combo:** 15% discount.
- **Sandwich + Fries Combo:** 10% discount.

### Constraints
- Each order can contain **only one item from each category** (one sandwich, one side, and one drink).
- Duplicate items are validated and return clear error messages.

## 🛠️ Technical Details and Requirements

- **Framework:** .NET / ASP.NET Core.
- **Persistence:** Entity Framework Core (SQLite for lightweight development).
- **Calculations:** The system automatically calculates the **Subtotal**, **Discount Amount**, and **Final Total**.
- **Validation:** Error handling for resources not found, invalid orders, and duplicate items.

---
*This document serves as a technical reference guide for the specifications implemented in this microservice.*