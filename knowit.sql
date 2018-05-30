-- phpMyAdmin SQL Dump
-- version 4.7.9
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Generation Time: 2018-05-30 16:49:32
-- 服务器版本： 5.5.59-log
-- PHP Version: 5.6.35

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `knowit`
--

-- --------------------------------------------------------

--
-- 表的结构 `comments`
--

CREATE TABLE `comments` (
  `id` int(11) NOT NULL,
  `comUser` varchar(100) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `message` varchar(200) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- 转存表中的数据 `comments`
--

INSERT INTO `comments` (`id`, `comUser`, `message`) VALUES
(1, 'Palette', 'Great, I like it very much~');

-- --------------------------------------------------------

--
-- 表的结构 `posts`
--

CREATE TABLE `posts` (
  `id` int(11) NOT NULL,
  `title` varchar(50) DEFAULT NULL,
  `editor` varchar(100) DEFAULT NULL,
  `content` varchar(1000) DEFAULT NULL,
  `imageUrl` varchar(50) DEFAULT NULL,
  `mediaUrl` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- 转存表中的数据 `posts`
--

INSERT INTO `posts` (`id`, `title`, `editor`, `content`, `imageUrl`, `mediaUrl`) VALUES
(1, 'SecondTry', 'Palette', 'Fucking Jian Yang!', '1-Palette-SecondTry.jpg', ''),
(2, 'ThirdTry', 'Palette', 'Fucking Jian Yang!', '2-Palette-ThirdTry.jpg', ''),
(3, 'asshole', 'Fucker', 'shit sssssssssssssssssssssssssssssssssssssssssssssssss', '3-Fucker-asshole.jpg', ''),
(4, 'asshole', 'Fucker', 'sssssssssssssss', '4-Fucker-asshole.jpg', ''),
(5, 'ssssssss', 'Fucker', 'saaaaaaaa', '5-Fucker-ssssssss.jpg', ''),
(6, 'dddddd', 'Fucker', 'dddddddddddd', '6-Fucker-dddddd.jpg', ''),
(7, 'dddfdccdc', 'Fucker', 'cccccccccccc', '7-Fucker-dddfdccdc.jpg', ''),
(8, 'shit', 'Fucker', '77777777777777', '', ''),
(9, 'eeeeeeeee', 'Fucker', 'eeeeeee', '', ''),
(10, 'shitty', 'Fucker', 'shittysssssss', '', '10-Fucker-shitty.mp4'),
(11, 'Pied Piper', 'Erlich Bachman', 'Decentralized Free network supported by Piper Coin', '', '');

-- --------------------------------------------------------

--
-- 表的结构 `thumbs`
--

CREATE TABLE `thumbs` (
  `id` int(11) NOT NULL,
  `thumbUser` varchar(100) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- 转存表中的数据 `thumbs`
--

INSERT INTO `thumbs` (`id`, `thumbUser`) VALUES
(2, 'Palette');

-- --------------------------------------------------------

--
-- 表的结构 `users`
--

CREATE TABLE `users` (
  `username` varchar(100) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `password` varchar(100) NOT NULL,
  `userImageUrl` varchar(100) NOT NULL,
  `phone` varchar(20) NOT NULL,
  `email` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- 转存表中的数据 `users`
--

INSERT INTO `users` (`username`, `password`, `userImageUrl`, `phone`, `email`) VALUES
('Erlich Bachman', '4a61808b653977490e42eb79715b0c43', 'Erlich Bachman.png', '12212211212', '12434213@kwrjfsdlsdf'),
('Fucker', 'd48975694b7fdeb8f85d9900b1e7b692', 'Fucker.jpg', '55552121465', '5555ffff4@c.d'),
('Fuckers', 'd48975694b7fdeb8f85d9900b1e7b692', 'Fuckers.jpg', '65465465456', 'cefd654@de.e'),
('JianYang', '4a61808b653977490e42eb79715b0c43', 'JianYang.png', '123123123123132', '1231221312@a.1'),
('Jianyang', '90828507578daf8a1f12c721df5fdcb6', 'Jianyang.png', '12121212121212', 'a@2.2'),
('Palette', 'f2fc56ae4fc34694b5fded4459a4e9b6', 'Palette.jpg', '13432874719', '1231231@qq.sax'),
('aaaaaa', '4a61808b653977490e42eb79715b0c43', 'aaaaaa.png', '123123123312213312', '123312132@231.123'),
('aaaaaaa', '426c06149c9a88c9afa3e3a3448def0a', 'aaaaaaa.png', '12121212121212121', '123ksd@sd.as'),
('asssss', '90828507578daf8a1f12c721df5fdcb6', 'asssss.png', '1212121221212121', '121212@1.1'),
('copyright', '90828507578daf8a1f12c721df5fdcb6', 'copyright.png', '111111111111111111', '1231321123@qq.com'),
('fuckershit', 'd48975694b7fdeb8f85d9900b1e7b692', 'fuckershit.png', '45645645645', '654654csd@de.ed'),
('lalala', '90828507578daf8a1f12c721df5fdcb6', 'lalala.png', '1212112121122112', 'aa@asd.a'),
('lalalaa', '426c06149c9a88c9afa3e3a3448def0a', 'lalalaa.png', '121212121212121211', '1212212@12121212.1'),
('richard', 'd48975694b7fdeb8f85d9900b1e7b692', 'richard.png', '65465465464', 'sdc654@de.ed'),
('sdcdccds', 'd48975694b7fdeb8f85d9900b1e7b692', 'sdcdccds.png', '45665465445', 'csd654@dedde.d');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `comments`
--
ALTER TABLE `comments`
  ADD KEY `id` (`id`),
  ADD KEY `comUser` (`comUser`);

--
-- Indexes for table `posts`
--
ALTER TABLE `posts`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `thumbs`
--
ALTER TABLE `thumbs`
  ADD KEY `id` (`id`),
  ADD KEY `thumbUser` (`thumbUser`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`username`);

--
-- 在导出的表使用AUTO_INCREMENT
--

--
-- 使用表AUTO_INCREMENT `posts`
--
ALTER TABLE `posts`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- 限制导出的表
--

--
-- 限制表 `comments`
--
ALTER TABLE `comments`
  ADD CONSTRAINT `comments_ibfk_3` FOREIGN KEY (`comUser`) REFERENCES `users` (`username`),
  ADD CONSTRAINT `comments_ibfk_2` FOREIGN KEY (`id`) REFERENCES `posts` (`id`);

--
-- 限制表 `thumbs`
--
ALTER TABLE `thumbs`
  ADD CONSTRAINT `thumbs_ibfk_3` FOREIGN KEY (`thumbUser`) REFERENCES `users` (`username`),
  ADD CONSTRAINT `thumbs_ibfk_2` FOREIGN KEY (`id`) REFERENCES `posts` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
