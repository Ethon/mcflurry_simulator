-- phpMyAdmin SQL Dump
-- version 3.5.5
-- http://www.phpmyadmin.net
--
-- Host: sql2.freemysqlhosting.net
-- Erstellungszeit: 02. Nov 2015 um 12:58
-- Server Version: 5.5.46-0ubuntu0.12.04.2
-- PHP-Version: 5.3.28

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Datenbank: `sql293335`
--

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `Artist`
--

CREATE TABLE IF NOT EXISTS `Artist` (
  `artistId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(64) NOT NULL,
  `email` varchar(64) NOT NULL,
  `categoryId` int(11) unsigned NOT NULL,
  `countryId` int(11) unsigned NOT NULL,
  `picturePath` varchar(128) NOT NULL,
  `videoPath` varchar(128) NOT NULL,
  PRIMARY KEY (`artistId`),
  KEY `categoryId` (`categoryId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `Category`
--

CREATE TABLE IF NOT EXISTS `Category` (
  `categoryId` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `shortcut` varchar(2) NOT NULL,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`categoryId`),
  UNIQUE KEY `shortcut` (`shortcut`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=28 ;

--
-- Daten für Tabelle `Category`
--

INSERT INTO `Category` (`categoryId`, `shortcut`, `name`) VALUES
(3, 'HU', 'Hubert'),
(4, 'J', 'Jonglage'),
(6, 'M', 'Iron Maiden'),
(8, 'OT', 'Figuren - und Objekttheater'),
(9, 'S', 'Samba'),
(10, 'ST', 'Stehstill - Statue'),
(13, 'Te', 'Test'),
(16, 'aA', 'aanzenen'),
(20, 'zu', 'zaanzenen'),
(22, 'AL', 'Albert'),
(23, 'RU', 'Rubert'),
(24, 'WT', 'wbhstiaall - Statue'),
(25, 'AP', 'AkrobatikARTSC'),
(27, 'ZQ', 'AkrobatikARasd');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `Country`
--

CREATE TABLE IF NOT EXISTS `Country` (
  `countryId` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(64) NOT NULL,
  `flagPath` varchar(32) NOT NULL,
  PRIMARY KEY (`countryId`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=6 ;

--
-- Daten für Tabelle `Country`
--

INSERT INTO `Country` (`countryId`, `name`, `flagPath`) VALUES
(3, 'Holland', 'fr'),
(5, 'Frankreich', 'fr');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `District`
--

CREATE TABLE IF NOT EXISTS `District` (
  `districtId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(64) NOT NULL,
  PRIMARY KEY (`districtId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `Performance`
--

CREATE TABLE IF NOT EXISTS `Performance` (
  `performanceId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `date` datetime NOT NULL,
  `artistId` int(11) unsigned NOT NULL,
  `venueId` int(10) unsigned NOT NULL,
  PRIMARY KEY (`performanceId`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=2 ;

--
-- Daten für Tabelle `Performance`
--

INSERT INTO `Performance` (`performanceId`, `date`, `artistId`, `venueId`) VALUES
(1, '2016-07-17 03:03:03', 1, 1);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `User`
--

CREATE TABLE IF NOT EXISTS `User` (
  `userId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `firstname` varchar(32) NOT NULL,
  `lastname` varchar(32) NOT NULL,
  `email` varchar(32) NOT NULL,
  PRIMARY KEY (`userId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `Venue`
--

CREATE TABLE IF NOT EXISTS `Venue` (
  `venueId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `shortcut` varchar(8) NOT NULL,
  `name` varchar(64) NOT NULL,
  `latitude` double NOT NULL,
  `longitude` double NOT NULL,
  PRIMARY KEY (`venueId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
