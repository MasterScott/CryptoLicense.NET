-- phpMyAdmin SQL Dump
-- version 4.9.4
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Jan 17, 2021 at 03:30 PM
-- Server version: 10.3.27-MariaDB-log-cll-lve
-- PHP Version: 7.3.6

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `biitzyql_database`
--

-- --------------------------------------------------------

--
-- Table structure for table `CryptoLicenseDB`
--

CREATE TABLE `CryptoLicenseDB` (
  `id` int(11) NOT NULL,
  `license` varchar(200) NOT NULL,
  `banned` int(11) NOT NULL,
  `hwid` varchar(300) NOT NULL,
  `multi` int(11) NOT NULL,
  `ipaddress` varchar(100) NOT NULL,
  `sessionid` varchar(50) NOT NULL,
  `accesskey` varchar(50) NOT NULL,
  `stamp` varchar(200) NOT NULL,
  `rank` int(15) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Dumping data for table `CryptoLicenseDB`
--

INSERT INTO `CryptoLicenseDB` (`id`, `license`, `banned`, `hwid`, `multi`, `ipaddress`, `sessionid`, `accesskey`, `stamp`, `rank`) VALUES
(1, 'test', 0, 'bfaee554dc88493a9c9e52e16082ecc6', 2, '162.213.255.50', '', '', '', 7);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `CryptoLicenseDB`
--
ALTER TABLE `CryptoLicenseDB`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `CryptoLicenseDB`
--
ALTER TABLE `CryptoLicenseDB`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
