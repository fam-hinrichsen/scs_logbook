ALTER TABLE `scs_logbook`.`city` 
ADD UNIQUE INDEX `name_UNIQUE` (`name` ASC) VISIBLE;
;

ALTER TABLE `scs_logbook`.`cargo` 
ADD UNIQUE INDEX `name_UNIQUE` (`name` ASC) VISIBLE;
;

ALTER TABLE `scs_logbook`.`company` 
ADD UNIQUE INDEX `name_UNIQUE` (`name` ASC) VISIBLE;
;


ALTER TABLE `scs_logbook`.`job` 
ADD INDEX `fk_city_destination_idx` (`citydestinationid` ASC) VISIBLE,
ADD INDEX `fk_city_source_idx` (`citysourceid` ASC) VISIBLE,
ADD INDEX `fk_cargo_idx` (`cargoid` ASC) VISIBLE,
ADD INDEX `fk_company_source_idx` ( `companysourceid` ASC) VISIBLE,
ADD INDEX `fk_company_destination_idx` (`companydestinationid` ASC) VISIBLE;
;

ALTER TABLE `scs_logbook`.`job` 
;
ALTER TABLE `scs_logbook`.`job` 
ADD CONSTRAINT `fk_city_source`
  FOREIGN KEY (`citysourceid`)
  REFERENCES `scs_logbook`.`city` (`idcity`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_city_destination`
  FOREIGN KEY (`citydestinationid`)
  REFERENCES `scs_logbook`.`city` (`idcity`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_cargo`
  FOREIGN KEY (`cargoid`)
  REFERENCES `scs_logbook`.`cargo` (`idcargo`)
  ON DELETE RESTRICT
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_company_destination`
  FOREIGN KEY (`companydestinationid`)
  REFERENCES `scs_logbook`.`company` (`idcompany` )
  ON DELETE RESTRICT
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_company_source`
  FOREIGN KEY ( `companysourceid`)
  REFERENCES `scs_logbook`.`company` (`idcompany`)
  ON DELETE RESTRICT
  ON UPDATE NO ACTION;