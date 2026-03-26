-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jun 12, 2025 at 09:15 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `bank_system`
--

-- --------------------------------------------------------

--
-- Table structure for table `customer_accounts`
--

CREATE TABLE `customer_accounts` (
  `id` int(11) NOT NULL,
  `name` varchar(100) DEFAULT NULL,
  `account_number` varchar(20) DEFAULT NULL,
  `balance` decimal(10,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `customer_accounts`
--

INSERT INTO `customer_accounts` (`id`, `name`, `account_number`, `balance`) VALUES
(1, 'Veasna', '1', 25451.22),
(3, 'Ly', '3', 2900.00),
(4, 'Ka', '32', 20080.00),
(5, 'Tra', '2', 43200.00),
(6, 'Seyha', '45', 39500.00),
(8, 'Sak', '16', 10100.00),
(9, 'Hour', '19', 1700.00);

-- --------------------------------------------------------

--
-- Table structure for table `employees`
--

CREATE TABLE `employees` (
  `id` int(11) NOT NULL,
  `username` varchar(50) NOT NULL,
  `password` varchar(255) NOT NULL,
  `name` varchar(100) NOT NULL,
  `role` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `employees`
--

INSERT INTO `employees` (`id`, `username`, `password`, `name`, `role`) VALUES
(1, 'admin', '1234', 'Admin User', 'Manager'),
(3, 'Hour', '123456', 'Ty chenghour', 'Manager'),
(5, 'Ly', '12347', 'Kimly', 'Manager');

-- --------------------------------------------------------

--
-- Table structure for table `transactions`
--

CREATE TABLE `transactions` (
  `id` int(11) NOT NULL,
  `account_number` varchar(50) NOT NULL,
  `transaction_type` varchar(20) NOT NULL,
  `amount` decimal(10,2) NOT NULL,
  `date` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `transactions`
--

INSERT INTO `transactions` (`id`, `account_number`, `transaction_type`, `amount`, `date`) VALUES
(3, '1', 'Cash In', 1000.00, '2025-06-01 15:09:25'),
(4, '1', 'Cash In', 1000.00, '2025-06-01 15:15:24'),
(5, '1', 'Cash In', 1000.00, '2025-06-01 15:22:37'),
(6, '1', 'Cash In', 2000.00, '2025-06-01 15:23:39'),
(7, '1', 'Cash In', 5000.00, '2025-06-01 15:26:08'),
(8, '1', 'Cash In', 5000.00, '2025-06-01 15:26:15'),
(9, '1', 'Cash Out', 5000.00, '2025-06-01 15:37:45'),
(10, '1', 'Cash In', 1000.00, '2025-06-01 15:39:15'),
(11, '1', 'Cash Out', 3000.00, '2025-06-01 15:45:40'),
(12, '1', 'Cash Out', 3000.00, '2025-06-01 15:48:46'),
(13, '1', 'Cash Out', 4000.00, '2025-06-01 15:51:00'),
(14, '1', 'Cash In', 5000.00, '2025-06-01 15:54:03'),
(15, '1', 'Cash In', 1000.00, '2025-06-01 15:54:45'),
(16, '2', 'Cash In', 10000.00, '2025-06-02 16:09:44'),
(17, '2', 'Cash In', 14000.00, '2025-06-02 16:09:50'),
(18, '2', 'Cash Out', 1000.00, '2025-06-02 16:19:01'),
(19, '1', 'Cash In', 5000.00, '2025-06-02 16:19:09'),
(20, '2', 'Cash In', 3400.00, '2025-06-02 16:19:14'),
(21, '2', 'Cash In', 15000.00, '2025-06-03 15:47:56'),
(22, '32', 'Cash In', 17800.00, '2025-06-03 15:48:21'),
(23, '32', 'Cash In', 1200.00, '2025-06-03 15:48:28'),
(24, '32', 'Cash Out', 120.00, '2025-06-03 15:48:35'),
(25, '45', 'Cash In', 35000.00, '2025-06-04 06:14:09'),
(26, '16', 'Cash In', 10000.00, '2025-06-05 09:36:30'),
(27, '19', 'Cash Out', 200.00, '2025-06-05 09:38:32');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `customer_accounts`
--
ALTER TABLE `customer_accounts`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `account_number` (`account_number`);

--
-- Indexes for table `employees`
--
ALTER TABLE `employees`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `transactions`
--
ALTER TABLE `transactions`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `customer_accounts`
--
ALTER TABLE `customer_accounts`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT for table `employees`
--
ALTER TABLE `employees`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `transactions`
--
ALTER TABLE `transactions`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=28;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
