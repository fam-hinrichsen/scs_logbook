CREATE TABLE `scs_logbook`.`log` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `date` DATETIME NOT NULL,
  `thread` VARCHAR(255) NOT NULL,
  `level` VARCHAR(50) NOT NULL,
  `logger` VARCHAR(255) NOT NULL,
  `message` VARCHAR(4000) NOT NULL,
  `exception` VARCHAR(2000) NULL,
  PRIMARY KEY (`id`));
