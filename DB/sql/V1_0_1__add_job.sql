CREATE TABLE `job` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) COLLATE utf8_bin NOT NULL,
  `income` float NOT NULL,
  `distance` float NOT NULL,
  `owner` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_owner_idx` (`owner`),
  CONSTRAINT `fk_owner` FOREIGN KEY (`owner`) REFERENCES `user` (`iduser`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
