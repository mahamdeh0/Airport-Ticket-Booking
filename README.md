# Airport Ticket Booking System
[![build and test](https://github.com/mahamdeh0/Airport-Ticket-Booking/actions/workflows/build-and-test.yml/badge.svg?branch=testing-with-xunit)](https://github.com/mahamdeh0/Airport-Ticket-Booking/actions/workflows/build-and-test.yml)

Welcome to the Airport Ticket Booking System! This console-based application allows passengers to book flight tickets and manage their reservations while providing managers with tools to handle bookings and import flight data. The system supports essential functionalities such as booking, searching, and managing flights, as well as batch uploading and validation of flight data.

## Features

### For Passengers:
- **Book a Flight**: Select a flight based on search parameters, choose a class (Economy, Business, First Class), and complete the booking.
- **Search Available Flights**: Find flights by price, departure country, destination country, departure date, departure and arrival airports, and class.
- **Manage Bookings**: Cancel or modify existing bookings and view personal reservations.

### For Managers:
- **Filter Bookings**: Search and filter bookings by flight details, price, departure and destination information, passenger details, and class.
- **Batch Flight Upload**: Import flight data from a CSV file into the system.
- **Validate Imported Flight Data**: Check the imported flight data for errors and receive a detailed list of issues to address.

## Principles Applied

- **Single Responsibility Principle**: Each class and method has a clear, single responsibility, enhancing maintainability and readability.
- **Open/Closed Principle**: The design accommodates feature extensions without altering existing code.
- **Liskov Substitution Principle**: Interfaces are used to ensure that components can be replaced with compliant implementations.
- **Interface Segregation Principle**: Interfaces are tailored to specific functionalities, avoiding unnecessary complexity.
- **Dependency Inversion Principle**: High-level modules depend on abstractions rather than concrete implementations, fostering flexibility.

## Validation and Error Handling

- **Input Validation**: Ensures data integrity through constraints and validation checks on user inputs.
- **Error Handling**: Employs try-catch blocks to manage exceptions and provide informative error messages.

## Getting Started

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/yourusername/airport-ticket-booking-system.git
