# CryptoMetaExchange

## Overview
The CryptoMetaExchange solution is a multi-project solution. It consists of four main components:

Shared Library: Contains core business logic, models, and services.
BSDigitalPart1 (Console App): A console application that reads exchange order data, calculates the best buy and sell orders, and displays the results.
BSDigitalPart2 (Web API): A web API that provides an HTTP interface for interacting with the order data and performing exchange evaluations.
Integration Tests (XUnit): Contains integration tests for testing the functionality of the Web API endpoints.
The core logic is abstracted into a shared library and both a console and web API application providing interfaces for users. The solution is also integrated with XUnit for testing and Dockerized.

## Shared library project

The Shared Library contains essential services, models, enums, and helpers used by the core applications (Console app and Web API) in the **CryptoMetaExchange** project. It provides business logic for tasks like price evaluation and data processing.

## Key Features

### Price Evaluation Service

The `PriceEvaluationService` provides the functionality to evaluate and calculate the best order prices for exchange orders. It interacts with `ExchangeOrderBook` objects and returns a list of `ExchangeOrder` objects based on specified conditions.

### Core Methods

#### `CalculateBestOrderPrice`

This method calculates the best order price based on the given `targetAmount` and `OrderType` (Buy or Sell). 

**Parameters:**
- `exchangeOrderBooks`: A list of exchange order books that include both bids (buy orders) and asks (sell orders).
- `targetAmount`: The total amount you want to buy or sell.
- `orderType`: Whether you are buying or selling.

The method processes the orders, selecting the best prices according to the order type:
- For `Buy`, it selects the lowest prices.
- For `Sell`, it selects the highest prices.

  ##
