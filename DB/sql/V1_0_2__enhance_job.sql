ALTER TABLE `scs_logbook`.`job` 
ADD COLUMN `citysourceid` INT NOT NULL AFTER `owner`,
ADD COLUMN `citydestinationid` INT NOT NULL AFTER `citysourceid`,
ADD COLUMN `companydestinationid` INT NOT NULL AFTER `citydestinationid`,
ADD COLUMN `companysourceid` INT NOT NULL AFTER `companydestinationid`,
ADD COLUMN `cargomass` FLOAT NOT NULL AFTER `companysourceid`,
ADD COLUMN `cargoid` INT NOT NULL AFTER `cargomass`;